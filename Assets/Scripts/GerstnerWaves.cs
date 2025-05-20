using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GerstnerWaves : MonoBehaviour
{
    [System.Serializable]
    public struct Wave
    {
        public float amplitude;    
        public float wavelength;  
        public float frequency;   
        public float phase;       
        public Vector2 direction;   
    }

    [Tooltip("Define one or more Gerstner waves here")]
    public Wave[] waves;

    private Mesh mesh;
    private Vector3[] baseVertices;
    private Vector3[] displacedVertices;

    private Vector2[] dirs;
    private float[] k;      
    private float[] speed;  

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        // We clone the original vertices (flat mesh created in WaterMesh)
        baseVertices = mesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];

        int n = waves.Length;
        dirs = new Vector2[n];
        k = new float[n];
        speed = new float[n];

        for (int i = 0; i < n; i++)
        {
            // Normalize address
            dirs[i] = waves[i].direction.normalized;
            // Precalculate wavenumber and speed
            k[i] = 2 * Mathf.PI / waves[i].wavelength;
            speed[i] = waves[i].frequency * waves[i].wavelength;
        }
    }

    void Update()
    {
        float time = Time.time;

        for (int vi = 0; vi < baseVertices.Length; vi++)
        {
            Vector3 v0 = baseVertices[vi];
            Vector2 posXZ = new Vector2(v0.x, v0.z);

            float yOffset = 0f;
            float xOffset = 0f;
            float zOffset = 0f;

            // Add contribution of each wave
            for (int i = 0; i < waves.Length; i++)
            {
                float d = Vector2.Dot(dirs[i], posXZ);
                float arg = k[i] * (d - speed[i] * time) + waves[i].phase;
                float sin = Mathf.Sin(arg);
                float cos = Mathf.Cos(arg);

                yOffset += waves[i].amplitude * sin;
                xOffset += waves[i].amplitude * dirs[i].x * cos;
                zOffset += waves[i].amplitude * dirs[i].y * cos;
            }

            displacedVertices[vi] = new Vector3(
                v0.x + xOffset,
                yOffset,
                v0.z + zOffset
            );
        }

        // Update the mesh
        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
