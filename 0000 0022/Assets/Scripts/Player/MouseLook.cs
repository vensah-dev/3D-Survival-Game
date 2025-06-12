using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float MouseSensitivity = 280f;
    private float xrotation = 0f;
    public Camera cam;
    public Transform playerBody;
    public GameObject MainInventoryGroup;



    void Update()
    {
        if(MainInventoryGroup.activeSelf)
        {
            CursorUnlock();
        }
        else
        {
            CursorLock();
        }

        float mouseX = Input.GetAxis("Mouse X") * MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSensitivity * Time.deltaTime;
        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);
        playerBody.transform.Rotate(Vector3.up * mouseX);
    }

    void CursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;

    }

    void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

}
