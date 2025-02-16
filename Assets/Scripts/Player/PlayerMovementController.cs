using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct MovementStats
{
    public float walkSpeed;
    public float dashForce;
    public float jumpForce;
    public float maxVelocityChange;
    public float groundCheckRadius;
    public int jumpInSecondLimit;
}

public class PlayerMovementController : MonoBehaviour
{
    public event Action JumpEvent;
    public event Action DashEvent;

    [SerializeField] private PlayerStaminaController _staminaController;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private MovementStats _movementStats;

    private Rigidbody _rigidbody;
    private Vector3 _direciton;
    private bool _isMoving;
    private int _lastSecondJumpCount;

    public bool IsGrounded {
        get {
            Collider[] colliders = Physics.OverlapSphere(_groundCheckPosition.position, _movementStats.groundCheckRadius, _groundCheckLayerMask);
            return colliders.Length > 0;
        }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void OnMove(Vector3 vector)
    {
        Vector3 targetDirection = transform.forward * vector.z + transform.right * vector.x;
        _direciton = targetDirection.normalized;
    }

    private void Move()
    {
        float speed = _movementStats.walkSpeed;
        Vector3 targetVelocity = _direciton;

        if (targetVelocity.magnitude > 0)
        {
            _isMoving = true;
            targetVelocity = targetVelocity * speed;

            Vector3 velocity = _rigidbody.linearVelocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_movementStats.maxVelocityChange, _movementStats.maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_movementStats.maxVelocityChange, _movementStats.maxVelocityChange);
            velocityChange.y = 0;

            _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            _isMoving = false;
        }
    }

    public void OnDash()
    {
        if (_staminaController.CanDash)
        {
            _staminaController.OnDash();
            Dash();
            DashEvent?.Invoke();
        }
    }

    private void Dash()
    {
        _rigidbody.AddForce(_direciton * _movementStats.dashForce, ForceMode.Impulse);
    }

    public void OnJump()
    {
        if (CanJump())
        {
            StartCoroutine(JumpCooldown());
            _staminaController.OnJump();
            Jump();
            JumpEvent?.Invoke();
        }
    }

    private IEnumerator JumpCooldown()
    {
        _lastSecondJumpCount++;
        yield return new WaitForSeconds(1f);
        _lastSecondJumpCount--;
    }

    private void Jump()
    {
        _rigidbody.AddForce(new Vector3(0f, _movementStats.jumpForce, 0f), ForceMode.Impulse);
    }

    public bool CanJump ()
    {
        bool hasStamina = _staminaController.CanJump;
        bool hasJumpLimit = _lastSecondJumpCount < _movementStats.jumpInSecondLimit;

        bool canJump = IsGrounded && hasStamina && hasJumpLimit;
        
        return canJump;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPosition.position, _movementStats.groundCheckRadius);
    }
}
