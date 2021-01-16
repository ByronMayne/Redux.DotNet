using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangeLastNameAction(string Value) : IAction
    {
        public string Type => "User/ChangeLastName";
    }
}
