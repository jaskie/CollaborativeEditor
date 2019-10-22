using System.Windows;
using System.Windows.Controls;

namespace TVP.CollaborativeEditor.ViewModels
{

    public abstract class WindowViewModelBase<TView> : ViewModelBase where TView : Window, new()
    {
        private TView _view;

        public TView View
        {
            get => _view;
            private set
            {
                var oldView = _view;
                if (Equals(_view, value))
                    return;
                _view = value;
                if (oldView != null)
                    oldView.Closing -= ViewClosing;
                if (value != null)
                    _view.Closing += ViewClosing;
            }
        }

        protected abstract void ViewClosing(object sender, System.ComponentModel.CancelEventArgs e);

        public virtual void Close()
        {
            View.Close();
            View = null;
        }

        public void Show()
        {
            View = new TView() { DataContext = this };
            View.Show();
        }

    }
}