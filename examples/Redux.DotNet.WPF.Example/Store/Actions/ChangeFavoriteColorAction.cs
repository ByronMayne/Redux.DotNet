using Redux.DotNet;
using ReduxSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redux.DotNet.WPF.Example.Store.Actions
{
    internal record ChangeFavoriteColorAction(string Color) : IAction
    {
        public string Type => "User/ChangeName";
    }
}
