using NUnit.Framework;
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

    private UIManager _uiManager;
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

    private void Start()
    {
        _uiManager = SL.Get<UIManager>();
        _uiManager.UpdateStamina(_stamina, _staminaStats.maxStamina);
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
            float staminaRegenAmount =  _staminaStats.staminaRegenerationAmount * Time.deltaTime;
            ChangeStamina(staminaRegenAmount);
        }
    }

    public void OnSprint(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    private void Sprint()
    {
        float staminacost = _staminaStats.sprintStaminaCost * Time.deltaTime;
        if (HasEnoughStamina(staminacost))
        {
            ChangeStamina(-staminacost);
        }
    }

    public void OnJump()
    {
        float staminacost = _staminaStats.jumpStaminaCost;
        if (IsGrounded && HasEnoughStamina(staminacost))
        {
            ChangeStamina(-staminacost);
            Jump();
        }
    }

    private void ChangeStamina(float amount)
    {
        float newStamina = _stamina + amount;
        Assert.IsTrue(newStamina >= 0 && newStamina <= _staminaStats.maxStamina, "Stamina is out of bounds");
        _stamina = Mathf.Clamp(newStamina, 0, _staminaStats.maxStamina);
        _uiManager.UpdateStamina(_stamina, _staminaStats.maxStamina);
    }

    private bool HasEnoughStamina(float amount)
    {
        return _stamina >= amount;
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
