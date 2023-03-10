using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorMenu : MonoBehaviour
{
    public void SelectTerrainSphere()
    {
        #if UNITY_EDITOR
        Selection.activeObject = GameObject.Find("Planet").GetComponent<Planet>().TerrainSphere.Settings;
        #endif
    }

    public void SelectTerrainMaterial()
    {
        #if UNITY_EDITOR
        Selection.activeObject = GameObject.Find("Planet").GetComponent<Planet>().TerrainSphere.Settings.material;
        #endif
    }

    public void SelectOceanSphere()
    {
        #if UNITY_EDITOR
        Selection.activeObject = GameObject.Find("Planet").GetComponent<Planet>().OceanSphere.Settings;
        #endif
    }

    public void SelectOceanMaterial()
    {
        #if UNITY_EDITOR
        Selection.activeObject = GameObject.Find("Planet").GetComponent<Planet>().OceanSphere.Settings.material;
        #endif
    }

}
