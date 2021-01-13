using ReduxSharp.Redux;
using System;

namespace ReduxSharp.Plugins.DevTools
{
    public class ReduxOptions
    {
        /// <summary>
        /// Gets or sets the naem of the client that shows up in the dev tool
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the socket information
        /// </summary>
        public SocketOptions SocketOptions { get; set; }

        public ReduxOptions()
        {
            ClientName = AppDomain.CurrentDomain.FriendlyName;
            SocketOptions = new SocketOptions();
        }
    }
}
