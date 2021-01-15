using System;

namespace Redux.DotNet.DevTools
{
    public class DevToolsOptions
    {
        /// <summary>
        /// Gets the name of the client that shows up in the dev tools.
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets the port that the remote tools is running on
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets the host that the remote tools are running one
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        ///  Gets or set if it will  include stack trace for every dispatched action, 
        ///  so you can see it in trace tab jumping directly to that part of code (more details).
        ///  You can use a function (with action object as argument) which 
        ///  should return new Error().stack string, getting the stack outside of reducers. 
        /// </summary>
        public bool Trace {get; set;}

        /// <summary>
        /// Creates a new instance of dev tools options with default values.
        /// </summary>
        public DevToolsOptions()
        {
            Port = 8000;
            Host = "localhost";
            ClientName = AppDomain.CurrentDomain.FriendlyName;
        }
    }
}
