using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    [Range(1f, 12f)]
    [SerializeField] private float distance;

    [Range(-90f, 90f)]
    [SerializeField] private float pitch;

    [Range(-20f, 20f)]
    [SerializeField] private float speed;

    private float yaw;
    private SphereSettings sphereSettings;

    [ExecuteAlways]
    private void Update()
    {
        if (sphereSettings == null)
        {
            if (GameObject.Find("Planet"))
                sphereSettings = GameObject.Find("Planet").GetComponent<Planet>().TerrainSphere.Settings;
            else
                return;
        }

        // If running
        if (Application.isPlaying)
        {
            yaw += speed * Time.deltaTime;
            yaw %= 360f;
            transform.rotation = Quaternion.Euler(pitch, -yaw, 0f);

            transform.position = -transform.forward * sphereSettings.radius * distance;
        }

        // If not running
        else
        {
            transform.position = transform.position.normalized * sphereSettings.radius * distance;
            transform.LookAt(Vector3.zero);
        }
    }

}
