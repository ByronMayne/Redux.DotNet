using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangePetCountAction(int Count) : IAction
    {
        public string Type => "About/ChangePetCount";
    }
}
