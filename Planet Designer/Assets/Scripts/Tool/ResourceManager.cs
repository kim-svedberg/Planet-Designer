using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private GameObject planetPrefab;

    /// <summary>
    /// Creats a new planet folder by duplicating the default preset
    /// </summary>
    public void CreatePlanet(string planetName)
    {
        #if UNITY_EDITOR
        AssetDatabase.CopyAsset("Assets/Resources/Presets/Default", "Assets/Resources/Planets/" + planetName);
        LoadPlanet(planetName);
        #endif
    }

    /// <summary>
    /// Returns the names of all saved planets
    /// </summary>
    public string[] GetPlanetNames()
    {
        string[] planetNames = Directory.GetDirectories(Application.dataPath + "/Resources/Planets");

        for (int i = 0; i < planetNames.Length; ++i)
        {
            planetNames[i] = planetNames[i].Substring(planetNames[i].LastIndexOf('/') + 1);
        }

        return planetNames;
    }

    /// <summary>
    /// Returns a sprite created using a stored Texture2D
    /// </summary>
    public Sprite GetPlanetSprite(string planetName)
    {
        Texture2D texture = Resources.Load<Texture2D>("Planets/" + planetName + "/Sprite");

        if (texture == null)
            return null;
        else
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }

    /// <summary>
    /// Replaces a planet sprite
    /// </summary>
    public void UpdatePlanetSprite(string planetName, Texture2D texture)
    {
        #if UNITY_EDITOR
        string spritePath = "Assets/Resources/Planets/" + planetName + "/Sprite.asset";
        AssetDatabase.DeleteAsset(spritePath);
        AssetDatabase.CreateAsset(texture, spritePath);
        #endif
    }

    /// <summary>
    /// Instantiates a planet prefab, initializes it, and generates the planet
    /// </summary>
    public void LoadPlanet(string planetName)
    {
        string planetFolder = "Planets/" + planetName + "/";

        // Instantiate and initialize planet
        Planet planet = Instantiate(planetPrefab).GetComponent<Planet>();
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

        // Load features
        #if UNITY_EDITOR
        string[] features = AssetDatabase.GetSubFolders("Assets/Resources/Planets/" + planetName + "/Features");

        foreach (string feature in features)
        {
            string featureName = feature.Substring(feature.LastIndexOf('/') + 1);

            if (featureName.Contains("Forest"))
            {
                ForestSettings forestSettings = Resources.Load<ForestSettings>(feature.Replace("Assets/Resources/", "") + "/Forest Settings");
                ZoneSettings zoneSettings = Resources.Load<ZoneSettings>(feature.Replace("Assets/Resources/", "") + "/Zone Settings");
                planet.AddForest(featureName, forestSettings, zoneSettings);
            }
        }
        #endif

        // Generate planet
        planet.Regenerate();
        Planet.Loaded.Invoke();

        #if UNITY_EDITOR
        Selection.activeObject = planet.TerrainSphere.gameObject;
        EditorGUIUtility.PingObject(planet.TerrainSphere.gameObject);
        #endif
    }

    /// <summary>
    /// Deletes a planet folder
    /// </summary>
    public void DeletePlanet(string planetName)
    {
        #if UNITY_EDITOR
        AssetDatabase.DeleteAsset("Assets/Resources/Planets/" + planetName);
        #endif
    }

    /// <summary>
    /// Renames a planet folder
    /// </summary>
    public void RenamePlanet(string planetName, string newPlanetName)
    {
        #if UNITY_EDITOR
        AssetDatabase.RenameAsset("Assets/Resources/Planets/" + planetName, newPlanetName);
        #endif
    }

    /// <summary>
    /// Duplicates a planet folder and returns the duplicate's name
    /// </summary>
    public string DuplicatePlanet(string planetName)
    {
        // CopyAsset does not work if duplicate already exists !!!

        #if UNITY_EDITOR
        AssetDatabase.CopyAsset("Assets/Resources/Planets/" + planetName, "Assets/Resources/Planets/" + planetName + " (copy)");
        #endif
        return planetName + " (copy)";
    }

    /// <summary>
    /// Creates a folder with data for a forest and returns the feature's name
    /// </summary>
    public string CreateForest(string planetName, out ForestSettings forestSettings, out ZoneSettings zoneSettings)
    {
        zoneSettings = ScriptableObject.CreateInstance<ZoneSettings>();
        forestSettings = ScriptableObject.CreateInstance<ForestSettings>();

        #if UNITY_EDITOR
        string folderGUID = AssetDatabase.CreateFolder("Assets/Resources/Planets/" + planetName + "/Features", "Forest");
        string folderPath = AssetDatabase.GUIDToAssetPath(folderGUID);

        AssetDatabase.CreateAsset(zoneSettings, folderPath + "/Zone Settings.asset");
        AssetDatabase.CreateAsset(forestSettings, folderPath + "/Forest Settings.asset");

        return folderPath.Substring(folderPath.LastIndexOf('/') + 1);
        #endif
        return null;
    }

    /// <summary>
    /// Deletes a feature of a planet
    /// </summary>
    public void DeleteFeature(string planetName, string featureName)
    {
        #if UNITY_EDITOR
        AssetDatabase.DeleteAsset("Assets/Resources/Planets/" + planetName + "/Features/" + featureName);
        #endif
    }

}
