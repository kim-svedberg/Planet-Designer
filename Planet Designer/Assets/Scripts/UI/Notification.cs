using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notification : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private int count;

    private void Awake()
    {
        UpdateAppearence();
    }

    public void SetCount(int count)
    {
        this.count = count;
        UpdateAppearence();
    }

    public void AddOne()
    {
        count++;
        UpdateAppearence();
    }

    public void SubtractOne()
    {
        count--;
        UpdateAppearence();
    }

    private void UpdateAppearence()
    {
        number.text = count.ToString();
        image.enabled = count != 0;
        number.enabled = count != 0;
    }
}
