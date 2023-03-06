using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCreator : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;

    [ContextMenu("Create planet")]
    public void CreatePlanet()
    {
        GameObject planetObj = Instantiate(planetPrefab);
        planetObj.name = "Planet";

        Planet planet = planetObj.AddComponent<Planet>();
        planet.Initialize();
        planet.Regenerate();
    }
}
