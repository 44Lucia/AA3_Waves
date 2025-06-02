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

    // get shader parameters
    float A;
    float L;
    float S;
    Vector2 D;
    float phi;
    bool useGerstner;

    private Rigidbody rb;
    private Material waterMaterial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (water.TryGetComponent(out Renderer renderer)) { waterMaterial = renderer.material; }
        else { Debug.LogError("No renderer found on the water object"); }

        UpdateShaderVariables();
    }

    private void Update() { UpdateShaderVariables(); }

    private void FixedUpdate()
    {
        // calculate flotability
        D.Normalize();
        float k = 2 * Mathf.PI / L;
        Vector3 pos = transform.position;
        float dot = Vector2.Dot(D, new(pos.x, pos.z));
        float f = k * (dot - S * Time.time) + phi;
        float cosF = Mathf.Cos(f);
        float sinF = Mathf.Sin(f);

        // rotate the object to align with the wave
        Vector3 tangent = new(-D.x * k * A * cosF, 1f, -D.y * k * A * cosF);
        Quaternion tilt = Quaternion.FromToRotation(Vector3.up, tangent.normalized);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, tilt, Time.fixedDeltaTime * 2f));

        float objectBottom = pos.y - objectHeightOffset;
        float immersion = GetWaterHeight() - objectBottom;

        if (immersion > 0)
        {
            float immersionRatio = Mathf.Clamp01(immersion / objectHeight);
            float displacedVolume = objectVolume * immersionRatio;
            float buoyantForceMagnitude = fluidDensity * (-Physics.gravity.y) * displacedVolume;

            rb.AddForce(Vector3.up * buoyantForceMagnitude, ForceMode.Acceleration);
        }
    }

    private void UpdateShaderVariables()
    {
        if (waterMaterial == null) { return; }

        // get shader parameters
        A = waterMaterial.GetFloat(AmplitudeID);
        L = waterMaterial.GetFloat(WavelengthID);
        S = waterMaterial.GetFloat(SpeedID);
        D = waterMaterial.GetVector(DirectionID);
        phi = waterMaterial.GetFloat(PhaseID);
        useGerstner = waterMaterial.GetFloat(UseGerstnerID) == 1f;
    }

    private float GetWaterHeight()
    {
        if (useGerstner)
        {
            float k = 2f * Mathf.PI / L;
            float c = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) / k);
            float f = k * (Vector2.Dot(new Vector2(D.x, D.y).normalized, new Vector2(transform.position.x, transform.position.z)) - c * Time.time + phi);
            return A * Mathf.Cos(f);
        }
        else
        {
            D.Normalize();
            float k = 2f * Mathf.PI / L;
            float wavePhase = Vector2.Dot(new(D.x, D.y), new(transform.position.x, transform.position.z)) - S * Time.time + phi;
            return A * Mathf.Sin(k * wavePhase);
        }
    }
}