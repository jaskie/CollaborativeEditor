using ComponentModelRPC.Server;
using TVP.CollaborativeEditor.Common.Interfaces;

namespace TVP.CollaborativeEditor.Models
{
    public class DocumentClient : DtoBase, IDocumentClient
    {
        private string _name;

        public string Name { get => _name; set => SetField(ref _name, value); }


    }
}
