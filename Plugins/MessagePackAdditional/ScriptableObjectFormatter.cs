using MessagePack;
using MessagePack.Formatters;
using UnityEngine;

namespace System.Collections.Generic
{
    public class ScriptableObjectFormatter<T> : IMessagePackFormatter<T> where T : ScriptableObjectUniq<T>
    {
        public void Serialize(ref MessagePackWriter writer, T value, MessagePackSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNil();
                return;
            }
            
            writer.Write(value.Path);
        }

        public T Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.TryReadNil())
            {
                return null;
            }
            
            options.Security.DepthStep(ref reader);

            var asset = ScriptableObjectUniq<T>.GetByPath(reader.ReadString());

            reader.Depth--;
            return asset;
        }
    }
}