using System;
using UnityEngine;

public class InputProvider : MonoBehaviour, IService
{
    public event Action<Vector3> MoveEvent;
    public event Action DashEvent;
    public event Action JumpEvent;
    public event Action<Vector2> RotateEvent;
    public event Action ShootEvent;

    [SerializeField] private KeyCode _moveForwardKey = KeyCode.W;
    [SerializeField] private KeyCode _moveBackwardKey = KeyCode.S;
    [SerializeField] private KeyCode _moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode _moveRightKey = KeyCode.D;
    [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode _shootKey = KeyCode.Mouse0;
    [SerializeField] private float _xSensitivity;
    [SerializeField] private float _ySensitivity;
    
    private bool _isInputEnabled = true;

    private void Awake()
    {
        RegisterService();
    }

    private void OnDestroy()
    {
        UnregisterService();
    }

    private void Update()
    {
        HandleMovementInput();
        HandleRotateInput();
        HandleShootInput();
    }

    private void HandleMovementInput()
    {
        if (_isInputEnabled)
        {
            Vector3 direction = Vector3.zero;

            direction += Input.GetKey(_moveForwardKey) ? Vector3.forward : Vector3.zero;
            direction += Input.GetKey(_moveBackwardKey) ? Vector3.back : Vector3.zero;
            direction += Input.GetKey(_moveLeftKey) ? Vector3.left : Vector3.zero;
            direction += Input.GetKey(_moveRightKey) ? Vector3.right : Vector3.zero;


            MoveEvent?.Invoke(direction);

            bool shouldJump = Input.GetKeyDown(_jumpKey);
            bool shouldDash = Input.GetKeyDown(_dashKey);

            if (shouldJump)
            {
                JumpEvent?.Invoke();
            }
            if (shouldDash)
            {
                DashEvent?.Invoke();
            }
        }
    }

    private void HandleRotateInput()
    {
        if (_isInputEnabled)
        {
            Vector2 rotateDirection = Vector2.zero;
            rotateDirection.x = -Input.GetAxis("Mouse Y") * _ySensitivity;
            rotateDirection.y = Input.GetAxis("Mouse X") * _xSensitivity;
            RotateEvent?.Invoke(rotateDirection);
        }
    }

    private void HandleShootInput()
    {
        if (_isInputEnabled)
        {
            if (Input.GetKeyDown(_shootKey))
            {
                ShootEvent?.Invoke();
            }
        }
    }

    public void EnableInput()
    {
        _isInputEnabled = true;
    }

    public void DisableInput()
    {
        _isInputEnabled = false;
    }

    public void RegisterService()
    {
        SL.Register(this);
    }

    public void UnregisterService()
    {
        SL.Unregister(this);
    }
}
