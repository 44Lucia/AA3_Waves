using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class WaterMesh : MonoBehaviour
{
    public int sizeX = 50; 
    public int sizeZ = 50; 
    public float gridSpacing = 1f;  

    private Mesh mesh;
    public Vector3[] vertices { get; private set; }
    private int[] triangles;

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateMesh();
        UpdateMesh();
    }

    void CreateMesh()
    {
        // Crear array de vértices
        vertices = new Vector3[(sizeX + 1) * (sizeZ + 1)];

        for (int z = 0; z <= sizeZ; z++)
        {
            for (int x = 0; x <= sizeX; x++)
            {
                vertices[z * (sizeX + 1) + x] = new Vector3(x * gridSpacing, 0, z * gridSpacing);
            }
        }

        // Create array of triangles
        triangles = new int[sizeX * sizeZ * 6]; // 2 triangles per square, 3 indices each
        int triIndex = 0;

        for (int z = 0; z < sizeZ; z++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                int topLeft = z * (sizeX + 1) + x;
                int topRight = topLeft + 1;
                int bottomLeft = topLeft + sizeX + 1;
                int bottomRight = bottomLeft + 1;

                // First triangle
                triangles[triIndex++] = topLeft;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = topRight;

                // Second triangle
                triangles[triIndex++] = topRight;
                triangles[triIndex++] = bottomLeft;
                triangles[triIndex++] = bottomRight;
            }
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); // For the lighting to be correct
    }

    public void SetVertexHeight(int index, float y)
    {
        if (index >= 0 && index < vertices.Length)
        {
            Vector3 v = vertices[index];
            vertices[index] = new Vector3(v.x, y, v.z);
        }
    }

    public void RefreshMesh()
    {
        UpdateMesh();
    }
    public void UpdateMeshVertices(Vector3[] newVertices)
    {
        mesh.vertices = newVertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}
