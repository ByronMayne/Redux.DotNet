using ReduxSharp.Plugins.DevTools;
using ReduxSharp.Redux;

namespace ReduxSharp
{

    public static class DevToolsExtensions
    {

        /// <summary>
        /// Adds a instance of <see cref="DevToolsMiddleware{TState}"/> to control sending events to the developers tool and a <see cref="DevToolsReducer"/>
        /// to handle patching of state with the dev tools window.
        /// </summary>
        /// <typeparam name="T">The root state type</typeparam>
        /// <param name="buidler">The current builder allowing for chaining of requests</param>
        /// <returns>The configuration sent in</returns>
        public static IStoreConfiguration<T> UseReduxDevTools<T>(this IStoreConfiguration<T> builder, ConfigurationDelegate<ReduxOptions> options = null)
        {
            builder.UseSingleton<ReduxConnection, ReduxOptions>(options);
            builder.UseMiddleware<DevToolsMiddleware<T>, ReduxOptions>(options);
            builder.UseReducer<DevToolsReducer<T>>();
            return builder;
        }


    }
}
