using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GeographicCoordinates
{
    [Tooltip("Longitude lines run north-south and converge at the poles. Longitude values (X) range from -180 to +180 degrees.")]
    public float longitude;

    [Tooltip("Latitude lines run east-west and are parallel to each other. Latitude values (Y) range from -90 to +90 degrees.")]
    public float latitude;

    [Tooltip("Magnitude represents the distance from the origin. Maginute values (Z) range from 0 to +infinite.")]
    public float magnitude;

    public static readonly GeographicCoordinates Zero = new GeographicCoordinates();
    public static readonly GeographicCoordinates One = new GeographicCoordinates(1f, 1f, 1f);

    /// <summary>
    /// Constructs geografic coordinates from the provided values
    /// </summary>
    public GeographicCoordinates(float longitude, float latitude, float magnitude = 1f)
    {
        this.longitude = longitude;
        this.latitude = latitude;
        this.magnitude = magnitude;
    }

    /// <summary>
    /// Constructs geografic coordinates from the provided Vector3 {x: longitude, y: latitude, z: magnitude}
    /// </summary>
    public GeographicCoordinates(Vector3 graphicCoordinates)
    {
        this.longitude = graphicCoordinates.x;
        this.latitude = graphicCoordinates.y;
        this.magnitude = graphicCoordinates.z;
    }

    /// <summary>
    /// Wraps the longitude and latitude to their intended ranges
    /// </summary>
    public GeographicCoordinates Wrap()
    {
        longitude.Wrap(-180f, 180f);
        latitude.Wrap(-90f, 90f);
        return this;
    }

    /// <summary>
    /// Constructs geographical coordinates from a position
    /// </summary>
    public static GeographicCoordinates FromPosition(Vector3 position)
    {
        GeographicCoordinates geographicCoordinates;

        geographicCoordinates.longitude = Mathf.Atan2(position.x, -position.z) * Mathf.Rad2Deg;
        geographicCoordinates.latitude  = Mathf.Asin(position.y / position.magnitude) * Mathf.Rad2Deg;
        geographicCoordinates.magnitude = position.magnitude;

        return geographicCoordinates;
    }

    /// <summary>
    /// Constructs a position from geographical coordinates
    /// </summary>
    public static Vector3 ToPosition(GeographicCoordinates coordinates)
    {
        Vector3 position;

        position.x =  coordinates.magnitude * Mathf.Cos(coordinates.latitude * Mathf.Deg2Rad) * Mathf.Sin(coordinates.longitude * Mathf.Deg2Rad);
        position.y =  coordinates.magnitude * Mathf.Sin(coordinates.latitude * Mathf.Deg2Rad);
        position.z = -coordinates.magnitude * Mathf.Cos(coordinates.latitude * Mathf.Deg2Rad) * Mathf.Cos(coordinates.longitude * Mathf.Deg2Rad);

        return position;
    }

    /// <summary>
    /// Returns the string representation of these coordinates in the format (longitude, latitude, magnitude)
    /// </summary>
    public override string ToString()
    {
        return "(" + longitude + ", " + latitude + ", " + magnitude + ")";
    }

}
