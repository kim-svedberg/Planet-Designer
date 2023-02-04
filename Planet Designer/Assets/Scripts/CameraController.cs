using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    private Camera camera;
    private Planet planet;

    [Range(1f, 12f)]
    [SerializeField] private float distance;

    private void Awake()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(Vector3.zero);

        if (planet == null && (planet = GameObject.Find("Planet").GetComponent<Planet>()) == null)
            return;

        transform.position = transform.position.normalized * planet.settings.radius * distance;
    }

}
