using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class PlanetInfo : MonoBehaviour
{
    [Header("Mesh")]
    [SerializeField][ReadOnly] public int vertices;
    [SerializeField][ReadOnly] public int triangles;

    [Header("Sphere")]
    [SerializeField][ReadOnly] public float surfaceArea;
    [SerializeField][ReadOnly] public float rectDensity;

    private void Awake()
    {
        Planet.RegenerationCompleted.AddListener(UpdateInfo);
    }

    public void UpdateInfo(Planet planet)
    {
        PlanetSettings settings = planet.settings;
        Mesh mesh = transform.GetComponentInChildren<MeshFilter>().mesh;

        // Mesh
        vertices = mesh.vertexCount * 6;
        triangles = mesh.triangles.Length * 6;

        // Sphere
        surfaceArea = 4f * Mathf.PI * settings.radius * settings.radius;
        rectDensity = triangles * 0.5f / surfaceArea;

    }
}
