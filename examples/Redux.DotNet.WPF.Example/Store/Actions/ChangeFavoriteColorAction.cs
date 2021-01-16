using ReduxSharp;

namespace ProfileEditor
{
    internal record ChangeFavoriteColorAction(string Color) : IAction
    {
        public string Type => "User/ChangeName";
    }
}
