using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReduxSharp.SocketCluster
{
    class Channel
    {
        private readonly WeakReference<Socket> m_owner;
        private readonly string m_name;

        public Channel(string name, Socket owner)
        {
            m_owner = new WeakReference<Socket>(owner);
            m_name = name;
        }

        /// <summary>
        /// Subscribes to this channels events
        /// </summary>
        public async Task SubscribeAsync(CancellationToken cancellationToken)
        {
            if (m_owner.TryGetTarget(out Socket owner))
            {
                await owner.SubscribeChannelAsync(m_name, cancellationToken);
            }
        }


        /// <summary>
        /// Unsubscribes to this channels events
        /// </summary>
        public async Task UnsubscribeAsync(CancellationToken cancellationToken)
        {
            if(m_owner.TryGetTarget(out Socket owner))
            {
                await owner.UnsubscribeChannelAsync(m_name, cancellationToken);
            }
        }
    }
}
