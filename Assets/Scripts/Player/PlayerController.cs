using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static string PlayerLayer = "Player";
    
    [SerializeField] private PlayerInputHandler _playerInputHandler;
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerRotateController _playerRotateController;

    private void Awake()
    {   
        _playerInputHandler.MoveEvent += OnMove;
        _playerInputHandler.JumpEvent += OnJump;
        _playerInputHandler.RotateEvent += OnRotate;
        _playerInputHandler.SprintEvent += OnSprint;
    }

    private void OnMove(Vector3 direction)
    {
        _playerMovementController.OnMove(direction);  
    }

    private void OnSprint(bool isSprinting)
    {
        _playerMovementController.OnSprint(isSprinting);
    }

    private void OnJump()
    {
        _playerMovementController.OnJump();
    }

    private void OnRotate(Vector2 direction)
    {
        _playerRotateController.Rotate(direction);
    }
}
