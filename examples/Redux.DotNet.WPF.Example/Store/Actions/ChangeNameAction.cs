using ReduxSharp;

namespace Redux.DotNet.Wpf.Store.Actions
{
    internal record ChangeNameAction(string FirstName, string LastName) : IAction
    {
        public string Type => "User/ChangeName";
    }
}
