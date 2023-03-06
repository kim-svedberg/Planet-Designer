using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet Designer/Sphere Settings")]
public class SphereSettings : ScriptableObject
{
    public bool autoRegenerate = true;
    public bool fixEdgeNormals = true;

    [Range(3, 256)]
    public int resolution = 150;

    [Range(-0.2f, 0.0f)]
    public float vertexDistributionBias = -0.06f;

    [Range(50, 500f)]
    public float radius = 150;

    public Material material;

    public List<NoiseLayer> noiseLayers = new List<NoiseLayer>();

}
