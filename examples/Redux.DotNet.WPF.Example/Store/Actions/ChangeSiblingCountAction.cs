using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangeSiblingCountAction(int Count) : IAction
    {
        public string Type => "About/ChangeSiblingCount";
    }
}
