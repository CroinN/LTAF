using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _sprintSpeed;
    [SerializeField] private float _maxVelocityChange;
    [SerializeField] private float _jumpForce;
    
    private Rigidbody _rigidbody;
    private Vector3 _direciton;
    private bool _isSprinting;
    private bool _isSprintCooldown;

    public bool IsGrounded { 
        get {
            Collider[] colliders = Physics.OverlapSphere(_groundCheckPosition.position, _groundCheckRadius, _groundCheckLayerMask);
            return colliders.Length > 0;
        }} 

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
        _direciton = vector;
    }

    private void Move()
    {
        float speed = _isSprinting ? _sprintSpeed : _walkSpeed;
        Vector3 targetVelocity = transform.forward * _direciton.z + transform.right * _direciton.x;

        targetVelocity = transform.TransformDirection(targetVelocity) * speed;

        Vector3 velocity = _rigidbody.linearVelocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
        velocityChange.y = 0;

        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    public void OnSprint(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    public void OnJump()
    {
        Jump();
    }

    private void Jump()
    {
        Collider[] colliders = Physics.OverlapSphere(_groundCheckPosition.position, _groundCheckRadius, _groundCheckLayerMask);

        if (colliders.Length > 0)
        {
            _rigidbody.AddForce(new Vector3(0f, _jumpForce, 0f), ForceMode.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_groundCheckPosition.position, _groundCheckRadius);
    }
}
