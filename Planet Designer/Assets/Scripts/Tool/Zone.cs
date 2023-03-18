using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ZoneType { Radial, Global }

public class Zone : Selectable
{
    [SerializeField] private ZoneType zoneType;
    [SerializeField] private GeographicTransform geographicTransform;
    [SerializeField] private float radius;

    private void OnValidate()
    {
        transform.localScale = Vector3.one * radius;
    }

    public override void WhileSelected()
    {
        if (Input.GetMouseButton(0) && !Input.GetKey(KeyCode.Space))
        {
            geographicTransform.Coordinates = SelectionManager.Instance.MouseSurfaceGeographicCoordinates;
            geographicTransform.UpdateTransform();
        }
    }

    public override void OnSelected()
    {
        Debug.Log(gameObject.name + " Selected");
    }

    public override void OnDeselected()
    {
        Debug.Log(gameObject.name + " Deselected");
    }

    public bool IsWithin()
    {
        return false;
    }

}
