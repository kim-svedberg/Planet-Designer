using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField] private GeographicTransform geographicTransform;
    private Vector3 originalScale;

    public GeographicTransform GeographicTransform => geographicTransform;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        transform.localScale = originalScale * distance;
    }

}
