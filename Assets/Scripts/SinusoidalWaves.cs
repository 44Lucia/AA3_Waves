using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SinusoidalWaves : MonoBehaviour
{
    public float amplitude = 1f;        
    public float wavelength = 5f;     
    public float frequency = 1f;          
    public float phase = 0f;             
    public Vector2 direction = new Vector2(1, 0); 

    private WaterMesh waterMesh;
    private Vector3[] baseVertices;       
    private Vector3[] displacedVertices;

    private float speed;

    void Start()
    {
        waterMesh = GetComponent<WaterMesh>();

        // Get base vertices of the WaterMesh
        baseVertices = waterMesh.vertices;
        displacedVertices = new Vector3[baseVertices.Length];

        direction.Normalize();

        speed = frequency * wavelength;
    }

    void Update()
    {
        float time = Time.time;

        for (int i = 0; i < baseVertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            float xz = Vector2.Dot(new Vector2(vertex.x, vertex.z), direction);
            float y = amplitude * Mathf.Sin((2 * Mathf.PI / wavelength) * (xz - speed * time) + phase);
            displacedVertices[i] = new Vector3(vertex.x, y, vertex.z);
        }

        // Updates the mesh with the shifted vertices
        waterMesh.UpdateMeshVertices(displacedVertices);
    }
}
