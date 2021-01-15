using Redux.DotNet.Wpf;
using Redux.DotNet.Wpf.ViewModel;
using ReduxSharp;
using System.Windows;

namespace Redux.DotNet.WPF.Example.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public IStore<AppState> Store
        {
            get { return (IStore<AppState>)GetValue(StoreProperty); }
            set { SetValue(StoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Store.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoreProperty =
            DependencyProperty.Register(nameof(Store), typeof(IStore<AppState>), typeof(MainView), new PropertyMetadata(null));


        public MainView(IStore<AppState> store)
        {
            DataContext = new MainViewModel();
            Store = store;
            InitializeComponent();
        }
    }
}
