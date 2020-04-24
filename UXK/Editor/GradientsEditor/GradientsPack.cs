using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

[CreateAssetMenu(fileName = "GradientsPack", menuName = "Art/Gradients Pack")]
public class GradientsPack : ScriptableObject
{
    public List<Gradient> Gradients;
    public Vector2Int TextureResolution = new Vector2Int(1024, 1024);
    public Vector2Int ColumnsAndRows = new Vector2Int(32, 2);

    [Button()]
    public void GenerateTexture()
    {
        var gradientsCapacity = ColumnsAndRows.x * ColumnsAndRows.y;

        if (gradientsCapacity < Gradients.Count)
        {
            Debug.LogError($"Can't generate all gradients, capacity is {gradientsCapacity}, but gradients count is {Gradients.Count}");
        }
        
        var texture = new Texture2D(TextureResolution.x, TextureResolution.y);

        var gradientWidth = TextureResolution.x / ColumnsAndRows.x;
        var gradientHeight = TextureResolution.y / ColumnsAndRows.y;
        
        for (int i = 0; i < gradientsCapacity && i < Gradients.Count; i++)
        {
            var gradient = Gradients[i];
            var rect = GetRectFromIndex(i);
            for (int yOffset = 0; yOffset < rect.height; yOffset++)
            {
                texture.SetPixels((int) rect.x, (int) (yOffset + rect.y), gradientWidth, 1, 
                    Enumerable.Repeat(gradient.Evaluate((float)yOffset / rect.height), gradientWidth).ToArray());
            }
        }
        
        Rect GetRectFromIndex(int index)
        {
            var row    = index / ColumnsAndRows.x;
            var column = index - row * ColumnsAndRows.x;

            return new Rect(
                gradientWidth * column,
                gradientHeight - gradientHeight * row,
                gradientWidth,
                gradientHeight
            );
        }

        var selfPath = AssetDatabase.GetAssetPath(this);
        var texturePath = selfPath.Replace(".asset", "_Texture.png");
        File.WriteAllBytes(texturePath, texture.EncodeToPNG());
        AssetDatabase.Refresh();
    }

    
}
