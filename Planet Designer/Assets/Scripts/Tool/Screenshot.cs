using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEditor;
using Debug = UnityEngine.Debug;

[ExecuteAlways]
public class Screenshot : MonoBehaviour
{
    public enum ImageFormat { JPG, PNG }

    [SerializeField, ReadOnly, TextArea] private string shortcut = "Left CTRL/CMD + S to screenshot";
    [SerializeField, ReadOnly, TextArea] private string folderPath;
    [SerializeField] private ImageFormat imageFormat;

    private static Screenshot instance;

    private void OnEnable()
    {
        instance = this;
        folderPath = Application.dataPath;
        folderPath = folderPath.Remove(folderPath.LastIndexOf("Assets"));
        folderPath += "Screenshots/";
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Capture();
            }
        }
    }

    #if UNITY_EDITOR
    [MenuItem("Planet Designer/Screenshot %#1")]
    #endif
    public static void Capture()
    {
        string fileName =
            instance.folderPath +
            DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss.") +
            instance.imageFormat.ToString().ToLower();

        Debug.Log("Screenshot taken: " + fileName);
        ScreenCapture.CaptureScreenshot(fileName);
    }

}
