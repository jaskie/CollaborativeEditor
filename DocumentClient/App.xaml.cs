using System.Windows;
using TVP.DocumentClient.ViewModels;

namespace TVP.DocumentClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            new EditWindowViewModel().Show();
        }
    }
}
