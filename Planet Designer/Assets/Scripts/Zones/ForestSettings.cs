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

    public List<SelectablePrefab> prefabs = new List<SelectablePrefab>();

    public void SetForest(Forest forest)
    {
        this.forest = forest;
    }

    private void OnValidate()
    {
        if (forest)
            forest.Regenerate();
    }
}
