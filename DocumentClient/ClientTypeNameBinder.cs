using Newtonsoft.Json.Serialization;
using System;

namespace TVP.DocumentClient
{
    public class ClientTypeNameBinder : ISerializationBinder
    {
        public Type BindToType(string assemblyName, string typeName)
        {
            switch (typeName)
            {
                case "TVP.CollaborativeEditor.Models.Document":
                    return typeof(Proxy.Document);
                case "TVP.CollaborativeEditor.Models.DocumentClient":
                    return typeof(Proxy.DocumentClient);
                default:
                    return Type.GetType($"{typeName}, {assemblyName}", true);
            }
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            typeName = serializedType.FullName;
            assemblyName = serializedType.Assembly.FullName;
        }

    }
}
