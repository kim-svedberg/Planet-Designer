using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet Designer/Forest Settings")]
public class ForestSettings : ScriptableObject
{
    private Forest forest;

    public Seed seed;

    [Range(0f, 100f)]
    public float density = 50f;

    [Range(0f, 30f)]
    public float seedScale = 10f;

    /// <summary>
    /// Stop trees som being placed under water
    /// </summary>
    public bool avoidOcean = true;

    /// <summary>
    /// Stop trees from being placed on objects of the default layer
    /// </summary>
    public bool avoidObjects = true;

    public List<SelectablePrefab> prefabs = new List<SelectablePrefab>();

    public void SetForest(Forest forest)
    {
        this.forest = forest;
    }

    private void OnValidate()
    {
        if (forest)
        {
            forest.UpdateLayerMask();
            forest.Regenerate();
        }
    }
}
