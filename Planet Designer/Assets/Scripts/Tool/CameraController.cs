using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(1f, 16f)]
    [SerializeField] private float distance;

    [Range(0.01f, 2f)]
    [SerializeField] private float scrollSpeed;

    [SerializeField] private SphereSettings sphereSettings;
    private Vector3 previousPosition;

    private void Awake()
    {
        Planet.Loaded.AddListener(OnPlanetLoaded);
    }

    private void OnPlanetLoaded(Planet planet)
    {
        sphereSettings = planet.TerrainSphere.Settings;
        UpdateMagnitude();
    }

    private void OnValidate()
    {
        UpdateMagnitude();
    }

    private void UpdateMagnitude()
    {
        if (sphereSettings)
            transform.position = transform.position.normalized * sphereSettings.radius * distance;
    }

    private void Update()
    {
        if (sphereSettings == null)
            return;

        // If scrolling
        if (Input.mouseScrollDelta !=  Vector2.zero)
        {
            distance -= Input.mouseScrollDelta.y * distance * scrollSpeed;
            distance = Mathf.Clamp(distance, 1.1f, 16f);
            UpdateMagnitude();
        }

        // If left mouse button pressed
        if (Input.GetMouseButtonDown(0))
        {
            previousPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        // If left mouse button held down
        if (Input.GetMouseButton(0))
        {
            Vector3 viewportPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 direction = previousPosition - viewportPoint;
            previousPosition = viewportPoint;

            transform.position = Vector3.zero;
            transform.Rotate(new Vector3(1f, 0f, 0f), direction.y * 180f);
            transform.Rotate(new Vector3(0f, 1f, 0f), direction.x * -180f, Space.World);
            transform.Translate(new Vector3(0f, 0f, sphereSettings.radius * -distance));
        }
    }

}
