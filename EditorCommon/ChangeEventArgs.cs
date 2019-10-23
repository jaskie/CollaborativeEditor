using System;
using TVP.CollaborativeEditor.Common.Interfaces;

namespace TVP.CollaborativeEditor.Common
{
    public enum ChangeType
    {
        Add,
        Remove
    }

    public class ChangeEventArgs: EventArgs
    {
        public ChangeEventArgs(IDocumentClient documentClient, ChangeType changeType, int offset, string changedText)
        {
            DocumentClient = documentClient;
            Offset = offset;
            ChangeType = changeType;
            ChangedText = changedText;
        }

        public IDocumentClient DocumentClient { get; }

        public ChangeType ChangeType { get; }

        public int Offset { get; }
        
        public string ChangedText { get; } 
    }
}