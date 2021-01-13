
using ReduxSharp.Activation;

namespace ReduxSharp
{
    public static class DefaultActivatorExtensions
    {
        /// <summary>
        /// Changes the current activator to be the default one implemented by Redux Sharp.
        /// </summary>
        public static IStoreConfiguration<TState> UseDefaultActivator<TState>(this IStoreConfiguration<TState> configuration)
            => configuration.UseActivator<DefaultActivator>();
    }
}
