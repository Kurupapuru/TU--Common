using UniRx;
using UnityEngine;

namespace TU.Unity.CameraRelated
{
    public class MainCamera : MonoBehaviour
    {
        public new static ReactiveProperty<Camera> camera = new ReactiveProperty<Camera>();

        public Camera mainCamera;
    
        private void Awake()
        {
            if (mainCamera == null)
                mainCamera = GetComponent<Camera>();

            if (mainCamera == null)
                mainCamera = Camera.main;
        
            camera.Value = mainCamera;
        }
    }
}