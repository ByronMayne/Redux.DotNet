using Redux.DotNet.DevTools;
using ReduxSharp;

namespace Redux.DotNet
{
    public static class StoreConfigurationExtensions
    {
        /// <summary>
        /// Adds the ability for a remote Redux Dev Tools Monitor to connect to this instance and interact with it.
        /// </summary>
        /// <typeparam name="T">The root state type</typeparam>
        /// <param name="buidler">The current builder allowing for chaining of requests</param>
        /// <returns>The configuration sent in</returns>
        public static IStoreConfiguration<T> UseReduxDevTools<T>(this IStoreConfiguration<T> builder, ConfigurationDelegate<DevToolsOptions> options = null)
        {
            builder.UseSingleton<ReduxConnection, DevToolsOptions>(options);
            builder.UseMiddleware<Middleware<T>, DevToolsOptions>(options);
            builder.UseRootReducer<Reducer<T>, DevToolsOptions>(options);
            return builder;
        }
    }
}
