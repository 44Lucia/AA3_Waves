using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [Header("Configuració física de la boya")]
    [SerializeField] private Transform water;
    [SerializeField] private float objectVolume = 0.52f;
    [SerializeField] private float objectHeight = 1f;
    [SerializeField] private float objectHeightOffset = 0.5f;
    private float fluidDensity = 1025f; // agua salada

    // shader parameters access
    private static readonly int AmplitudeID = Shader.PropertyToID("_Amplitude");
    private static readonly int WavelengthID = Shader.PropertyToID("_Length");
    private static readonly int SpeedID = Shader.PropertyToID("_Speed");
    private static readonly int DirectionID = Shader.PropertyToID("_Direction");
    private static readonly int PhaseID = Shader.PropertyToID("_Phase");
    private static readonly int UseGerstnerID = Shader.PropertyToID("_useGerstner");

    private Rigidbody rb;
    private Material waterMaterial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (water.TryGetComponent(out Renderer renderer)) { waterMaterial = renderer.material; }
        else { Debug.LogError("No renderer found on the water object"); }
    }

    void FixedUpdate()
    {
        if (waterMaterial == null) { return; }

        // get shader parameters
        float A = waterMaterial.GetFloat(AmplitudeID);
        float L = waterMaterial.GetFloat(WavelengthID);
        float S = waterMaterial.GetFloat(SpeedID);
        Vector2 D = waterMaterial.GetVector(DirectionID);
        float phi = waterMaterial.GetFloat(PhaseID);
        bool useGerstner = waterMaterial.GetFloat(UseGerstnerID) == 1f;

        Debug.Log($"A: {A}, L: {L}, S: {S}, D: {D}, phi: {phi}, useGerstner: {useGerstner}");

        // calculate flotability
        D.Normalize();
        float k = 2 * Mathf.PI / L;
        Vector3 pos = transform.position;
        float dot = Vector2.Dot(D, new Vector2(pos.x, pos.z));
        float f = k * (dot - S * Time.time) + phi;

        float waterHeight;

        // apply flotability
        if (useGerstner)
        {
            float cosF = Mathf.Cos(f);
            float sinF = Mathf.Sin(f);

            Vector3 displacedPos = pos;
            displacedPos.x += A * D.x * cosF;
            displacedPos.y += A * sinF;
            displacedPos.z += A * D.y * cosF;

            waterHeight = displacedPos.y;

            Vector3 tangent = new(-D.x * k * A * cosF, 1f, -D.y * k * A * cosF);
            Quaternion tilt = Quaternion.FromToRotation(Vector3.up, tangent.normalized);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, tilt, Time.fixedDeltaTime * 2f));
        }
        else { waterHeight = A * Mathf.Sin(f); }

        float objectBottom = pos.y - objectHeightOffset;
        float immersion = waterHeight - objectBottom;

        if (immersion > 0)
        {
            float immersionRatio = Mathf.Clamp01(immersion / objectHeight);
            float displacedVolume = objectVolume * immersionRatio;
            float buoyantForceMagnitude = fluidDensity * (-Physics.gravity.y) * displacedVolume;

            rb.AddForce(Vector3.up * buoyantForceMagnitude, ForceMode.Acceleration);
        }
    }
}