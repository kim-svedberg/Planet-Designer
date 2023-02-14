using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Updater : MonoBehaviour
{
    private Transform[] children;
    private bool[] active;

    private void Start()
    {
        UpdateReferences();
    }

    private void Update()
    {
        if (transform.childCount != children.Length)
        {
            UpdateReferences();
            GetComponent<Sphere>().Regenerate();
        }

        else
        {
            for (int i = 0; i < children.Length; ++i)
            {
                if (children[i].GetSiblingIndex() != i)
                {
                    UpdateReferences();
                    GetComponent<Sphere>().Regenerate();
                    break;
                }

                else if (children[i].gameObject.activeSelf != active[i])
                {
                    UpdateReferences();
                    GetComponent<Sphere>().Regenerate();
                    break;
                }
            }
        }
    }

    private void UpdateReferences()
    {
        children = new Transform[transform.childCount];
        active = new bool[transform.childCount];

        for (int i = 0; i < children.Length; ++i)
        {
            children[i] = transform.GetChild(i);
            active[i] = children[i].gameObject.activeSelf;
        }
            
    }
}
