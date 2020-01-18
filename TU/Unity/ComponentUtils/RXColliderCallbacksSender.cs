using System;
using TU.Unity.Extensions;
using UniRx;
using UnityEngine;

namespace TU.Unity.ComponentUtils
{
    public class RXColliderCallbacksSender : MonoBehaviour
    {
        public bool useLayerMaskCheck = false;
        public LayerMask layerMask = new LayerMask();
    
        // RX Callbacks
        public ReactiveCommand<Collision> OnCollisionEnterCallback = new ReactiveCommand<Collision>();
        public ReactiveCommand<Collision> OnCollisionStayCallback  = new ReactiveCommand<Collision>();
        public ReactiveCommand<Collision> OnCollisionExitCallback  = new ReactiveCommand<Collision>();
    
        public ReactiveCommand<Collider> OnTriggerEnterCallback = new ReactiveCommand<Collider>();
        public ReactiveCommand<Collider> OnTriggerStayCallback  = new ReactiveCommand<Collider>();
        public ReactiveCommand<Collider> OnTriggerExitCallback  = new ReactiveCommand<Collider>();

        // RX United Callbacks
        public bool isInitialized { get; private set; }
        public IObservable<Collider> UnitedOnColliderEnterCallback;
        public IObservable<Collider> UnitedOnColliderStayCallback ;
        public IObservable<Collider> UnitedOnColliderExitCallback ;
        protected virtual void Awake()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (isInitialized) return;
        
            UnitedOnColliderEnterCallback = OnTriggerEnterCallback.Merge(OnCollisionEnterCallback.Select(c => c.collider));
            UnitedOnColliderStayCallback = OnTriggerStayCallback.Merge(OnCollisionStayCallback.Select(c => c.collider));
            UnitedOnColliderExitCallback = OnTriggerExitCallback.Merge(OnCollisionExitCallback.Select(c => c.collider));
        
            isInitialized = true;
        }
    

        // Unity method callbacks
        protected void OnCollisionEnter(Collision other)
        {
            if (useLayerMaskCheck)
                if (!layerMask.LayerCheck(other.gameObject.layer))
                    return;
            OnCollisionEnterCallback.Execute(other);
        }
        protected void OnCollisionStay(Collision other) 
        {
            if (useLayerMaskCheck)
                if (!layerMask.LayerCheck(other.gameObject.layer))
                    return;
            OnCollisionStayCallback.Execute(other);
        }
        protected void OnCollisionExit(Collision other) 
        {
            if (useLayerMaskCheck)
                if (!layerMask.LayerCheck(other.gameObject.layer))
                    return;
            OnCollisionExitCallback.Execute(other);
        }

        protected void OnTriggerEnter(Collider other) 
        {
            if (useLayerMaskCheck)
                if (!layerMask.LayerCheck(other.gameObject.layer))
                    return;
            OnTriggerEnterCallback.Execute(other);
        }
        protected void OnTriggerStay(Collider other)  
        {
            if (useLayerMaskCheck)
                if (!layerMask.LayerCheck(other.gameObject.layer))
                    return;
            OnTriggerStayCallback. Execute(other);
        }
        protected void OnTriggerExit(Collider other)  
        {
            if (useLayerMaskCheck)
                if (!layerMask.LayerCheck(other.gameObject.layer))
                    return;
            OnTriggerExitCallback. Execute(other);
        }
    }
}
