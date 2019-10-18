namespace BehavioursBasedCamera
{
    using UnityEngine;

    public interface IHasTransformTargetForCenter
    {
        Transform transformTargetForCenter { get; set; }
    }

    public interface IHasCameraTargetForCamera
    {
        Camera cameraTargetForCamera { get; set; }
    }

    public interface IHasTransformTargetForCamera
    {
        Transform transformTargetForCamera { get; set; }
    }
}