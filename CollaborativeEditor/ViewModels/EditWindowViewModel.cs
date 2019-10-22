using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TVP.CollaborativeEditor.Helpers;
using TVP.CollaborativeEditor.Models;
using TVP.CollaborativeEditor.Views;

namespace TVP.CollaborativeEditor.ViewModels
{
    internal class EditWindowViewModel : WindowViewModelBase<EditWindow>
    {
        private string _userName;
        private int _line;
        private int _column;
        private readonly Document _document;
        private readonly DocumentClient _documentClient;
        private readonly ObservableCollection<DocumentClient> _clients;
        private bool _isUpdateNotifying;

        public EditWindowViewModel()
        {
            _documentClient = new DocumentClient();
            _document = AppDataContext.Instance.Document;
            _clients = new ObservableCollection<DocumentClient>(_document.Clients);
            _document.ClientJoined += Document_ClientJoined;
            _document.ClientQuited += Document_ClientQuited;
            _document.TextChanged += Document_TextChanged;
            _document.ClientJoin(_documentClient);
            EditDocument = new TextDocument(_document.Text);
            EditDocument.Changed += Text_Changed;
        }

        public UiCommand CommandNewWindow { get; } = new UiCommand(NewWindow);

        private static void NewWindow(object obj)
        {
            new EditWindowViewModel().Show();
        }

        public string UserName
        {
            get => _userName; 
            set
            {
                if (!Set(ref _userName, value))
                    return;
                _documentClient.Name = value;
            }
        }

        public IEnumerable<DocumentClient> Clients => _clients;

        public TextDocument EditDocument { get; }

        public int Line { get => _line; set => Set(ref _line, value); }

        public int Column { get => _column; set => Set(ref _column, value); }

        private void Text_Changed(object sender, DocumentChangeEventArgs e)
        {
            if (_isUpdateNotifying)
                return;
            if (e.RemovalLength > 0)
                _document.DeleteText(_documentClient, e.Offset, e.RemovedText.Text);
            if (e.InsertionLength > 0)
                _document.AddText(_documentClient, e.Offset, e.InsertedText.Text);
        }

        private void Document_ClientJoined(object sender, ClientEventArgs e)
        {
            _clients.Add(e.Client);
        }

        private void Document_ClientQuited(object sender, ClientEventArgs e)
        {
            _clients.Remove(e.Client);
        }

        private void Document_TextChanged(object sender, ChangeEventArgs e)
        {
            if (e.DocumentClient == _documentClient)
                return;
            _isUpdateNotifying = true;
            try
            {
                if (e.ChangeType == ChangeType.Add)
                    EditDocument.Insert(e.Offset, e.ChangedText);
                else
                    EditDocument.Remove(e.Offset, e.ChangedText.Length);
            }
            finally
            {
                _isUpdateNotifying = false;
            }
        }


        protected override void ViewClosing(object sender, CancelEventArgs e)
        {
            _document.ClientJoined += Document_ClientJoined;
            _document.ClientQuited += Document_ClientQuited;
            _document.ClientQuit(_documentClient);
        }
    }
}
