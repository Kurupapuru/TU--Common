namespace TU.Unity
{
    public static class UnityEngineExtensions
    {
        public static bool IsNull(this UnityEngine.Object source) => ReferenceEquals(source, null);
    }
}