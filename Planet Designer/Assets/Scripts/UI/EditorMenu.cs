using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class EditorMenu : MonoBehaviour
{
    public static EditorMenu Instance { get; private set; }

    [SerializeField] private ResourceManager resourceManager;
    [SerializeField] private TextMeshProUGUI helpText;
    [SerializeField] private TextMeshProUGUI cameraControlText;

    private void Awake()
    {
        Instance = this;
    }

    public void SetHelpText(string text)
    {
        helpText.text = text;
    }

    public void ShowCameraControlText(bool visible)
    {
        cameraControlText.gameObject.SetActive(visible);
    }

    // Button actions

    public void SelectTerrainSphere()
    {
        #if UNITY_EDITOR
        Selection.activeObject = Planet.Instance.TerrainSphere.Settings;
        #endif
    }

    public void SelectTerrainMaterial()
    {
        #if UNITY_EDITOR
        Selection.activeObject = Planet.Instance.TerrainSphere.Settings.material;
        #endif
    }

    public void SelectOceanSphere()
    {
        #if UNITY_EDITOR
        Selection.activeObject = Planet.Instance.OceanSphere.Settings;
        #endif
    }

    public void SelectOceanMaterial()
    {
        #if UNITY_EDITOR
        Selection.activeObject = Planet.Instance.OceanSphere.Settings.material;
        #endif
    }

    public void NewForest()
    {
        string forestName = resourceManager.CreateForest(Planet.Instance.PlanetName, out ForestSettings forestSettings, out ZoneSettings zoneSettings);
        Forest forest = Planet.Instance.AddForest(forestName, forestSettings, zoneSettings);
        forest.GetComponent<Zone>().Select();

        FeatureOverview.Instance.Refresh();
    }

    public void RandomizePlanet()
    {
        Planet.Instance.RandomizeSeeds();
        Planet.Instance.Regenerate();
    }

}
