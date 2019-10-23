using System;
using TVP.CollaborativeEditor.Common.Interfaces;

namespace TVP.CollaborativeEditor.Common
{
    public class ClientEventArgs: EventArgs
    {
        public ClientEventArgs(IDocumentClient client)
        {
            Client = client;
        }

        public IDocumentClient Client { get; }
    }
}