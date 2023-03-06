using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlanetCard : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Animator animator;

    private static MainMenu mainMenu;

    private void Awake()
    {
        if (mainMenu == null)
            mainMenu = GameObject.Find("Canvas").GetComponent<CanvasManager>().MainMenu;

        toggle.group = GetComponentInParent<ToggleGroup>();
    }

    public void SetName(string planetName)
    {
        gameObject.name = planetName;
        text.text = planetName;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
    
    public void OnValueChanged()
    {
        if (toggle.isOn)
        {
            mainMenu.SelectPlanetCard(this);
            animator.SetTrigger("Selected");
        }
        else
        {
            mainMenu.DeselectPlanetCard();
            animator.SetTrigger("Deselected");
        }
    }

}
