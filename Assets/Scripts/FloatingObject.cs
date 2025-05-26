using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingObject : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Transform water;
    [SerializeField] private float floatStrength = 10f;
    private float objectHeightOffset = 0.5f; // pivote del objeto (aprox)

    [Header("Shader properties")]
    private static readonly int AmplitudeID = Shader.PropertyToID("_Amplitude");
    private static readonly int WavelengthID = Shader.PropertyToID("_Length");
    private static readonly int SpeedID = Shader.PropertyToID("_Speed");
    private static readonly int DirectionID = Shader.PropertyToID("_Direction");
    private static readonly int PhaseID = Shader.PropertyToID("_Phase");
    private static readonly int useGerstnerID = Shader.PropertyToID("_useGerstner");

    private Rigidbody rb;
    private Material waterMaterial;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (water.TryGetComponent(out Renderer renderer)) { waterMaterial = renderer.material; }
        else { Debug.LogError("No se encontró un Renderer en el objeto del agua."); }
    }

    private void FixedUpdate()
    {
        if (waterMaterial == null) { return; }

        // Obtener parámetros del shader
        float A = waterMaterial.GetFloat(AmplitudeID);
        float L = waterMaterial.GetFloat(WavelengthID);
        float S = waterMaterial.GetFloat(SpeedID);
        Vector2 D = waterMaterial.GetVector(DirectionID);
        float phase = waterMaterial.GetFloat(PhaseID);
        bool useGerstner = waterMaterial.GetFloat(useGerstnerID) == 1f;

        D.Normalize();
        float k = 2 * Mathf.PI / L;
        Vector3 pos = transform.position;
        float dot = Vector2.Dot(D, new Vector2(pos.x, pos.z));
        float f = k * (dot - S * Time.time) + phase;

        float height = 0f;

        // Calcular fuerza de flotación
        if (useGerstner)
        {
            // TO DO
            // por ahora que haga lo mismo que el sinusoidal
            height = A * Mathf.Sin(f);
        }
        else
        {
            height = A * Mathf.Sin(f);
        }

        // Aplicar fuerza de flotación
        float diff = height - (pos.y - objectHeightOffset);
        if (diff > 0f)
        {
            float force = floatStrength * diff;
            rb.AddForce(Vector3.up * force, ForceMode.Acceleration);
        }

        // Rotación
        float cosF = Mathf.Cos(f);
        Vector3 tangent = new(-D.x * k * A * cosF, 1f, -D.y * k * A * cosF);
        Quaternion tilt = Quaternion.FromToRotation(Vector3.up, tangent.normalized);
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, tilt, Time.fixedDeltaTime * 2f));
    }
}