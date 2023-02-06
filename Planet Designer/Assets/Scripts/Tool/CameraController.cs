using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    private Camera camera;
    private SphereSettings sphereSettings;

    [Range(1f, 12f)]
    [SerializeField] private float distance;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(Vector3.zero);

        if (sphereSettings == null)
        {
            if (GameObject.Find("Planet"))
                sphereSettings = GameObject.Find("Planet").GetComponent<Planet>().TerrainSphere.Settings;
            else
                return;
        }

        transform.position = transform.position.normalized * sphereSettings.radius * distance;
    }

}
