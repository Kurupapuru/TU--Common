/*

using BehavioursBasedCamera;

namespace Shared.Code.CharacterController
{
    using Photon.Pun;
    using UnityEngine;


    public class OneDirectionCharacterController_Photon : MonoBehaviourPun
    {
        [Header("References")] public Animator animator;

        [Header("Settings")] public float speedMultiplier = 1;


        private Camera _camera;

        private void Start()
        {
            _camera = BehavioursBasedCameraController.instance.camera;
        }

        private void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
                return;


            animator.SetBool(-843014753, animator.IsInTransition(0)); // "IsInTransition"

            var input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // Speed
            var speed = Mathf.Abs(input.magnitude);
            speed = Mathf.Clamp(speed, 0, 1);
            animator.SetFloat(-823668238, speed); // "Speed"


            // Rotation
            if (speed != 0)
            {
                var input3D = new Vector3(input.x, 0, input.y);

                var rotatedInput3D = //transform.rotation * 
                    Quaternion.AngleAxis(_camera.transform.eulerAngles.y, Vector3.up) *
                    input3D;

                transform.rotation = Quaternion.LookRotation(rotatedInput3D);
            }
        }
    }
}

*/