using UnityEngine;

namespace SubProjects.GiantHalls.Code
{
    public interface IExplosionParameters
    {
        Vector3 Position {get;}
        float Force {get;}
        float Radius { get; }
        LayerMask LayerMask { get; }
        AnimationCurve ForceOverDistance { get; }
        ForceMode ForceMode { get; }
    }
}