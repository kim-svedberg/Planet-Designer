using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace
{
    private Mesh mesh;
    private int resolution;
    private Vector3 localUp;
    private Vector3 axisA;
    private Vector3 axisB;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        axisB = Vector3.Cross(localUp, axisA);
    }

    public void ConstructMesh(PlanetSettings planetSettings)
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        int i = 0;
        for (int y = 0; y < resolution; ++y)
        {
            for (int x = 0; x < resolution; ++x)
            {
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                percent = new Vector2(
                    CustomSmoothstep(percent.x, planetSettings.vertexDistributionBias),
                    CustomSmoothstep(percent.y, planetSettings.vertexDistributionBias));

                Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2f * axisA + (percent.y - 0.5f) * 2f * axisB;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = pointOnUnitSphere * planetSettings.radius;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }

                ++i;
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    /// <summary>
    /// See plotted function: https://www.desmos.com/calculator/ktcwf5obja
    /// </summary>
    /// <param name="x">Input value (0 to 1)</param>
    /// <param name="a">Function amplitude (-0.98 to 0.98)</param>
    /// <param name="p">Position of function separator (0 to 1)</param>
    public float CustomSmoothstep(float x, float a = 0.5f, float p = 0.5f)
    {
        if (a == 0f)
            return Mathf.Clamp01(x);

        x = Mathf.Clamp01(x);
        a = Mathf.Clamp(a, -0.98f, 0.98f);
        p = Mathf.Clamp01(p);

        float c = 2 / (1 - a) - 1; // Function amplitude

        float F(float x, float n) // Function
        {
            return Mathf.Pow(x, c) / Mathf.Pow(n, c - 1);
        }

        return x < p ? F(x, p) : 1 - F(1 - x, 1 - p); // Output
    }
}