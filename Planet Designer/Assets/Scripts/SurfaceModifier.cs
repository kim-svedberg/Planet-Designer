using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public abstract class SurfaceModifier : MonoBehaviour
{
    private void Awake()
    {
        Run(GameObject.Find("Planet").GetComponent<Planet>());
    }

    public abstract void Run(Planet planet);
}
