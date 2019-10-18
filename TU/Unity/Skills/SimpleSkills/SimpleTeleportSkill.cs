using BehavioursBasedCamera;
using KurupapuruLab.KRobots;
using MGame.Code.Skills.Base.Abstract;
using MGame.Code.Skills.Base.Interfaces;
using UnityEngine;

namespace MGame.Code.Skills.SimpleSkills
{
    [CreateAssetMenu(menuName = "MGame/Settings/Skill/Simple Teleport Skill")]
    public class SimpleTeleportSkill : AbstractSkill
    {
        private Vector3? cachedTeleportTo;

        public override bool Use(ISkillsUser user)
        {
            cachedTeleportTo = WorldScanner.GetPointFromScreenRay(BehavioursBasedCameraController.instance.camera, Input.mousePosition, user.invisiblePlaneLayerMask);
            if (cachedTeleportTo == null)
            {
                Debug.LogError($"Skill \"{name}\". Can't Find location to teleport");
                return false;
            }
            return base.Use(user);
        }

        public override void UseBehaviour(ISkillsUser user)
        {
            user.UserTransform.position = cachedTeleportTo.Value;
        }
    }
}