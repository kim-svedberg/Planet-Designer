using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;

[ExecuteAlways]
public class Planet : MonoBehaviour
{
    [SerializeField, HideInInspector] private Sphere terrainSphere;
    [SerializeField, HideInInspector] private Sphere oceanSphere;

    public static UnityEvent<Planet> RegenerationCompleted = new UnityEvent<Planet>();

    private bool initialized;

    public Sphere TerrainSphere => terrainSphere;
    public Sphere OceanSphere => oceanSphere;

    private void OnValidate()
    {
        if (!initialized)
            return;

        Regenerate();
    }

    public void Initialize()
    {
        initialized = true;

        terrainSphere = transform.Find("Terrain Sphere").GetComponent<Sphere>();
        oceanSphere = transform.Find("Ocean Sphere").GetComponent<Sphere>();

        terrainSphere.Initialize();
        oceanSphere.Initialize();
    }

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
        if (!initialized)
            return;

        Stopwatch stopwatch = Stopwatch.StartNew();

        terrainSphere.ReconstructData();
        oceanSphere.ReconstructData();

        /*foreach (Transform child in surfaceModifiersParent)
            if (child.gameObject.activeSelf)
                child.GetComponent<SurfaceModifier>().Run(this);*/

        terrainSphere.UpdateMesh();
        oceanSphere.UpdateMesh();

        RegenerationCompleted.Invoke(this);

        stopwatch.Stop();
        Debug.Log("Regenerated (" + stopwatch.ElapsedMilliseconds + "ms)");
    }

}