using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class GeographicTransform : MonoBehaviour
{
    [SerializeField] private GeographicCoordinates coordinates;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private bool continuousUpdate;

    public GeographicCoordinates Coordinates => coordinates;

    public float longitude { get { return coordinates.longitude; } set { coordinates.longitude = value; } }
    public float latitude  { get { return coordinates.latitude;  } set { coordinates.latitude = value;  } }
    public float magnitude { get { return coordinates.magnitude; } set { coordinates.magnitude = value; } }

    private void OnValidate()
    {
        coordinates.Normalize();
        UpdateTransform();
    }

    [ExecuteAlways]
    private void Update()
    {
        if (continuousUpdate)
            UpdateTransform();
    }

    public void UpdateTransform()
    {
        transform.localPosition = GeographicCoordinates.ToPosition(coordinates);
        transform.up = transform.localPosition.normalized;
        //transform.rotation.Set(transform.rotation.x, 0f, transform.rotation.z, 0f);
        //transform.Rotate(rotation);
    }
}
