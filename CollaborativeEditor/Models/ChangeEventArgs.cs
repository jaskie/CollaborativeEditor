using System;

namespace TVP.CollaborativeEditor.Models
{
    public enum ChangeType
    {
        Add,
        Remove
    }

    public class ChangeEventArgs: EventArgs
    {
        public ChangeEventArgs(DocumentClient documentClient, ChangeType changeType, int offset, string changedText)
        {
            DocumentClient = documentClient;
            Offset = offset;
            ChangeType = changeType;
            ChangedText = changedText;
        }

        public DocumentClient DocumentClient { get; }

        public ChangeType ChangeType { get; }

        public int Offset { get; }
        
        public string ChangedText { get; } 
    }
}