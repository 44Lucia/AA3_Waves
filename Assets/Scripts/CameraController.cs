using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sprintMultiplier = 3f;
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float verticalSpeed = 5f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // wasd
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed *= sprintMultiplier; // Más rápido si se mantiene Shift
        }

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) { moveDir += forward; }
        if (Input.GetKey(KeyCode.S)) { moveDir -= forward; }
        if (Input.GetKey(KeyCode.D)) { moveDir += right; }
        if (Input.GetKey(KeyCode.A)) { moveDir -= right; }

        moveDir = moveDir.normalized * speed * Time.deltaTime;

        // up and down
        if (Input.GetKey(KeyCode.E)) { moveDir += Vector3.up * verticalSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.Q)) { moveDir -= Vector3.up * verticalSpeed * Time.deltaTime; }

        transform.position += moveDir;

        // Rotación con mouse
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // angle limit

        transform.localRotation = Quaternion.Euler(-rotationY, rotationX, 0f);

        // toggle cursor visibility
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}