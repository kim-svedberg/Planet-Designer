using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlanetCreator))]
public class PlanetCreatorEditor : Editor
{
    PlanetCreator planetCreator;

    private void OnEnable()
    {
        planetCreator = (PlanetCreator)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Create Planet"))
            planetCreator.CreatePlanet();

        EditorGUILayout.Space();
        base.OnInspectorGUI();
    }
}
