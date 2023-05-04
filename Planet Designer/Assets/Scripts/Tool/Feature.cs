using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feature : MonoBehaviour
{
    public abstract void Regenerate();

    public abstract Object InspectObject();
}
