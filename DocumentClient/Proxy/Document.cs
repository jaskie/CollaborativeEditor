using ComponentModelRPC.Client;
using System;
using ComponentModelRPC;
using TVP.CollaborativeEditor.Common.Interfaces;
using TVP.CollaborativeEditor.Common;

namespace TVP.DocumentClient.Proxy
{
    public class Document : ProxyBase, IDocument
    {
        public bool AddText(IDocumentClient client, int offset, string changedText)
        {
            return Query<bool>(parameters: new object[] { client, offset, changedText });
        }

        public IDocumentClient ClientJoin(string clientName)
        {
            return Query<DocumentClient>(parameters: new object[] { clientName });
        }

        public void ClientQuit(IDocumentClient client)
        {
            Invoke(parameters: new object[] { client });
        }

        public bool DeleteText(IDocumentClient client, int offset, string changedText)
        {
            return Query<bool>(parameters: new object[] { client, offset, changedText });
        }

        public IDocumentClient[] GetClients()
        {
            return Query<DocumentClient[]>();
        }

        public string GetText()
        {
            return Query<string>();
        }

        private event EventHandler<ClientEventArgs> _clientJoined;
        private event EventHandler<ClientEventArgs> _clientQuited;
        private event EventHandler<ChangeEventArgs> _textChanged;

        public event EventHandler<ClientEventArgs> ClientJoined
        {
            add
            {
                EventAdd(_clientJoined);
                _clientJoined += value;
            }
            remove
            {
                _clientJoined -= value;
                EventRemove(_clientJoined);
            }
        }
        public event EventHandler<ClientEventArgs> ClientQuited
        {
            add
            {
                EventAdd(_clientQuited);
                _clientQuited += value;
            }
            remove
            {
                _clientQuited -= value;
                EventRemove(_clientQuited);
            }

        }
        
        public event EventHandler<ChangeEventArgs> TextChanged
        {
            add
            {
                EventAdd(_textChanged);
                _textChanged += value;
            }
            remove
            {
                _textChanged -= value;
                EventRemove(_textChanged);
            }

        }

        protected override void OnEventNotification(SocketMessage message)
        {
            switch (message.MemberName)
            {
                case nameof(IDocument.ClientJoined):
                    _clientJoined?.Invoke(this, Deserialize<ClientEventArgs>(message));
                    break;
                case nameof(IDocument.ClientQuited):
                    _clientQuited?.Invoke(this, Deserialize<ClientEventArgs>(message));
                    break;
                case nameof(IDocument.TextChanged):
                    _textChanged?.Invoke(this, Deserialize<ChangeEventArgs>(message));
                    break;
            }
        }
    }
}
