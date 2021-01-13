using ReduxSharp.Wpf;
using ReduxSharp.Wpf.ViewModel;
using System.Windows;

namespace ReduxSharp.WPF.Example.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public IStore<ApplicationState> Store
        {
            get { return (IStore<ApplicationState>)GetValue(StoreProperty); }
            set { SetValue(StoreProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Store.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StoreProperty =
            DependencyProperty.Register(nameof(Store), typeof(IStore<ApplicationState>), typeof(MainView), new PropertyMetadata(null));


        public MainView(IStore<ApplicationState> store)
        {
            DataContext = new MainViewModel();
            Store = store;
            InitializeComponent();
        }
    }
}
