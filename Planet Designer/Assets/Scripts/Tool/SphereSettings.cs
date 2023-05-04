using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet Designer/Sphere Settings")]
public class SphereSettings : ScriptableObject
{
    private Sphere sphere;

    public bool autoRegenerate = true;

    [HideInInspector]
    public bool fixEdgeNormals = true;

    [Range(3, 256)]
    public int resolution = 150;

    //[Range(-0.2f, 0.0f)]
    [Range(-1f, 1f), HideInInspector]
    public float vertexDistributionBias = -0.06f;

    [Range(50, 500f)]
    public float radius = 150;

    public Material material;

    public List<NoiseLayer> noiseLayers = new List<NoiseLayer>();

    public void SetSphere(Sphere sphere)
    {
        this.sphere = sphere;
    }

    private void OnValidate()
    {
        if (sphere && autoRegenerate)
            sphere.Regenerate();
    }

}
