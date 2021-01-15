using ReduxSharp;

namespace Redux.DotNet.Wpf.Store.Actions
{
    internal record ChangeAgeAction(int NewAge) : IAction
    {
        public string Type => "User/ChangeAge";
    }
}
