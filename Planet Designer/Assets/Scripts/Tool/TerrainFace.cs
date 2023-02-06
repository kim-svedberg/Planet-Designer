using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TerrainFace
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    private int resolution;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;

    private Vector3[] vertices;
    private int[] triangles;

    public MeshFilter MeshFilter => meshFilter;
    public Vector3[] Vertices => vertices;
    public int[] Triangles => triangles;

    public TerrainFace(Transform parent, Vector3 localUp, Material material)
    {
        this.localUp = localUp;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);

        GameObject meshObj = new GameObject("Mesh " + parent.childCount);
        meshObj.transform.parent = parent;
        meshFilter = meshObj.AddComponent<MeshFilter>();
        meshRenderer = meshObj.AddComponent<MeshRenderer>();

        mesh = meshFilter.sharedMesh = new Mesh();
        meshRenderer.material = material;
    }

    public void ReconstructData(SphereSettings sphereSettings)
    {
        resolution = sphereSettings.resolution;
        vertices = new Vector3[resolution * resolution];
        triangles = new int[(resolution - 1) * (resolution - 1) * 6];

        int triangleIndex = 0;
        int vertexIndex = 0;

        Vector2 percent;
        Vector3 pointOnUnitCube, pointOnUnitSphere;

        for (int y = 0; y < resolution; ++y)
        {
            for (int x = 0; x < resolution; ++x)
            {
                percent = new Vector2(x, y) / (resolution - 1);

                percent = new Vector2(
                    percent.x.CustomSmoothstep(sphereSettings.vertexDistributionBias),
                    percent.y.CustomSmoothstep(sphereSettings.vertexDistributionBias));

                pointOnUnitCube = localUp + (percent.x - 0.5f) * 2f * axisA + (percent.y - 0.5f) * 2f * axisB;
                pointOnUnitSphere = pointOnUnitCube.normalized;

                vertices[vertexIndex] = pointOnUnitSphere * sphereSettings.radius;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triangleIndex] = vertexIndex;
                    triangles[triangleIndex + 1] = vertexIndex + resolution + 1;
                    triangles[triangleIndex + 2] = vertexIndex + resolution;

                    triangles[triangleIndex + 3] = vertexIndex;
                    triangles[triangleIndex + 4] = vertexIndex + 1;
                    triangles[triangleIndex + 5] = vertexIndex + resolution + 1;
                    triangleIndex += 6;
                }

                ++vertexIndex;
            }
        }
    }

    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}