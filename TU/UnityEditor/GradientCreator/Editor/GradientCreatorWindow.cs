using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class GradientCreatorWindow : EditorWindow
{
    private const string GRADIENTS_FOLDER_KEY = "GradientsFolder";
    
    [MenuItem("Tools/Gradient Texture Creator")]
    private static void OpenWindow()
    {
        var window = GetWindow<GradientCreatorWindow>();
        window.Show();
        
        window.saveTo = AssetDatabase.LoadAssetAtPath<DefaultAsset>(
            PlayerPrefs.GetString(key: GRADIENTS_FOLDER_KEY, defaultValue: "Assets/Textures/Gradients"));
    }

    public Gradient gradient = new Gradient();
    public int mainAxisSize = 256;
    public int sideAxisSize = 1;
    public GradientTextureCreator.Direction direction = GradientTextureCreator.Direction.LeftToRight;
    public DefaultAsset saveTo = null;
    public string fileName = "Gradient";

    public void GenerateImage()
    {
        var texture = GradientTextureCreator.Create(gradient, (int)mainAxisSize, (int)sideAxisSize, direction);
        var bytes = texture.EncodeToPNG();
        var projectPath = Application.dataPath;
        projectPath = projectPath.Remove(projectPath.Length-7, 7);
        File.WriteAllBytes($"{projectPath}/{AssetDatabase.GetAssetPath(saveTo)}/{fileName}.png", bytes);
        AssetDatabase.Refresh();
    }

    private void SaveToChanged()
    {
        PlayerPrefs.SetString(GRADIENTS_FOLDER_KEY, AssetDatabase.GetAssetPath(saveTo));
        PlayerPrefs.Save();
    }

    private void OnGUI()
    {
        gradient = EditorGUILayout.GradientField(gradient);
        
        mainAxisSize = EditorGUILayout.IntField(mainAxisSize);
        sideAxisSize = EditorGUILayout.IntField(sideAxisSize);

        direction = (GradientTextureCreator.Direction)EditorGUILayout.EnumPopup(direction);
        var newSaveTo = (DefaultAsset)EditorGUILayout.ObjectField("Save To: ", saveTo, typeof(DefaultAsset), false);
        if (newSaveTo != saveTo)
        {
            saveTo = newSaveTo;
            SaveToChanged();
        }
        fileName = EditorGUILayout.TextField(fileName);

        if (GUILayout.Button("Generate Image"))
            GenerateImage();
    }
}
