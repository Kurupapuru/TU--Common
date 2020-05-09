using System;
using System.Collections.Generic;
using UnityEngine;

public static class GradientTextureCreator
{
    private static List<Color> GradientColors;
    private static List<Color> TextureColors;
    
    public static Texture2D Create(Gradient gradient, int mainAxisSize = 256, int sideAxisSize = 1, Direction direction = Direction.LeftToRight)
    {
        int width, height;
        switch (direction)
        {
            case Direction.TopToBottom:
            case Direction.BottomToTop:
                width = sideAxisSize;
                height = mainAxisSize;
                break;
            case Direction.RightToLeft:
            case Direction.LeftToRight:
                width = mainAxisSize;
                height = sideAxisSize;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }

        
        if (GradientColors == null)
        {
            GradientColors = new List<Color>(mainAxisSize);
            TextureColors = new List<Color>(mainAxisSize * sideAxisSize);
        }
        else
        {
            GradientColors.Clear();
            TextureColors.Clear();
            
            if (GradientColors.Capacity < mainAxisSize)
                GradientColors.Capacity = mainAxisSize;

            var neededTextureColorsCapacity = mainAxisSize * sideAxisSize;
            if (TextureColors.Capacity < neededTextureColorsCapacity)
                TextureColors.Capacity = neededTextureColorsCapacity;
        }
        
        Texture2D result = new Texture2D(width, height);
        
        // Reading Left To Right
        for (int i = 0; i < mainAxisSize; i++)
        {
            var time = (float)i / mainAxisSize;
            GradientColors.Add(gradient.Evaluate( time));
        }

        switch (direction)
        {
            case Direction.TopToBottom:
                for (int gradientIndex = mainAxisSize - 1; gradientIndex >= 0; gradientIndex--)
                for (int sideAxis = 0; sideAxis < sideAxisSize; sideAxis++)
                    TextureColors.Add(GradientColors[gradientIndex]);
                break;
            case Direction.BottomToTop:
                for (int gradientIndex = 0; gradientIndex < mainAxisSize; gradientIndex++)
                for (int sideAxis = 0; sideAxis < sideAxisSize; sideAxis++)
                    TextureColors.Add(GradientColors[gradientIndex]);
                break;
            case Direction.RightToLeft:
                for (int sideAxis = 0; sideAxis < sideAxisSize; sideAxis++)
                for (int gradientIndex = mainAxisSize - 1; gradientIndex >= 0; gradientIndex--)
                    TextureColors.Add(GradientColors[gradientIndex]);
                break;
            case Direction.LeftToRight:
                for (int sideAxis = 0; sideAxis < sideAxisSize; sideAxis++)
                for (int gradientIndex = 0; gradientIndex < mainAxisSize; gradientIndex++)
                    TextureColors.Add(GradientColors[gradientIndex]);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
        
        result.SetPixels(TextureColors.ToArray());

        return result;
    }
    
    [Serializable]
    public enum Direction
    {
        TopToBottom,
        BottomToTop,
        RightToLeft,
        LeftToRight
    }
}
