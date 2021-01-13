﻿using ReduxSharp.Middleware;
using ReduxSharp.Redux;
using ReduxSharp.Redux.Actions;
using System.Threading;
using System.Threading.Tasks;

namespace ReduxSharp.Plugins.DevTools
{
    public class DevToolsMiddleware<T> : IMiddleware<T>
    {
        private ActionDispatchDelegate m_next;
        private readonly ReduxConnection m_connection;

        public DevToolsMiddleware(ReduxConnection connection, ActionDispatchDelegate next)
        {
            m_connection = connection;
            m_next = next;
        }

        public async Task InvokeAsync(IActionContext<T> actionContext)
        {
            if (actionContext.Action is InitializeAction)
            {
                await m_connection.ConnectAsync(CancellationToken.None);

                m_connection.InitState(actionContext.Store.GetState(), "");
                m_connection.SetDispatch(actionContext.Store.Dispatch);
            }

            await m_next(actionContext);

            // We are connected and we are not the base action
            if (m_connection.IsConnected && !(actionContext.Action is BaseAction))
            {
                m_connection.UpdateState(actionContext.Result, actionContext.Action.Type);
            }

        }
    }
}
