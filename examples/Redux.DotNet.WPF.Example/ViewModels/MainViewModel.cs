using ReduxSharp.Wpf.Store.Actions;

namespace ReduxSharp.Wpf.ViewModel
{

    public class MainViewModel
    {
        public DelegateCommand IncrementCommand { get; }

        public DelegateCommand DecrementCommand { get; }


        public MainViewModel()
        {

            DecrementCommand = new DelegateCommand(() => ReduxDispatch.Dispatch(new DecrementCount(1)));
            IncrementCommand = new DelegateCommand(() => ReduxDispatch.Dispatch(new IncrementCount(1)));
        }
    }
}
