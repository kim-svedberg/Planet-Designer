using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SphereSettings
{
    public bool fixEdgeNormals;

    [Range(3, 256)]
    public int resolution;

    [Range(-0.2f, 0.0f)]
    public float vertexDistributionBias;

    [Range(50, 500f)]
    public float radius;

    public Material material;

    public Gradient gradient;
}
