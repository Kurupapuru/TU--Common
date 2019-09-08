using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace KurupapuruLab
{
    public class BinarySerializer
    {
        public static byte[] BinaryFormatterSerialize(object customObject)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                formatter.Serialize(ms, customObject);
                return ms.ToArray();
            }
        }

        public static object BinaryFormatterDeserialize(byte[] data)
        {
            var formatter = new BinaryFormatter();
            using (var ms = new MemoryStream(data))
            {
                return formatter.Deserialize(ms);
            }
        }
    }
}