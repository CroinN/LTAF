using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static string PlayerLayer = "Player";
    
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerRotateController _playerRotateController;
    [SerializeField] private PlayerShootingController _playerShootingController;

    private void Awake()
    {   
        _playerInputHandler.MoveEvent += OnMove;
        _playerInputHandler.JumpEvent += OnJump;
        _playerInputHandler.RotateEvent += OnRotate;
        _playerInputHandler.DashEvent += OnDash;
        _playerInputHandler.ShootEvent += OnShoot;
    }

    private void OnMove(Vector3 direction)
    {
        _playerMovementController.OnMove(direction);  
    }

    private void OnDash()
    {
        _playerMovementController.OnDash();
    }

    private void OnJump()
    {
        _playerMovementController.OnJump();
    }

    private void OnRotate(Vector2 direction)
    {
        _playerRotateController.Rotate(direction);
    }
    private void OnShoot()
    {
        _playerShootingController.Shoot();
    }

}
