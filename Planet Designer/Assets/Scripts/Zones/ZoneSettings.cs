using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Planet Designer/Zone Settings")]
public class ZoneSettings : ScriptableObject
{
    [Range(0f, 5f)]
    public float pointAngle = 1;
    public List<Vector3> points = new List<Vector3>();
}
