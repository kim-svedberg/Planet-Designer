using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Range
{
    public float min;
    public float max;

    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Returns whether or not the provided value lies within this range
    /// </summary>
    public bool Contains(float value)
    {
        return value >= min && value <= max;
    }

    /// <summary>
    /// Expands this range to the provided value if it lies outside the current range
    /// </summary>
    public void Expand(float value)
    {
        if (value < min)
            min = value;

        if (value > max)
            max = value;
    }

    /// <summary>
    /// Sets both min and max values to zero
    /// </summary>
    public void Clear()
    {
        min = max = 0f;
    }

    /// <summary>
    /// Sets the min and max value of this range
    /// </summary>
    public void Set(float min, float max)
    {
        this.min = min;
        this.max = max;
    }

    /// <summary>
    /// Sets the min and max value of this range
    /// </summary>
    public void Set(float value)
    {
        min = max = value;
    }
}
