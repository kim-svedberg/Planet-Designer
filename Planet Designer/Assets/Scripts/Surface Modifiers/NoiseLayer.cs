using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseLayer : SurfaceModifier
{
    private Noise noise;

    [Min(0f)]
    [SerializeField] private float amplitude = 1f;

    [Min(0f)]
    [SerializeField] private float scale = 1f;

    [SerializeField] private AnimationCurve manipulation;

    [SerializeField] private Seed seed;

    public override void Run(Sphere sphere)
    {
        noise = new Noise(seed.value);

        foreach (SphereFace sphereFace in sphere.SphereFaces)
        {
            for (int i = 0; i < sphereFace.Vertices.Length; ++i)
            {
                ModifyVertex(ref sphereFace.Vertices[i]);
                
                void ModifyVertex(ref Vector3 vertex)
                {
                    vertex +=
                        vertex.normalized
                        * Manipulate(noise.Evaluate((vertex.normalized + Vector3.one * seed.value) * scale))
                        * amplitude
                        * amplitude;
                }
            }
        }
    }

    private float Manipulate(float value)
    {
        return manipulation.Evaluate((value + 1f) * 0.5f) * 2f - 1f;
    }
}
