using TU.Unity.ComponentUtils;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace TU.Unity.World
{
    public class ObstacleFromSky : MonoBehaviour
    {
        [SerializeField] private RXColliderCallbacksSender warningAppearColliderCallbacksSender;
        [FormerlySerializedAs("obstacleAppearColliderCallbacksSender")] [SerializeField] private RXColliderCallbacksSender appearColliderCallbacksSender;
        [SerializeField] private LayerMask canTriggerLayerMask;

        [SerializeField] private Animator warningAnimator;
        [SerializeField] private Animator obstacleAnimator;
        [SerializeField] private Rigidbody obstacleRigidbody;


        private CompositeDisposable disposables = new CompositeDisposable();


        private void OnEnable()
        {
            if (warningAppearColliderCallbacksSender != null)
            {
                warningAppearColliderCallbacksSender.useLayerMaskCheck = true;
                warningAppearColliderCallbacksSender.layerMask = canTriggerLayerMask;
                
                warningAppearColliderCallbacksSender.Initialize();
                
                warningAppearColliderCallbacksSender.UnitedOnColliderEnterCallback
                    .Subscribe(WarningAppearColliderEntered)
                    .AddTo(disposables);
            }  else
                Debug.LogWarning("WarningAppearColliderCallbacksSender didn't set", this);

            if (appearColliderCallbacksSender != null)
            {
                appearColliderCallbacksSender.useLayerMaskCheck = true;
                appearColliderCallbacksSender.layerMask = canTriggerLayerMask;
                
                appearColliderCallbacksSender.Initialize();
                
                appearColliderCallbacksSender.UnitedOnColliderEnterCallback
                    .Subscribe(AppearColliderEntered)
                    .AddTo(disposables);
            }
            else
                Debug.LogError("ObstacleAppearColliderCallbacksSender didn't set", this);
        }

        private void OnDisable() => disposables?.Dispose();


        private void AppearColliderEntered(Collider obj)
        {
            appearColliderCallbacksSender.GetComponent<Collider>().enabled = false;
            StartAppear();
        }

        private void WarningAppearColliderEntered(Collider obj)
        {
            warningAppearColliderCallbacksSender.GetComponent<Collider>().enabled = false;
            Warning();
        }

        public void StartAppear()
        {
            if (obstacleAnimator != null)
                obstacleAnimator.SetTrigger(1809480452); // Appear
            if (obstacleRigidbody != null)
                obstacleRigidbody.isKinematic = false;
            
            if (warningAnimator != null)
                warningAnimator.SetTrigger(-1427642350); // Disappear

#if DEVELOPMENT_BUILD || UNITY_EDITOR
            if (obstacleAnimator == null && obstacleRigidbody == null)
                Debug.LogError("Obstacle animator and obstacleRigidbody didn't set, please set one or another", this);
#endif
        }
        
        public void Warning()
        {
            if (warningAnimator != null)
                warningAnimator.SetTrigger(1809480452); // Appear
            else
                Debug.LogWarning("Warning animator didn't set", this);
        }
    }
}