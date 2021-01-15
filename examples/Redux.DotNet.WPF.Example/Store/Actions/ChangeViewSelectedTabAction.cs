using ReduxSharp;

namespace Redux.DotNet.Wpf.Store.Actions
{
    internal record ChangeViewSelectedTabAction(int Tab) : IAction
    {
        public string Type => "View/SelectedTab";
    }
}
