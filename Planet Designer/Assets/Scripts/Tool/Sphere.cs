using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[ExecuteAlways]
public class Sphere : MonoBehaviour
{
    public enum SphereType { Ocean, Terrain }

    [SerializeField] private SphereSettings settings;
    [SerializeField] private SphereFace[] sphereFaces;
    [SerializeField, HideInInspector] private SphereInfo info;
    [SerializeField, HideInInspector] private bool initialized;
    [SerializeField, HideInInspector] private Range elevationRange;
    [SerializeField, HideInInspector] private SphereType sphereType;
    [SerializeField, HideInInspector] private Planet planet;

    public SphereSettings Settings => settings;
    public SphereFace[] SphereFaces => sphereFaces;
    public Range ElevationRange => elevationRange;

    private void OnValidate()
    {
        if (!initialized)
            return;

        if (sphereFaces == null)
            sphereFaces = GetComponentsInChildren<SphereFace>();

        Stopwatch stopwatch = Stopwatch.StartNew();
        Regenerate();
        stopwatch.Stop();
        Debug.Log("Regenerated " + gameObject.name + " (" + stopwatch.ElapsedMilliseconds + "ms)");
    }

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
        if (!initialized)
            return;

        ReconstructData();
        UpdateMesh();
        UpdateShaders();
        info.UpdateInfo(this);
    }

    public void Initialize()
    {
        if (initialized)
            return;

        planet = GetComponentInParent<Planet>();
        sphereType = gameObject.name.Contains("Ocean") ? SphereType.Ocean : SphereType.Terrain;
        initialized = true;
        sphereFaces = new SphereFace[6];
        info = GetComponent<SphereInfo>();
        elevationRange = new Range();

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < sphereFaces.Length; ++i)
        {
            sphereFaces[i] = new GameObject("Sphere Face").AddComponent<SphereFace>();
            sphereFaces[i].Initialize(transform, directions[i]);
        }
    }

    public void ReconstructData()
    {
        if (settings.fixEdgeNormals)
            foreach (SphereFace sphereFace in sphereFaces)
                sphereFace.ReconstructData_SeamlessNormals(settings);
        else
            foreach (SphereFace sphereFace in sphereFaces)
                sphereFace.ReconstructData_NoNormalFix(settings);

        SurfaceModifier surfaceModifier;

        foreach (Transform child in transform)
            if (child.gameObject.activeSelf && (surfaceModifier = child.GetComponent<SurfaceModifier>()))
                surfaceModifier.Run(this);
    }

    public void UpdateMesh()
    {
        elevationRange.Set(sphereFaces[0].Vertices[0].magnitude);

        foreach (SphereFace sphereFace in sphereFaces)
        {
            sphereFace.UpdateMesh(settings);

            foreach (Vector3 vertex in sphereFace.Vertices)
                elevationRange.Expand(vertex.magnitude);
        }
    }

    public void UpdateShaders()
    {
        // If this is ocean sphere: Update the terrain shader's ocean level
        if (sphereType == SphereType.Ocean)
        {
            planet.TerrainSphere.settings.material.SetFloat("_Ocean_Elevation", settings.radius);
        }
           
        // If this is terrain sphere: Update the terrain shader's elevation range
        else
        {
            settings.material.SetFloat("_Min_Elevation", elevationRange.min);
            settings.material.SetFloat("_Max_Elevation", elevationRange.max);
        }   
    }

    [ContextMenu("Toggle Mesh In Hierarchy")]
    private void ToggleMeshInHierarchy()
    {
        bool hide = sphereFaces[0].MeshFilter.gameObject.hideFlags == HideFlags.None;

        foreach (SphereFace sphereFace in sphereFaces)
            sphereFace.MeshFilter.gameObject.hideFlags = hide ? HideFlags.HideInHierarchy : HideFlags.None;
    }

}
