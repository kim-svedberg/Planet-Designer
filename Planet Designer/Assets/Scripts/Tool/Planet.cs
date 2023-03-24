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
    [SerializeField, ReadOnly] private bool initialized;

    [SerializeField] private Sphere oceanSphere;
    [SerializeField] private Sphere terrainSphere;
    [SerializeField] private Transform featuresParent;
    [SerializeField] private Transform objectsParent;

    [SerializeField] private GameObject forestPrefab;

    private List<Feature> features = new List<Feature>();

    public static Planet Instance { get; private set; }
    public static UnityEvent RegenerationCompleted = new UnityEvent();
    public static UnityEvent Loaded = new UnityEvent();

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
        Instance = this;
        initialized = true;

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
        RegenerateFeatures();

        stopwatch.Stop();
        Debug.Log("Regenerated " + gameObject.name + " (" + stopwatch.ElapsedMilliseconds + "ms)");
        RegenerationCompleted.Invoke();
    }

    public void RegenerateFeatures()
    {
        foreach (Feature feature in features)
            feature.Regenerate();
    }

    public Forest AddForest(string forestName, ForestSettings forestSettings, ZoneSettings zoneSettings)
    {
        Forest forest = Instantiate(forestPrefab, featuresParent).GetComponent<Forest>();
        forest.gameObject.name = forestName;
        forest.Initialize(forestSettings, zoneSettings);
        features.Add(forest);
        return forest;
    }

    public void DeleteFeature(Feature feature)
    {
        features.Remove(feature);
        Destroy(feature.gameObject);
        GameObject.Find("Managers").GetComponent<ResourceManager>().DeleteFeature(planetName, feature.name);
    }

}