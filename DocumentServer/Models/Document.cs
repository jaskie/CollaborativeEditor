using ComponentModelRPC.Server;
using System;
using System.Collections.Generic;
using System.Text;
using TVP.CollaborativeEditor.Common;
using TVP.CollaborativeEditor.Common.Interfaces;

namespace TVP.CollaborativeEditor.Models
{
    public class Document: DtoBase, IDocument
    {
        private static NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly List<IDocumentClient> _clients = new List<IDocumentClient>();
        
        private readonly StringBuilder _text = new StringBuilder();

        public string GetText() => _text.ToString();

        public bool AddText(IDocumentClient client, int offset, string changedText)
        {
            _text.Insert(offset, changedText);
            TextChanged?.Invoke(this, new ChangeEventArgs(client, ChangeType.Add, offset, changedText));
            return true;
        }

        public bool DeleteText(IDocumentClient client, int offset, string changedText)
        {
            if (offset > _text.Length - 1)
                return false;
            _text.Remove(offset, changedText.Length);
            TextChanged?.Invoke(this, new ChangeEventArgs(client, ChangeType.Remove, offset, changedText));
            return true;
        }

        public IDocumentClient ClientJoin(string clientName)
        {
            var client = new DocumentClient() { Name = clientName };
            Logger.Trace("Client joined {0}", clientName);
            _clients.Add(client);
            ClientJoined?.Invoke(this, new ClientEventArgs(client));
            return client;
        }

        public void ClientQuit(IDocumentClient client)
        {
            if (!_clients.Remove(client))
                return;
            Logger.Trace("Client quited {0}", client.Name);
            ClientQuited?.Invoke(this, new ClientEventArgs(client));
        }

        public IDocumentClient[] GetClients() => _clients.ToArray();

        public event EventHandler<ClientEventArgs> ClientJoined;

        public event EventHandler<ClientEventArgs> ClientQuited;

        public event EventHandler<ChangeEventArgs> TextChanged;

    }
}
