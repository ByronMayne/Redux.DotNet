using System.Windows;
using System.Windows.Controls;

namespace ReduxSharp.WPF.Controls
{
    /// <summary>
    /// Interaction logic for StoreProvider.xaml
    /// </summary>
    public partial class StoreProvider : UserControl
    {
        public static readonly DependencyProperty StoreProperty;
        static StoreProvider()
        {
            StoreProperty = DependencyProperty.Register(nameof(Store), typeof(IStore),
                typeof(StoreProvider), new PropertyMetadata(null));
        }

        /// <summary>
        /// Gets or sets the store instance that this will provide
        /// </summary>
        public IStore Store
        {
            get => (IStore)GetValue(StoreProperty);
            set => SetValue(StoreProperty, value);
        }

        public StoreProvider() : base()
        {
            InitializeComponent();
        }
    }
}
