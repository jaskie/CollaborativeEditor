using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVP.CollaborativeEditor.Models;

namespace TVP.CollaborativeEditor.Models
{
    public class Document
    {
        private readonly List<DocumentClient> _clients = new List<DocumentClient>();
        
        private readonly StringBuilder _text = new StringBuilder();

        public string Text => _text.ToString();

        public IEnumerable<DocumentClient> Clients => _clients;

        public bool AddText(DocumentClient client, int offset, string changedText)
        {
            _text.Insert(offset, changedText);
            TextChanged?.Invoke(this, new ChangeEventArgs(client, ChangeType.Add, offset, changedText));
            return true;
        }

        public bool DeleteText(DocumentClient client, int offset, string changedText)
        {
            if (offset > _text.Length - 1)
                return false;
            _text.Remove(offset, changedText.Length);
            TextChanged?.Invoke(this, new ChangeEventArgs(client, ChangeType.Remove, offset, changedText));
            return true;
        }

        public void ClientJoin(DocumentClient client)
        {
            if (_clients.Contains(client))
                return;
            _clients.Add(client);
            ClientJoined?.Invoke(this, new ClientEventArgs(client));

        }

        public void ClientQuit(DocumentClient client)
        {
            if (!_clients.Remove(client))
                return;
            ClientQuited?.Invoke(this, new ClientEventArgs(client));
        }

        public DocumentClient[] GetClients() => _clients.ToArray();

        public event EventHandler<ClientEventArgs> ClientJoined;

        public event EventHandler<ClientEventArgs> ClientQuited;

        public event EventHandler<ChangeEventArgs> TextChanged;

    }
}
