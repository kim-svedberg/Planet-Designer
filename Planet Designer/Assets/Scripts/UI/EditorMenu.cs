using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorMenu : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;

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

        #if UNITY_EDITOR
        Selection.activeObject = forestSettings;
        #endif
    }

}
