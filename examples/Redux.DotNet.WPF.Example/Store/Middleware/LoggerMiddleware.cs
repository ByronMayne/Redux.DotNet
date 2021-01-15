using ReduxSharp;
using ReduxSharp.Middleware;
using System.Threading.Tasks;

namespace Redux.DotNet.Wpf.Store.Middleware
{
    internal class LoggerMiddleware : IMiddleware
    {
        private ActionDispatchDelegate m_next;

        public LoggerMiddleware(ActionDispatchDelegate next)
        {
            m_next = next;
        }

        public void Initialize(IStore<AppState> state)
        {
        }

        public async Task InvokeAsync(IActionContext actionContext)
        {
            await m_next(actionContext);
        }
    }
}
