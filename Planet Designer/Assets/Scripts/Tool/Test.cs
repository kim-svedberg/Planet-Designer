using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Test : MonoBehaviour
{
    [SerializeField] private GeographicCoordinates coordinates;

    [ExecuteAlways]
    void Update()
    {
        coordinates = GeographicCoordinates.FromPosition(transform.position);
        Debug.Log(GeographicCoordinates.ToPosition(coordinates).ToString());
    }
}
