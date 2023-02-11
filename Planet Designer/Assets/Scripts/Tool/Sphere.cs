using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

[ExecuteAlways]
public class Sphere : MonoBehaviour
{
    [SerializeField] private SphereSettings settings;
    [SerializeField, HideInInspector] private TerrainFace[] terrainFaces;
    [SerializeField, HideInInspector] private SphereInfo info;
    [SerializeField, HideInInspector] private bool initialized;

    public SphereSettings Settings => settings;
    public TerrainFace[] TerrainFaces => terrainFaces;

    private void OnValidate()
    {
        if (!initialized)
            return;

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
        info.UpdateInfo(this);
    }

    public void Initialize()
    {
        if (initialized)
            return;

        initialized = true;
        terrainFaces = new TerrainFace[6];
        info = GetComponent<SphereInfo>();

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < terrainFaces.Length; ++i)
            terrainFaces[i] = new TerrainFace(transform, directions[i]);
    }

    public void ReconstructData()
    {
        if (settings.fixEdgeNormals)
            foreach (TerrainFace terrainFace in terrainFaces)
                terrainFace.ReconstructData_SeamlessNormals(settings);
        else
            foreach (TerrainFace terrainFace in terrainFaces)
                terrainFace.ReconstructData_NoNormalFix(settings);

        SurfaceModifier surfaceModifier;

        foreach (Transform child in transform)
            if (child.gameObject.activeSelf && (surfaceModifier = child.GetComponent<SurfaceModifier>()))
                surfaceModifier.Run(this);
    }

    public void UpdateMesh()
    {
        foreach (TerrainFace terrainFace in terrainFaces)
            terrainFace.UpdateMesh(settings);  
    }

    [ContextMenu("Toggle Mesh In Hierarchy")]
    private void ToggleMeshInHierarchy()
    {
        bool hide = terrainFaces[0].MeshFilter.gameObject.hideFlags == HideFlags.None;

        foreach (TerrainFace terrainFace in terrainFaces)
            terrainFace.MeshFilter.gameObject.hideFlags = hide ? HideFlags.HideInHierarchy : HideFlags.None;
    }

}
