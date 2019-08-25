namespace ZR.Runtime.Utils
{
    public interface IAutoHashID
    {
        string Name { get; set; }
        int ID { get; }
        bool HasValue { get; }
        void UpdateID();
    }
}