using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TVP.CollaborativeEditor.Common;
using TVP.CollaborativeEditor.Common.Interfaces;
using TVP.DocumentClient.Helpers;
using TVP.DocumentClient.Views;

namespace TVP.DocumentClient.ViewModels
{
    internal class EditWindowViewModel : WindowViewModelBase<EditWindow>
    {
        private string _userName = $"client {new Random().Next()}";
        private readonly Proxy.Document _remoteDocument;
        private readonly IDocumentClient _documentClient;
        private bool _isUpdateNotifying;

        public EditWindowViewModel()
        {
            var client = AppDataContext.Instance.RemoteClient;
            _remoteDocument = client.GetRootObject<Proxy.Document>();
            _documentClient = _remoteDocument.ClientJoin(UserName);
            Clients = new ObservableCollection<IDocumentClient>(_remoteDocument.GetClients());
            _remoteDocument.ClientJoined += Document_ClientJoined;
            _remoteDocument.ClientQuited += Document_ClientQuited;
            _remoteDocument.TextChanged += Document_TextChanged;
            EditDocument = new ICSharpCode.AvalonEdit.Document.TextDocument(_remoteDocument.GetText());
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

        public IList<IDocumentClient> Clients { get; }

        public ICSharpCode.AvalonEdit.Document.TextDocument EditDocument { get; }

        private void Text_Changed(object sender, ICSharpCode.AvalonEdit.Document.DocumentChangeEventArgs e)
        {
            if (_isUpdateNotifying)
                return;
            if (e.RemovalLength > 0)
                _remoteDocument.DeleteText(_documentClient, e.Offset, e.RemovedText.Text);
            if (e.InsertionLength > 0)
                _remoteDocument.AddText(_documentClient, e.Offset, e.InsertedText.Text);
        }

        private void Document_ClientJoined(object sender, ClientEventArgs e)
        {
            OnUiThread(() => Clients.Add(e.Client));
        }

        private void Document_ClientQuited(object sender, ClientEventArgs e)
        {
            OnUiThread(() => Clients.Remove(e.Client));
        }

        private void Document_TextChanged(object sender, ChangeEventArgs e)
        {
            if (e.DocumentClient == _documentClient)
                return;
            OnUiThread(() =>
            {
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
            });
        }


        protected override void ViewClosing(object sender, CancelEventArgs e)
        {
            _remoteDocument.ClientJoined += Document_ClientJoined;
            _remoteDocument.ClientQuited += Document_ClientQuited;
            _remoteDocument.ClientQuit(_documentClient);
        }
    }
}
