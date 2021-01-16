using ReduxSharp;
using ReduxSharp.Reducers;

namespace ProfileEditor
{
    internal class AboutReducer : IReducer<About>
    {
        public AboutReducer()
        {
        }

        public About Reduce(About currentState, IAction action)
        {
            switch (action)
            {
                case ChangeSiblingCountAction siblingCount:
                    return currentState with { SiblingCount = siblingCount.Count };
                case ChangeFavoriteColorAction favoriteColor:
                    return currentState with { FavoriteColor = favoriteColor.Color };
                case ChangePetCountAction petCount:
                    return currentState with { PetCount = petCount.Count };
            }

            return currentState;
        }
    }
}
