namespace ReduxSharp.Wpf.Store.Actions
{
    internal record IncrementCount(int Amount) : IAction
    {
        public string Type => "Count/Increment";
    }
}
