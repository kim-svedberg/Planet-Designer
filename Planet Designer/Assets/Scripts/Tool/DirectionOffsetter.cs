using UnityEngine;

public static class DirectionOffsetter
{
    private static Quaternion quaternion;

    /// <summary>
    /// Offsets the provided direction vector within the provided angle
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static Vector3 Offset(Vector3 direction, float maxOffsetDegrees, bool biasEvenDistribution = false)
    {
        // Angle quaternion in the provided direction
        quaternion.SetLookRotation(direction);

        // The angle the quaternion will be skewed in
        float zAxisOffset = Random.Range(0f, 360f);

        // How much to skew the quaternion
        float xAxisOffset = Random.Range(0f, 1f);

        // Bias the xAxisOffset to be greater on average
        // (while still within the 0 to 1 range)
        if (biasEvenDistribution)
            xAxisOffset = -Mathf.Pow(xAxisOffset - 1f, 2f) + 1f;

        // Multiply to correct range
        xAxisOffset *= maxOffsetDegrees;

        // Skew quaternion (first Z then X)
        quaternion *= Quaternion.Euler(0f, 0f, zAxisOffset);
        quaternion *= Quaternion.Euler(xAxisOffset, 0f, 0f);

        // Get new point from quaternion
        Vector3 newDirection = quaternion * Vector3.forward;

        return newDirection;
    }
}
