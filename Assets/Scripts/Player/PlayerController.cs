using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static string PlayerLayer = "Player";
    
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerRotateController _playerRotateController;
    [SerializeField] private PlayerShootingController _playerShootingController;
    
    private PlayerInfoManager _playerInfoManager;

    private void Awake()
    {   
        _playerInputHandler.MoveEvent += OnMove;
        _playerInputHandler.JumpEvent += OnJump;
        _playerInputHandler.RotateEvent += OnRotate;
        _playerInputHandler.ShootEvent += OnShoot;
    }

    private void Start()
    {
        _playerInfoManager = SL.Get<PlayerInfoManager>();
        _playerInfoManager.PlayerTransform = transform;
    }

    private void OnMove(Vector3 direction)
    {
        _playerMovementController.Move(direction);  
    }

    private void OnJump()
    {
        _playerMovementController.Jump();
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
