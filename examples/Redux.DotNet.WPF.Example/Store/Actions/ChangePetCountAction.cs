using ReduxSharp;

namespace Redux.DotNet.WPF.Example.Store.Actions
{
    internal record ChangePetCountAction(int Count) : IAction
    {
        public string Type => "About/ChangePetCount";
    }
}
