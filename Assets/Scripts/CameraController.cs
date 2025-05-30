using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject[] targetsToFollow;
    private int currentTargetIndex = 0;

    [Header("Movement")]
    [SerializeField, Min(0)] private Vector2 distanceLimits = new(1.5f, 50f);
    [SerializeField, Min(0)] private float m_cameraLerp = 20f;
    private float m_targetDistance;
    private float rotationX, rotationY;

    private float timeScaleMultiplier = 1.0f;

    private void Awake() { m_targetDistance = distanceLimits.y / 2; }

    private void LateUpdate()
    {
        if (targetsToFollow.Length == 0) { return; } // no targets to follow

        HandleRotation();
        HandleMovement();

        HandleZoom();
        HandleTargetSelection();
        HandleTimeScale();
    }

    private void HandleRotation()
    {
        // WASD
        rotationX += Input.GetAxis("Vertical") / 3;
        rotationY -= Input.GetAxis("Horizontal") / 3;

        // MOUSE DELTA
        if (Input.GetMouseButton(0)) // drag screen
        {
            rotationX -= Input.GetAxis("Mouse Y") * 2.0f;
            rotationY += Input.GetAxis("Mouse X") * 2.0f;
        }

        rotationX = Mathf.Clamp(rotationX, -40, 50f); // -40,50 --> camera angle limits
        transform.eulerAngles = new(rotationX, rotationY, 0);
    }

    private void HandleMovement()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetsToFollow[currentTargetIndex].transform.position - transform.forward * m_targetDistance,
            m_cameraLerp * Time.deltaTime
        );
    }

    private void HandleZoom()
    {
        m_targetDistance -= Input.mouseScrollDelta.y;

        if (Input.GetKey(KeyCode.DownArrow)) { m_targetDistance += 0.1f; } // v = -zoom
        else if (Input.GetKey(KeyCode.UpArrow)) { m_targetDistance -= 0.1f; } // ^ = +zoom

        m_targetDistance = Mathf.Clamp(m_targetDistance, distanceLimits.x, distanceLimits.y); // zoom limits
    }

    private void HandleTargetSelection()
    {
        if (targetsToFollow.Length == 0) { return; } // no targets to follow

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentTargetIndex++;
            if (currentTargetIndex >= targetsToFollow.Length) { currentTargetIndex = 0; }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            currentTargetIndex--;
            if (currentTargetIndex < 0) { currentTargetIndex = targetsToFollow.Length - 1; }
        }
    }

    private void HandleTimeScale()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            timeScaleMultiplier *= 2.0f;
            Time.timeScale = Mathf.Clamp(timeScaleMultiplier, 1.0f, 16.0f);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            timeScaleMultiplier = 1.0f;
            Time.timeScale = 1.0f;
        }
    }
}