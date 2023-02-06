using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Sphere : MonoBehaviour
{
    [SerializeField] private SphereSettings settings;
    [SerializeField, HideInInspector] private TerrainFace[] terrainFaces;
    [SerializeField, HideInInspector] private Material material;

    private SphereInfo info;
    private Transform meshesParent;
    private Transform surfaceModifiersParent;

    private bool initialized;

    public SphereSettings Settings => settings;
    public TerrainFace[] TerrainFaces => terrainFaces;

    private void OnValidate()
    {
        if (!initialized)
            return;

        Regenerate();
    }

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
        initialized = true;
        meshesParent = transform.Find("Meshes");
        surfaceModifiersParent = transform.Find("Surface Modifiers");

        terrainFaces = new TerrainFace[6];
        material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        info = GetComponent<SphereInfo>();

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < terrainFaces.Length; ++i)
            terrainFaces[i] = new TerrainFace(meshesParent, directions[i], material);
    }

    public void ReconstructData()
    {
        foreach (TerrainFace terrainFace in terrainFaces)
            terrainFace.ReconstructData(settings);

        foreach (Transform child in surfaceModifiersParent)
            if (child.gameObject.activeSelf)
                child.GetComponent<SurfaceModifier>().Run(this);
    }

    public void UpdateMesh()
    {
        material.color = settings.color;

        foreach (TerrainFace terrainFace in terrainFaces)
            terrainFace.UpdateMesh();
    }

}
