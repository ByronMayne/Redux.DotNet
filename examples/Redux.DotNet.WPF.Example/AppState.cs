namespace Redux.DotNet.Wpf
{
    public record UserInfo(string FirstName, string LastName, int Age);
    public record About(string FavoriteColor, int PetCount, int SiblingCount);
    public record ViewState(int SelectedTab);

    public record AppState
    {
        public UserInfo User { get; init; }

        public About About { get; init; }

        public ViewState View { get; init; }

        public AppState()
        {
            User = new UserInfo("", "", 0);
            About = new About("", 0, 0);
            View = new ViewState(0);
        }
    }
}
