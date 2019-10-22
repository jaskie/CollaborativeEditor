using System;

namespace TVP.CollaborativeEditor.Models
{
    public class ClientEventArgs: EventArgs
    {
        public ClientEventArgs(DocumentClient client)
        {
            Client = client;
        }

        public DocumentClient Client { get; }
    }
}