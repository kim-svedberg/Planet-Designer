using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SurfaceModifier : MonoBehaviour
{
    public abstract void Run(Sphere sphere);

    private void OnValidate()
    {
        if (transform.parent)
            transform.parent.GetComponent<Sphere>().Regenerate();
    }
}
