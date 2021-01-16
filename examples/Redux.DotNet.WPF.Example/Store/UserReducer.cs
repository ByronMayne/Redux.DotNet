using ReduxSharp;
using ReduxSharp.Reducers;

namespace ProfileEditor
{
    internal class UserReducer : IReducer<UserInfo>
    {
        public UserInfo Reduce(UserInfo currentState, IAction action)
        {
            switch (action)
            {
                case ChangeAgeAction changeAge:
                    return currentState with { Age = changeAge.NewAge };
                case ChangeLastNameAction lastName:
                    return currentState with { LastName = lastName.Value };
                case ChangeFirstNameAction firstName:
                    return currentState with { FirstName = firstName.Value };
            }

            return currentState;
        }
    }
}
