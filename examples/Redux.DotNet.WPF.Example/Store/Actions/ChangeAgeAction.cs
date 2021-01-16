using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangeAgeAction(int NewAge) : IAction
    {
        public string Type => "User/ChangeAge";
    }
}
