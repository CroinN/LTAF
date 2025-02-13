using System;
using UnityEngine;

[Serializable]
public struct MovementStats
{
    public float walkSpeed;
    public float sprintSpeed;
    public float maxVelocityChange;
    public float groundCheckRadius;
    public float jumpForce;
}

[Serializable]
public struct StaminaStats
{
    public float maxStamina;
    public float staminaRegenerationAmount;
    public float jumpStaminaCost;
    public float sprintStaminaCost;
}

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private StaminaStats _staminaStats;
    [SerializeField] private MovementStats _movementStats;

    private Rigidbody _rigidbody;
    private Vector3 _direciton;
    private float _stamina;
    private bool _isMoving;
    private bool _isSprinting;

    public bool IsGrounded { 
        get {
            Collider[] colliders = Physics.OverlapSphere(_groundCheckPosition.position, _movementStats.groundCheckRadius, _groundCheckLayerMask);
            return colliders.Length > 0;
        }
    }

    private bool CanSprint => _isSprinting && _isMoving && _stamina >= _staminaStats.sprintStaminaCost;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _stamina = _staminaStats.maxStamina;
    }

    private void FixedUpdate()
    {
        Move();
        RegenerateStamina();
    }

    public void OnMove(Vector3 vector)
    {
        _direciton = vector;
    }

    private void Move()
    {
        float speed = _movementStats.walkSpeed;

        if (CanSprint)
        {
            Sprint();
            speed = _movementStats.sprintSpeed;
        }

        Vector3 targetVelocity = transform.forward * _direciton.z + transform.right * _direciton.x;

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

    private void RegenerateStamina()
    {
        if ((!_isSprinting || !_isMoving) && _stamina < _staminaStats.maxStamina)
        {
            float newStamina = _stamina + _staminaStats.staminaRegenerationAmount * Time.deltaTime;
            _stamina = Mathf.Clamp(newStamina, 0, _staminaStats.maxStamina);
        }
    }

    public void OnSprint(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    private void Sprint()
    {
        float newStamina = _stamina - _staminaStats.sprintStaminaCost * Time.deltaTime;
        _stamina = Mathf.Clamp(newStamina, 0, _staminaStats.maxStamina);
    }

    public void OnJump()
    {
        if (IsGrounded && _stamina >= _staminaStats.jumpStaminaCost)
        {
            _stamina -= _staminaStats.jumpStaminaCost;
            Jump();
        }
    }

    private void Jump()
    {
        _rigidbody.AddForce(new Vector3(0f, _movementStats.jumpForce, 0f), ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPosition.position, _movementStats.groundCheckRadius);
    }
}
