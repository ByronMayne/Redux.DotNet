using System;

namespace ReduxSharp.Logging
{
    public interface ILogger
    {
        /// <summary>
        /// Writes an error to the logger 
        /// </summary>
        public void Error(string messageTemplate, params object[] parameters);

        /// <summary>
        /// Writes fatal information to the logger
        /// </summary>
        public void Fatal(string messageTemplate, params object[] parameters);

        /// <summary>
        /// Writers information to the logger
        /// </summary>
        public void Information(string messageTemplate, params object[] parameters);
        
        /// <summary>
        /// Writes verbose content to the logger
        /// </summary>
        /// <param name="messageTemplate"></param>
        /// <param name="parameters"></param>
        public void Verbose(string messageTemplate, params object[] parameters);

        /// <summary>
        /// Writes a warning to the logger
        /// </summary>
        public void Warning(string messageTemplate, params object[] parameters);
    }
}
