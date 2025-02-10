using System;
using UnityEngine;

public class PlayerRotateController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _yRotationLimit;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Rotate(Vector2 direction)
    {
        float cameraXRotation = _cameraTransform.localEulerAngles.x;
        
        if(cameraXRotation > 180)
        {
            cameraXRotation = cameraXRotation - 360;
        }
        
        cameraXRotation += direction.x;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -_yRotationLimit, _yRotationLimit);
        
        _cameraTransform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);
        transform.Rotate(new Vector3(0, direction.y, 0));
    }
}
