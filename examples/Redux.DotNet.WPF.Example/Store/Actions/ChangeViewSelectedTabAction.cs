using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangeViewSelectedTabAction(int Tab) : IAction
    {
        public string Type => "View/SelectedTab";
    }
}
