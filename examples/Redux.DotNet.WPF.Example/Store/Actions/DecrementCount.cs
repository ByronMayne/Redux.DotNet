namespace ReduxSharp.Wpf.Store.Actions
{
    internal record DecrementCount(int Amount) : IAction
    {
        public string Type => "Count/Decrement";
    }
}
