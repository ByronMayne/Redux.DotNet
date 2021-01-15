using ReduxSharp;

namespace Redux.DotNet.WPF.Example.Store.Actions
{
    internal record ChangeSiblingCountAction(int Count) : IAction
    {
        public string Type => "About/ChangeSiblingCount";
    }
}
