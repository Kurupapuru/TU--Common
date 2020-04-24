using UnityEngine;
using UnityEngine.UI;

namespace UXK.UnityUtils
{
    public static class UnityExtensions
    {
        public static void SetSprite(this RawImage rawImage, Sprite sprite)
        {
            rawImage.texture = sprite.texture;
            rawImage.uvRect = new Rect(
                sprite.rect.x      / sprite.texture.width,
                sprite.rect.y      / sprite.texture.height,
                sprite.rect.width  / sprite.texture.width,
                sprite.rect.height / sprite.texture.height);
        }
    }
}