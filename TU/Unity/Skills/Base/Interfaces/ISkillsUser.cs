using UnityEngine;

namespace MGame.Code.Skills.Base.Interfaces
{
    public interface ISkillsUser
    {
        LayerMask invisiblePlaneLayerMask { get; }
        LayerMask attackMask { get; }
        Transform UserTransform { get; }
    }
}