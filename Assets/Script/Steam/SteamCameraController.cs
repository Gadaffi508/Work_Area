using System;
using UnityEngine;

public class SteamCameraController : MonoBehaviour
{
    public const float Ymin = -50f;
    public const float Ymax = 50f;

    public Transform lookAt;

    public float distance = 3;
    public float sensitivity = 90;

    private float currentX = 0f;
    private float currentY = 25f;

    private bool _clickEscape = false;

    private void LateUpdate()
    {
        currentX += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, Ymin, Ymax);
        
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
        Vector3 direction = new Vector3(0,0, -distance);
        transform.position = lookAt.position + rotation * direction;
        
        transform.LookAt(lookAt.position);

        if (_clickEscape == true)
            Cursor.lockState = CursorLockMode.Locked;
        else 
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetKeyDown(KeyCode.Escape))
            _clickEscape = !_clickEscape;
    }
}
