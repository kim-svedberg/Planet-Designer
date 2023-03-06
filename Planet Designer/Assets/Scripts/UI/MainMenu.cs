using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private TextMeshProUGUI newPlanetName;
    [SerializeField] private GridLayoutGroup cardGrid;
    [SerializeField] private GameObject planetCardPrefab;
    [SerializeField] private GameObject newPlanetPrompt;
    [SerializeField] private GameObject editorMenu;
    [SerializeField] private GameObject backButton;

    private PlanetCard selectedPlanetCard;

    private void OnEnable()
    {
        RefreshGrid();
        backButton.SetActive(GameObject.Find("Planet"));
    }

    public void RefreshGrid()
    {
        selectedPlanetCard = null;

        foreach (Transform planetCard in cardGrid.transform)
            Destroy(planetCard.gameObject);

        foreach (string planetName in resourceManager.GetPlanetNames())
        {
            PlanetCard planetCard = Instantiate(planetCardPrefab, cardGrid.transform).GetComponent<PlanetCard>();
            planetCard.SetName(planetName);
            planetCard.SetSprite(resourceManager.GetPlanetSprite(planetName));
        }

        Debug.Log("Grid refreshed");
    }

    public void SelectPlanetCard(PlanetCard planetCard)
    {
        selectedPlanetCard = planetCard;
    }

    public void DeselectPlanetCard()
    {
        selectedPlanetCard = null;
    }

    public void CreatePlanet()
    {
        if (newPlanetName.text == "")
            throw new System.Exception("No planet name entered");

        Destroy(GameObject.Find("Planet"));
        resourceManager.CreatePlanet(newPlanetName.text);
        newPlanetPrompt.SetActive(false);
        gameObject.SetActive(false);
        editorMenu.SetActive(true);
    }

    public void DeleteSelectedPlanet()
    {
        if (selectedPlanetCard == null)
            throw new System.Exception("No planet selected");

        resourceManager.DeletePlanet(selectedPlanetCard.name);
        RefreshGrid();
    }

    public void OpenSelectedPlanet()
    {
        if (selectedPlanetCard == null)
            throw new System.Exception("No planet selected");

        resourceManager.LoadPlanet(selectedPlanetCard.name);
    }

}
