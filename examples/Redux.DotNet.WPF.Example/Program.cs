using Redux.DotNet;
using ReduxSharp;
using ReduxSharp.Logging;
using ReduxSharp.WPF;
using System;
using System.Threading;

namespace ProfileEditor
{
    public static class Program
    {
        [STAThread]
        public static int Main()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                AppState initialState = new AppState();

                IStore<AppState> store = new StoreConfiguration<AppState>()
                              .UseReduxDevTools(options => options.ClientName = "RestSharp.WPF")
                              .UseMiddleware<LoggerMiddleware>()
                              .UseReducer<AboutReducer, About>(state => state.About)
                              .UseReducer<UserReducer, UserInfo>(state => state.User)
                              .UseReducer<ViewReducer, ViewState>(state => state.View)
                              .UseInitialState(initialState)
                              .UseDefaultActivator()
                              .UseDefaultLogger(options => options.MinimumLogLevel = LogLevel.Verbose)
                              .CreateStore<WPFStore<AppState>>();

                App app = new App(store);
                app.Run();
            }
            catch
            {
                return -1;
            }

            return 0;
        }
    }
}
