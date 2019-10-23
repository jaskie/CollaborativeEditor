using Newtonsoft.Json;

namespace TVP.CollaborativeEditor.Common.Interfaces
{
    public interface IDocumentClient
    {
        [JsonProperty]
        string Name { get; set; }
    }
}