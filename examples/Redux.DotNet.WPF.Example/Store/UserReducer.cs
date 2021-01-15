using Redux.DotNet.Wpf;
using Redux.DotNet.Wpf.Store.Actions;
using ReduxSharp;
using ReduxSharp.Reducers;

namespace Redux.DotNet.WPF.Store
{
    internal class UserReducer : IReducer<UserInfo>
    {
        public UserInfo Reduce(UserInfo currentState, IAction action)
        {
            switch (action)
            {
                case ChangeAgeAction changeAge:
                    return currentState with { Age = changeAge.NewAge };
                case ChangeNameAction chagneName:
                    return currentState with
                    {
                        FirstName = chagneName.FirstName,
                        LastName = chagneName.LastName
                    };
            }

            return currentState;
        }
    }
}
