using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class SurfaceModifier
{
    public bool enabled = true;

    public abstract void Run(Sphere sphere);

}
