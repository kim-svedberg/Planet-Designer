using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Updater : MonoBehaviour
{
    private Planet planet;
    private Transform[] children;

    private void Awake()
    {
        planet = GameObject.Find("Planet").GetComponent<Planet>();
    }

    private void Update()
    {
        if (transform.childCount != children.Length)
        {
            UpdateReferences();
            planet.Regenerate();
        }

        else
        {
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i].GetSiblingIndex() != i)
                {
                    UpdateReferences();
                    planet.Regenerate();
                    break;
                }
            }
        }
    }

    private void UpdateReferences()
    {
        children = new Transform[transform.childCount];

        for (int i = 0; i < children.Length; ++i)
            children[i] = transform.GetChild(i);
    }
}
