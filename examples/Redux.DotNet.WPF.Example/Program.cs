using Redux.DotNet.Wpf.Store.Middleware;
using ReduxSharp;
using ReduxSharp.Logging;
using ReduxSharp.Wpf;
using ReduxSharp.Wpf.Store;
using ReduxSharp.WPF;
using ReduxSharp.WPF.Example;
using System;
using System.Threading;

namespace Redux.DotNet.Wpf
{
    public static class Program
    {
        [STAThread]
        public static int Main()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            try
            {
                ApplicationState initialState = new ApplicationState("Hello", 32);

                IStore<ApplicationState> store = new StoreConfiguration<ApplicationState>()
                              .UseReduxDevTools(options => options.ClientName = "RestSharp.WPF")
                              .UseMiddleware<LoggerMiddleware>()
                              .UseReducer<AppStateReducer>()
                              .UseInitialState(initialState)
                              .UseDefaultActivator()
                              .UseDefaultLogger(options => options.MinimumLogLevel = LogLevel.Verbose)
                              .CreateStore<WPFStore<ApplicationState>>();

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
