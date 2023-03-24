using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : Feature
{
    [SerializeField] private Zone zone;
    [SerializeField] private ForestSettings settings;
    [SerializeField] private LayerMask raycastLayerMask;

    private Noise noise;

    public void Initialize(ForestSettings forestSettings, ZoneSettings zoneSettings)
    {
        forestSettings.SetForest(this);
        settings = forestSettings;
        zone.Initialize(zoneSettings, this);

        Sphere.RegenerationCompleted.AddListener((sphere) => { Regenerate(); });
    }

    public override void Regenerate()
    {
        // Reset

        noise = new Noise(settings.seed.value);

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Get selected prefabs

        List<GameObject> prefabs = new List<GameObject>();

        foreach (SelectablePrefab selectablePrefab in settings.prefabs)
        {
            if (selectablePrefab.selected && selectablePrefab.prefab)
                prefabs.Add(selectablePrefab.prefab);
        }

        if (prefabs.Count == 0)
            return;

        // Place prefabs in zone

        int terrainLayer = LayerMask.NameToLayer("Terrain");
        RaycastHit raycastHit;
        GameObject go;
        float raycastDistance = Planet.Instance.TerrainSphere.ElevationRange.max + 1;

        foreach (Vector3 point in zone.Settings.points)
        {
            // Sample noise to determine whether or not to place object on this point
            float sample1 = noise.Evaluate(point * settings.seedScale).Remapped(-1f, 1f, 0f, 100f);

            if (sample1 > settings.density)
                continue;

            Physics.Raycast(point * raycastDistance, -point, out raycastHit, raycastDistance, raycastLayerMask);

            if (raycastHit.collider.gameObject.layer != terrainLayer)
                continue;

            // Sample noise to determine which object to instantiate
            float sample2 = noise.Evaluate(point * settings.seedScale * 2f).Remapped(-1f, 1f, 0f, prefabs.Count);

            // Sample noise to determine the object's rotation
            float sample3 = noise.Evaluate(point * settings.seedScale * 3f).Remapped(-1f, 1f, 0f, 360f);

            // Instantiate object
            go = Instantiate(prefabs[(int)sample2], transform);
            go.transform.position = raycastHit.point;
            go.transform.up = point;
            go.transform.Rotate(0f, sample3, 0f);
        }
    }
}
