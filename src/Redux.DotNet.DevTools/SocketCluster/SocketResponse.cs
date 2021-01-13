using Newtonsoft.Json.Linq;

namespace Redux.DotNet.DevTools.SocketCluster
{
    /// <summary>
    /// Wraps a message that was received by a socket 
    /// </summary>
    internal struct SocketResponse
    {
        /// <summary>
        /// Gets the message that was sent
        /// </summary>
        public JToken Content { get; }

        /// <summary>
        /// Gets the <see cref="Socket"/> that received the message 
        /// </summary>
        public Socket Sender { get; }

        /// <summary>
        /// Gets if this message contains content 
        /// </summary>
        public bool HasContent => Content != null;

        /// <summary>
        /// Gets the type of the content 
        /// </summary>
        public JTokenType ContentType => HasContent ? Content.Type : JTokenType.None;

        /// <summary>
        /// Creates a new instance of a socket message 
        /// </summary>
        public SocketResponse(JToken content, Socket sender)
        {
            Content = content;
            Sender = sender;
        }
    }
}
