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
    [SerializeField, ReadOnly] private string planetName;
    [SerializeField, HideInInspector] private Sphere terrainSphere;
    [SerializeField, HideInInspector] private Sphere oceanSphere;
    [SerializeField, HideInInspector] private Transform featuresParent;
    [SerializeField, HideInInspector] private Transform objectsParent;
    [SerializeField, HideInInspector] private bool initialized;

    public static UnityEvent<Planet> RegenerationCompleted = new UnityEvent<Planet>();
    public static UnityEvent<Planet> Loaded = new UnityEvent<Planet>();

    public string PlanetName { get { return planetName; } set { planetName = value; } }
    public Sphere TerrainSphere => terrainSphere;
    public Sphere OceanSphere => oceanSphere;
    public Transform FeaturesParent => featuresParent;
    public Transform ObjectsParent => objectsParent;

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
        featuresParent = transform.Find("Features");
        objectsParent = transform.Find("Objects");

        terrainSphere.Initialize();
        oceanSphere.Initialize();
    }

    [ContextMenu("Regenerate")]
    public void Regenerate()
    {
        if (!initialized)
            return;

        Stopwatch stopwatch = Stopwatch.StartNew();

        terrainSphere.Regenerate();
        oceanSphere.Regenerate();

        stopwatch.Stop();
        Debug.Log("Regenerated " + gameObject.name + " (" + stopwatch.ElapsedMilliseconds + "ms)");
        RegenerationCompleted.Invoke(this);
    }

}