using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private MainMenu mainMenu;

    public MainMenu MainMenu => mainMenu;

    private void Start()
    {
        startMenu.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

}
