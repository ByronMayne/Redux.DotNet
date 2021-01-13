using ReduxSharp.Logging;

namespace ReduxSharp
{
    public static class LoggingExtensions
    {
        /// <summary>
        /// Changes the logger to use the default one provided by Redux.
        /// </summary>
        public static IStoreConfiguration<T> UseDefaultLogger<T>(this IStoreConfiguration<T> buidler, ConfigurationDelegate<LoggerOptions> options = null)
        {
            if (options == null)
            {
                options = (op) => op.MinimumLogLevel = LogLevel.Warning;
            }

            buidler.UseLogger<DefaultLogger, LoggerOptions>(options);
            return buidler;
        }
    }
}
