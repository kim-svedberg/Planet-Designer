using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private TextMeshProUGUI newPlanetName;
    [SerializeField] private TextMeshProUGUI renamePlanetName;
    [SerializeField] private GridLayoutGroup cardGrid;
    [SerializeField] private ToggleGroup planetCardToggleGroup;
    [SerializeField] private GameObject planetCardPrefab;
    [SerializeField] private GameObject newPlanetPrompt;
    [SerializeField] private GameObject editorMenu;

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button newButton;
    [SerializeField] private Button openButton;
    [SerializeField] private Button renameButton;
    [SerializeField] private Button duplicateButton;
    [SerializeField] private Button deleteButton;

    private PlanetCard selectedPlanetCard;

    private void OnEnable()
    {
        RefreshGrid();
        backButton.gameObject.SetActive(GameObject.Find("Planet"));
    }

    private void Update()
    {
        if (planetCardToggleGroup.AnyTogglesOn())
        {
            if (!openButton.interactable)
                EnablePlanetCardButtons(true);
        }
        else
        {
            if (openButton.interactable)
                EnablePlanetCardButtons(false);
        }
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

    public void EnablePlanetCardButtons(bool enabled)
    {
        openButton.SetEnabled(enabled);
        renameButton.SetEnabled(enabled);
        duplicateButton.SetEnabled(enabled);
        deleteButton.SetEnabled(enabled);
    }

    // Button actions

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

    public void OpenSelectedPlanet()
    {
        resourceManager.LoadPlanet(selectedPlanetCard.name);
    }

    public void RenameSelectedPlanet()
    {
        string newName = renamePlanetName.text;
        resourceManager.RenamePlanet(selectedPlanetCard.name, newName);
        selectedPlanetCard.SetName(newName);
    }

    public void DuplicateSelectedPlanet()
    {
        string newPlanetName = resourceManager.DuplicatePlanet(selectedPlanetCard.name);
        RefreshGrid();
        cardGrid.transform.Find(newPlanetName).GetComponent<Toggle>().isOn = true;
    }

    public void DeleteSelectedPlanet()
    {
        resourceManager.DeletePlanet(selectedPlanetCard.name);
        RefreshGrid();
    }

}
