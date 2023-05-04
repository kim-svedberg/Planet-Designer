using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FeatureOverview : MonoBehaviour
{
    public static FeatureOverview Instance { get; private set; }

    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Notification notification;

    private void Awake()
    {
        Instance = this;
        Planet.Loaded.AddListener(Refresh);
        gameObject.SetActive(false);
    }

    private void Start()
    {
        Refresh();
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
            Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in transform)
        {
            if (child.name != "Placeholder")
                Destroy(child.gameObject);
        }

        transform.Find("Placeholder").gameObject.SetActive(!Planet.Instance || Planet.Instance.Features.Count == 0);

        foreach (Feature feature in Planet.Instance.Features)
        {
            Instantiate(itemPrefab, transform).GetComponent<FeatureItem>().Initialize(feature);
        }

        notification.SetCount(Planet.Instance.Features.Count);
    }
}
