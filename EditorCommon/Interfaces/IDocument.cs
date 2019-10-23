using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TVP.CollaborativeEditor.Common.Interfaces
{
    public interface IDocument
    {
        bool AddText(IDocumentClient client, int offset, string changedText);
        bool DeleteText(IDocumentClient client, int offset, string changedText);
        IDocumentClient ClientJoin(string clientName);
        void ClientQuit(IDocumentClient client);
        IDocumentClient[] GetClients();
        string GetText();
        event EventHandler<ClientEventArgs> ClientJoined;
        event EventHandler<ClientEventArgs> ClientQuited;
        event EventHandler<ChangeEventArgs> TextChanged;
    }
}
