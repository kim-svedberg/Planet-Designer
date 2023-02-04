using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Planet : MonoBehaviour
{
    [SerializeField] public PlanetSettings settings;
    [SerializeField, HideInInspector] private MeshFilter[] meshFilters;

    [SerializeField, HideInInspector] private TerrainFace[] terrainFaces;
    [SerializeField, HideInInspector] private Material material;

    [SerializeField, HideInInspector] private Transform meshesParent;
    [SerializeField, HideInInspector] private Transform surfaceModifiersParent;

    public static UnityEvent<Planet> RegenerationCompleted = new UnityEvent<Planet>();

    private void OnValidate()
    {
        Regenerate();
    }

    public void Regenerate()
    {
        Initialize();
        GenerateMesh();

        foreach (Transform child in surfaceModifiersParent)
            child.GetComponent<SurfaceModifier>().Run(this);

        RegenerationCompleted.Invoke(this);
    }

    private void Initialize()
    {
        if (settings == null)
        {
            settings = new PlanetSettings();
        }

        if (meshesParent == null)
        {
            meshesParent = transform.Find("Meshes");
            surfaceModifiersParent = transform.Find("Surface Modifiers");
            meshFilters = new MeshFilter[6];
            material = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        }

        if (meshFilters == null || meshFilters.Length == 0)
        {
           
        }

        terrainFaces = new TerrainFace[6];
        material.color = settings.color;

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; ++i)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("Mesh " + i);
                meshObj.transform.parent = meshesParent;
                meshObj.AddComponent<MeshRenderer>().sharedMaterial = material;

                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            //meshFilters[i].GetComponent<MeshRenderer>().material.color = settings.color;
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, settings.resolution, directions[i]);
        }
    }

    private void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh(settings);
        }
    }

}