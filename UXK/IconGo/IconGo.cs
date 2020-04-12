using System;
using TU.Unity.CameraRelated;
using UnityEngine;

namespace UXK.IconGoCreator
{
    public class IconGo : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Transform _scaleTransform;

        private Transform _cameraT;
        
        public Sprite Sprite
        {
            get => _renderer.sprite;
        }

        public IconGo Setup(Sprite sprite, float scale)
        {
            _renderer.sprite = sprite;
            var longestLength = sprite.rect.height > sprite.rect.width ? sprite.rect.height : sprite.rect.width;
            var scaleForOne   = 1 / (longestLength / sprite.pixelsPerUnit);
            var neededScale = scaleForOne * scale;
            _scaleTransform.localScale = Vector3.one * neededScale;
            return this;
        }

        private void Start()
        {
            _cameraT = MainCamera.camera.Value.transform;
        }

        private void Update()
        {
            // Inverted rotation, because this is how SpriteRenderer works
            _renderer.transform.rotation = Quaternion.LookRotation(_cameraT.position - transform.position);
        }

#if UNITY_EDITOR // Test
        [ContextMenu("Test with current sprite and scale 1")]
        private void Test() => Setup(_renderer.sprite, 1);
#endif
    }
}