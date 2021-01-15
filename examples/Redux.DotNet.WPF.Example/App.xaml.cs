using Redux.DotNet.Wpf;
using Redux.DotNet.WPF.Example.Views;
using ReduxSharp;
using System.Windows;

namespace Redux.DotNet.WPF.Example
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IStore<AppState> Store { get; private set; }

        public App()
        {

        }

        public App(IStore<AppState> store)
        {
            Store = store;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainView mainView = new MainView(Store);

            mainView.Show();
        }
    }
}
