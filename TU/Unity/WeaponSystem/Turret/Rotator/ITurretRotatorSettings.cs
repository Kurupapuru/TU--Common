namespace WeaponSystem.Turret
{
    public interface ITurretRotatorSettings
    {
        float verticalRotateSpeed { get; }
        float horizontalRotateSpeed { get; }
        float isRotatedAngle { get; }
        float maxRotateAngle { get; }
    }
}