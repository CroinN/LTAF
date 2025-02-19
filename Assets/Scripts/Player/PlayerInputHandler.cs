using System;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector3> MoveEvent;
    public event Action DashEvent;
    public event Action JumpEvent;
    public event Action<Vector2> RotateEvent;
    public event Action ShootEvent;
    
    private InputProvider _inputProvider;

    private void Start()
    {
        _inputProvider = SL.Get<InputProvider>();
        _inputProvider.MoveEvent += OnMove;
        _inputProvider.DashEvent += OnDash;
        _inputProvider.JumpEvent += OnJump;
        _inputProvider.RotateEvent += OnRotate;
        _inputProvider.ShootEvent += OnShoot;
    }

    private void OnMove(Vector3 direction)
    {
        MoveEvent?.Invoke(direction);
    }

    private void OnDash()
    {
        DashEvent?.Invoke();
    }

    private void OnJump()
    {
        JumpEvent?.Invoke();
    }

    private void OnRotate(Vector2 direction)
    {
        RotateEvent?.Invoke(direction);
    }

    private void OnShoot()
    {
        ShootEvent?.Invoke();
    }
}
