using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;

    public void CreatePlanet(string planetName)
    {
        #if UNITY_EDITOR
        AssetDatabase.CopyAsset("Assets/Resources/Presets/Default", "Assets/Resources/Planets/" + planetName);
        LoadPlanet(planetName);
        #endif
    }

    public string[] GetPlanetNames()
    {
        string[] planetNames = Directory.GetDirectories(Application.dataPath + "/Resources/Planets");

        for (int i = 0; i < planetNames.Length; ++i)
        {
            planetNames[i] = planetNames[i].Substring(planetNames[i].LastIndexOf('/') + 1);
        }

        return planetNames;
    }

    public Sprite GetPlanetSprite(string planetName)
    {
        Texture2D texture = Resources.Load<Texture2D>("Planets/" + planetName + "/Sprite");

        if (texture == null)
            return null;
        else
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    public void UpdatePlanetSprite(string planetName, Texture2D texture)
    {
#if UNITY_EDITOR
        string spritePath = "Assets/Resources/Planets/" + planetName + "/Sprite.asset";
        AssetDatabase.DeleteAsset(spritePath);
        AssetDatabase.CreateAsset(texture, spritePath);
#endif
    }

    public void LoadPlanet(string planetName)
    {
        string planetFolder = "Planets/" + planetName + "/";

        // Instantiate and initialize planet
        Planet planet = Instantiate(planetPrefab).AddComponent<Planet>();
        planet.gameObject.name = "Planet";
        planet.PlanetName = planetName;
        planet.Initialize();

        // Link settings and material resources
        planet.OceanSphere.Settings = Resources.Load<SphereSettings>(planetFolder + "Ocean Settings");
        planet.TerrainSphere.Settings = Resources.Load<SphereSettings>(planetFolder + "Terrain Settings");

        planet.OceanSphere.Settings.material = Resources.Load<Material>(planetFolder + "Ocean Material");
        planet.TerrainSphere.Settings.material = Resources.Load<Material>(planetFolder + "Terrain Material");

        planet.OceanSphere.Settings.SetSphere(planet.OceanSphere);
        planet.TerrainSphere.Settings.SetSphere(planet.TerrainSphere);

        // Generate planet
        planet.Regenerate();
        Planet.Loaded.Invoke(planet);

#if UNITY_EDITOR
        Selection.activeObject = planet.TerrainSphere.gameObject;
        EditorGUIUtility.PingObject(planet.TerrainSphere.gameObject);
#endif
    }

    public void DeletePlanet(string planetName)
    {
#if UNITY_EDITOR
        AssetDatabase.DeleteAsset("Assets/Resources/Planets/" + planetName);
#endif
    }

    

}
