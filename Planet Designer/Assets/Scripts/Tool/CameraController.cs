using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Light light;

    [Range(1.1f, 16f)]
    [SerializeField] private float distance;

    [Range(0.01f, 2f)]
    [SerializeField] private float scrollSpeed;

    private Vector3 previousPosition;

    public static bool BeingControlled()
    {
        return Input.GetKey(KeyCode.Space);
    }

    private void Awake()
    {
        Planet.Loaded.AddListener(UpdateMagnitude);
        Sphere.RegenerationCompleted.AddListener((sphere) => { UpdateMagnitude(); });

        UpdateLight();
    }

    private void OnValidate()
    {
        UpdateMagnitude();
        UpdateLight();
    }

    private void UpdateMagnitude()
    {
        if (!Planet.Instance)
            return;

        transform.position = transform.position.normalized * Planet.Instance.TerrainSphere.Settings.radius * distance;
    }

    private void UpdateLight()
    {
        light.intensity = Mathf.Clamp(distance.Remapped(2.5f, 5f, 0.9f, 0f), 0f, 0.9f);
    }

    private void Update()
    {
        if (!Planet.Instance)
            return;

        if (!BeingControlled())
            return;

        // If scrolling
        if (Input.mouseScrollDelta != Vector2.zero)
        {
            distance -= Input.mouseScrollDelta.y * distance * scrollSpeed;
            distance = Mathf.Clamp(distance, 1.1f, 16f);

            UpdateMagnitude();
            UpdateLight();
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
            transform.Translate(new Vector3(0f, 0f, Planet.Instance.TerrainSphere.Settings.radius * -distance));
        }
    }

}
