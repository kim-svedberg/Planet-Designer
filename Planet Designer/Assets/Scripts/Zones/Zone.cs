using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Zone : Selectable
{
    [SerializeField] private ZoneSettings settings;
    [SerializeField] private Feature feature;

    public ZoneSettings Settings => settings;
    public Feature Feature => feature;

    public void Initialize(ZoneSettings settings, Feature feature)
    {
        this.settings = settings;
        this.feature = feature;
    }

    public override void WhileSelected()
    {
        if (!CameraController.BeingControlled() && Reticle.Instance.OnPlanetSurface && Input.GetMouseButton(0))
        {
            int oldPointCount = settings.points.Count;
            Vector3 mouseSurfaceDirection = Reticle.Instance.transform.position.normalized;

            // Remove points
            if (Input.GetKey(KeyCode.LeftShift))
            {
                for (int i = settings.points.Count - 1; i >= 0; --i)
                {
                    // Remove point if its center is within the brush
                    if (Vector3.Angle(settings.points[i], mouseSurfaceDirection) < Reticle.Instance.BrushAngle)
                    {
                        settings.points.RemoveAt(i);
                    }
                }
            }

            // Add points
            else
            {
                int pointAddAttempts = Mathf.Max(400, (int)(Reticle.Instance.BrushAngle * 20));

                for (int i = 0; i < pointAddAttempts; ++i)
                {
                    // Get direction vector with random offset within the brush angle
                    Vector3 newPoint = DirectionOffsetter.Offset(Reticle.Instance.transform.position, Reticle.Instance.BrushAngle, true);

                    // Make sure point is not too close to any other point
                    if (Clear())
                    {
                        // Add point
                        settings.points.Add(newPoint);
                    }

                    bool Clear()
                    {
                        foreach (Vector3 zonePoint in settings.points)
                            if (Vector3.Angle(newPoint, zonePoint) < settings.pointAngle)
                                return false;
                        return true;
                    }
                }
            }

            // Update feature if zone changed
            if (oldPointCount != settings.points.Count)
            {
                feature.Regenerate();
            }
        }
    }

    public override void OnSelected()
    {
        Debug.Log(gameObject.name + " Selected");
        #if UNITY_EDITOR
        Selection.activeObject = feature;
        #endif
    }

    public override void OnDeselected()
    {
        Debug.Log(gameObject.name + " Deselected");
    }

    public override void Delete()
    {
        Debug.Log(gameObject.name + " Deleted");
        Planet.Instance.DeleteFeature(feature);
    }

    private void OnDrawGizmos()
    {
        float radius = Planet.Instance.TerrainSphere.Settings.radius;
        float pointRadius = Vector2.Distance(new Vector2(0, radius), new Vector2(0, radius).RotateAroundZero(settings.pointAngle * Mathf.Deg2Rad));

        Gizmos.color = new Color(1f, 1f, 1f, 0.5f);

        foreach (Vector3 zonePoint in settings.points)
            Gizmos.DrawSphere(zonePoint * radius, pointRadius);
    }


}
