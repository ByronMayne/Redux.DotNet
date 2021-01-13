using ReduxSharp.Wpf;
using ReduxSharp.WPF.Example.Views;
using System.Windows;

namespace ReduxSharp.WPF.Example
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IStore<ApplicationState> Store { get; private set; }

        public App()
        {

        }

        public App(IStore<ApplicationState> store)
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
