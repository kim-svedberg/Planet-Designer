using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    private Vector3[] normals;
    private Vector2[] uvs;

    public MeshFilter MeshFilter => meshFilter;
    public Vector3[] Vertices => vertices;
    public int[] Triangles => triangles;

    public TerrainFace(Transform parent, Vector3 localUp)
    {
        this.localUp = localUp;
        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);

        GameObject meshObj = new GameObject("Mesh");
        meshObj.transform.parent = parent;
        meshObj.hideFlags = HideFlags.HideInHierarchy;

        meshFilter = meshObj.AddComponent<MeshFilter>();
        meshRenderer = meshObj.AddComponent<MeshRenderer>();

        mesh = meshFilter.sharedMesh = new Mesh();
    }

    public void ReconstructData_NoNormalFix(SphereSettings sphereSettings)
    {
        resolution = sphereSettings.resolution;
        vertices = new Vector3[resolution * resolution];
        triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        uvs = new Vector2[resolution * resolution];

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

                // Set uv of vertex
                uvs[vertexIndex] = new Vector2(percent.x, percent.y);

                // Increment vertexIndex
                ++vertexIndex;
            }
        }
    }

    public void ReconstructData_SeamlessNormals(SphereSettings sphereSettings)
    {
        resolution = sphereSettings.resolution;

        int vertexCount = resolution * resolution;
        int expandedVertexCount = vertexCount + resolution * 4;

        // Triangle count = width * height * 2 triangles per square * 3 vertices per triangle
        int triangleCount = (resolution - 1) * (resolution - 1) * 2 * 3;

        // Expanded count = triangle count + width * 2 triangles per unit * 3 vertices per triangle * 4 sides to expand
        int expandedTriangleCount = triangleCount + (resolution - 1) * 2 * 3 * 4;

        vertices = new Vector3[expandedVertexCount];
        triangles = new int[expandedTriangleCount];
        uvs = new Vector2[vertexCount];

        int vertexIndex = 0;
        int expandedVertexIndex = 0;
        int expandedTriangleIndex = 0;

        Vector3 percent;
        Vector3 pointOnUnitCube;
        bool xEdge, yEdge;

        // Precompute percent.z for edge vertices
        float precomputedZ = (resolution - 2f) / (resolution - 1f);
        precomputedZ = precomputedZ.CustomSmoothstep(sphereSettings.vertexDistributionBias);

        // Loop though all vertices on the mesh
        for (int y = -1; y <= resolution; ++y)
        {
            yEdge = y == -1 || y == resolution;

            for (int x = -1; x <= resolution; ++x)
            {
                xEdge = x == -1 || x == resolution;
                
                // Don't create vertex for the four corners of the expanded mesh
                if (xEdge && yEdge)
                    continue;

                // Calculate vertex position on unit cube
                // (Edge vertices will be clamped to the real mesh by the smoothstep function)

                percent.x = (float)x / (resolution - 1);
                percent.y = (float)y / (resolution - 1);
                percent.z = xEdge || yEdge ? precomputedZ : 1f;

                // Use smoothstep function to bias the vertex distribution in order to lower the distortion in the mesh
                percent.x = percent.x.CustomSmoothstep(sphereSettings.vertexDistributionBias);
                percent.y = percent.y.CustomSmoothstep(sphereSettings.vertexDistributionBias);

                pointOnUnitCube =
                      (percent.z - 0.5f) * 2f * localUp
                    + (percent.x - 0.5f) * 2f * axisA
                    + (percent.y - 0.5f) * 2f * axisB;

                // Normalize unit cube position to get unit sphere position, multipy by radius
                vertices[expandedVertexIndex] = pointOnUnitCube.normalized * sphereSettings.radius;

                // If vertex is not on left or bottom edge
                if (x != resolution && y != resolution)
                {
                    // And if vertex does not expand down left into a corner
                    if (expandedVertexIndex != resolution - 1 &&
                        expandedVertexIndex != expandedVertexCount - resolution - 2 - resolution &&
                        expandedVertexIndex != expandedVertexCount - resolution - 2 )
                    {
                        // Calculate index of vertex below (normally vertexIndex + resolution)
                        int indexBelow = expandedVertexIndex + resolution + (y == -1 || y == resolution - 1 ? 1 : 2);

                        // Create mesh square reaching down left

                        // 1 -
                        // 3 2
                        triangles[expandedTriangleIndex    ] = expandedVertexIndex;
                        triangles[expandedTriangleIndex + 1] = indexBelow + 1;
                        triangles[expandedTriangleIndex + 2] = indexBelow;

                        // 1 2
                        // - 3
                        triangles[expandedTriangleIndex + 3] = expandedVertexIndex;
                        triangles[expandedTriangleIndex + 4] = expandedVertexIndex + 1;
                        triangles[expandedTriangleIndex + 5] = indexBelow + 1;

                        expandedTriangleIndex += 6;
                    }
                }

                // If not edge vertex
                if (!xEdge && !yEdge)
                {
                    // Set uv of vertex
                    uvs[vertexIndex++] = new Vector2(percent.x, percent.y);
                }

                // Increment expandedVertexIndex
                ++expandedVertexIndex;
            }
        }
    }

    public void UpdateMesh(SphereSettings sphereSettings)
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        normals = mesh.normals;
        
        if (sphereSettings.fixEdgeNormals)
            RemoveOuterEdge();

        mesh.SetUVs(0, uvs);
        meshRenderer.sharedMaterial = sphereSettings.material;
    }

    private void RemoveOuterEdge()
    {
        int vertexCount = resolution * resolution;

        // Triangle count = width * height * 2 triangles per square * 3 vertices per triangle
        int triangleCount = (resolution - 1) * (resolution - 1) * 2 * 3;

        Vector3[] newVertices = new Vector3[vertexCount];
        int[] newTriangles = new int[triangleCount];
        Vector3[] newNormals = new Vector3[vertexCount];

        int newVertexIndex = 0;
        int oldVertexIndex = 0;
        int newTriangleIndex = 0;

        bool xEdge, yEdge;

        // Loop though all vertices on the mesh
        for (int y = -1; y <= resolution; ++y)
        {
            yEdge = y == -1 || y == resolution;

            for (int x = -1; x <= resolution; ++x)
            {
                xEdge = x == -1 || x == resolution;

                // No verices exist in the corners
                if (xEdge && yEdge)
                    continue;

                // If edge vertex, don't copy
                if (xEdge || yEdge)
                {
                    ++oldVertexIndex;
                    continue;
                }

                // Copy vertex position and normal
                newVertices[newVertexIndex] = vertices[oldVertexIndex];
                newNormals[newVertexIndex] = normals[oldVertexIndex];
                
                // If new vertex expands down left into a square within the non-expanded mesh
                if (x < resolution - 1 && y < resolution - 1)
                {
                    // Create mesh square reaching down left

                    // 1 -
                    // 3 2
                    newTriangles[newTriangleIndex    ] = newVertexIndex;
                    newTriangles[newTriangleIndex + 1] = newVertexIndex + resolution + 1;
                    newTriangles[newTriangleIndex + 2] = newVertexIndex + resolution;

                    // 1 2
                    // - 3
                    newTriangles[newTriangleIndex + 3] = newVertexIndex;
                    newTriangles[newTriangleIndex + 4] = newVertexIndex + 1;
                    newTriangles[newTriangleIndex + 5] = newVertexIndex + resolution + 1;

                    newTriangleIndex += 6;
                }

                ++newVertexIndex;
                ++oldVertexIndex;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices = newVertices;
        mesh.triangles = triangles = newTriangles;
        mesh.normals = normals = newNormals;
    }
}