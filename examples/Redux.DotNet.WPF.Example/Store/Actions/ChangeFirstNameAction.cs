using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangeFirstNameAction(string Value) : IAction
    {
        public string Type => "User/ChangeFirstName";
    }
}
