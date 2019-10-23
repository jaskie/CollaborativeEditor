using ComponentModelRPC;
using ComponentModelRPC.Client;
using Newtonsoft.Json;
using TVP.CollaborativeEditor.Common.Interfaces;

namespace TVP.DocumentClient.Proxy
{
    public class DocumentClient : ProxyBase, IDocumentClient
    {
#pragma warning disable CS0649

        [JsonProperty(nameof(Name))]
        private string _name;

#pragma warning restore

        [JsonIgnore]
        public string Name { get => _name; set => Set(value); }

        protected override void OnEventNotification(SocketMessage message)
        {

        }
    }
}
