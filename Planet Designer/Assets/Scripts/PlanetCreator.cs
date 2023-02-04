using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour
{
    [ContextMenu("Create planet")]
    private void CreatePlanet()
    {
        GameObject planet = new GameObject("Planet");
        GameObject meshes = new GameObject("Meshes");
        GameObject surfaceModifiers = new GameObject("Surface Modifiers");

        meshes.transform.parent = planet.transform;
        surfaceModifiers.transform.parent = planet.transform;

        planet.AddComponent<Planet>();
    }
}
