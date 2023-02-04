using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetSettings
{
    [Range(3, 256)]
    public int resolution = 20;

    [Range(-0.2f, 0.0f)]
    public float vertexDistributionBias;

    [Range(50, 500f)]
    public float radius = 50;

    public Color color = Color.green;
}
