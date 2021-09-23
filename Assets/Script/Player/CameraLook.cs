using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float upAngle = -45f;
    [SerializeField] float downAngle = 45f;

    [SerializeField] float mouseSensitivityX = 450f;
    [SerializeField] float mouseSensitivityY = 320f;

    [SerializeField] Transform playerBody;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    float xRorataion = 0f;
    void Update()
    {
        if (PlayerState.Instance.IsStopRotate)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        xRorataion -= mouseY;

        // xRoration의 값이 최소 -45f, 최대 45f.
        xRorataion = Mathf.Clamp(xRorataion, upAngle, downAngle);

        transform.localRotation = Quaternion.Euler(xRorataion, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
