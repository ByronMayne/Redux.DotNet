using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebSocket4Net;

namespace ReduxSharp.SocketCluster
{
    internal record Response<T>(int Id, T Value, bool Okay, string Error)
    {
        public void Deconstruct(out T result)
        {
            result = Value;
        }

        public void Deconstruct(out T result, out bool okay)
        {
            result = Value;
            okay = Okay;
        }

        public void Deconstruct(out T result, out bool okay, out string error)
        {
            result = Value;
            okay = Okay;
            error = Error;
        }

        public void Deconstruct(out int responseId, out T result, out bool okay, out string error)
        {
            result = Value;
            okay = Okay;
            error = Error;
            responseId = Id;
        }
    }



    internal class Socket
    {
        public record Authentication(string Id, int PingTimeout, bool IsAuthenticated);


        public delegate void SocketObjectMessageDelegate(SocketMessage message);

        private readonly ILogger m_log;
        private readonly List<Channel> m_channels;
        private readonly Dictionary<long, TaskCompletionSource<JToken>> m_pendingResponses;
        private long m_nextCallId;
        private readonly WebSocket m_socket;

        private TaskCompletionSource<Response<Authentication>> m_connectedCompletionSource;

        /// <summary>
        /// Invoked whenver the socket receives a message 
        /// </summary>
        public SocketObjectMessageDelegate ObjectRecevied;

        /// <summary>
        /// Gets the URL the socket is connecting too
        /// </summary>
        public Uri Uri { get; }

        /// <summary>
        /// Gets the list of channels
        /// </summary>
        public IReadOnlyList<Channel> Channels { get; }

        /// <summary>
        /// Gets the id of the socket
        /// </summary>
        public string Id { get; private set; }

        public Socket(string url, ILogger logger)
        {
            Uri = new Uri(url);
            m_socket = new WebSocket(url);
            m_nextCallId = 0;
            m_channels = new List<Channel>();
            m_pendingResponses = new Dictionary<long, TaskCompletionSource<JToken>>();
            m_log = logger;

            m_socket.Opened += OnSocketOpened;
            m_socket.Closed += OnSocketClosed;
            m_socket.Error += OnSocketError;
            m_socket.MessageReceived += OnSocketMessageReceived;
        }


        /// <summary>
        /// Emits an event on the sever asyn
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<Response<T>> EmitAsync<T>(string eventName, object data)
        {
            m_log.Information("Emit {EventName} {@Data}", eventName, data);

            SocketRequest request = new SocketRequest(GetNextCallId())
            {
                Data = data,
                EventName = eventName,
            };

            return await SendRequestAsync<T>(request);
        }

        public void Emit(string eventName, object data)
        {
            m_log.Information("Emit {EventName} {@Data}", eventName, data);

            SocketRequest request = new SocketRequest(GetNextCallId())
            {
                Data = data,
                EventName = eventName,
            };

            request.Send(m_socket);
        }

        /// <summary>
        /// Sends raw content to the socket.
        /// </summary>
        public void Send(string content)
        {
            m_socket.Send(content);
        }

        /// <summary>
        /// Creates a new channel
        /// </summary>
        /// <param name="channelName">The name of the channel</param>
        /// <returns>The created channel</returns>
        public Channel CreateChannel(string channelName)
        {
            Channel channel = new Channel(channelName, this);
            m_channels.Add(channel);
            return channel;
        }

        /// <summary>
        /// Attempts to connect the socket to the server and returns back when it has completed.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the request</param>
        /// <returns>True if we could connect otherwise false</returns>
        public async Task<Response<Authentication>> ConnectAsync(CancellationToken cancellationToken)
        {
            m_connectedCompletionSource = new TaskCompletionSource<Response<Authentication>>();

            cancellationToken.Register(m_connectedCompletionSource.SetCanceled);

            await m_socket.OpenAsync();

            return await m_connectedCompletionSource.Task;
        }


        internal async Task SubscribeChannelAsync(string channelName, CancellationToken cancellation)
        {
            m_log.Information("SubscribeChannel {ChannelName}", channelName);

            SocketRequest request = new SocketRequest(GetNextCallId())
            {
                EventName = "#subscribe",
                Data = new
                {
                    ChannelName = channelName,
                }
            };


            await SendRequestAsync<object>(request);
        }


        internal Task UnsubscribeChannelAsync(string channelName, CancellationToken cancellation)
        {
            m_log.Information("Unsubscribe {ChannelName}", channelName);
            return Task.CompletedTask;
        }

        private void OnSocketError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
             => m_log.Error("Socket Error {@Error}", e);

        private void OnSocketClosed(object sender, EventArgs e)
            => m_log.Information("Socket Opened");

        private void OnSocketOpened(object sender, EventArgs e)
            => Task.Run(async () =>
            {
                m_log.Information("Socket Closed");
                m_nextCallId = 0;

                SocketRequest request = new SocketRequest(GetNextCallId())
                {
                    EventName = "#handshake",
                };

                Response<Authentication> response = await SendRequestAsync<Authentication>(request);

                request.Send(m_socket);
                
                m_connectedCompletionSource.SetResult(response);
            });

        private void OnSocketMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            m_log.Information("Message {Message}", e.Message);

            JToken content;

            string message = e.Message;

            if (message == null)
            {
                return;
            }

            // Check if it's not json
            if (message.Length == 0 || (message[0] != '[' && message[0] != '{'))
            {
                content = new JValue(message);
            }
            else
            {

                try
                {
                    if ((content = JToken.Parse(e.Message)) != null)
                    {
                        if (content["rid"] is JToken rid)
                        {
                            long responseId = rid.ToObject<long>();

                            if (m_pendingResponses.TryGetValue(responseId, out TaskCompletionSource<JToken> emitResult))
                            {
                                m_pendingResponses.Remove(responseId);
                                emitResult.SetResult(content);
                            }
                        }
                    }
                }
                catch
                {
                    // Fallback an non-json value
                    content = new JValue(e.Message);
                }
            }

            ObjectRecevied?.Invoke(new SocketMessage(content, this));
        }

        private long GetNextCallId() => Interlocked.Increment(ref m_nextCallId);

        /// <summary>
        /// Enqueues a new result object and waits for the servers response
        /// </summary>
        private async Task<Response<T>> SendRequestAsync<T>(SocketRequest request)
        {
            TaskCompletionSource<JToken> result = new TaskCompletionSource<JToken>();
            m_pendingResponses.Add(request.CallId, result);

            request.Send(m_socket);

            JObject response = (JObject)await result.Task;

            int responseId = response["rid"].ToObject<int>();
            bool okay = !response.ContainsKey("error");
            string error = okay ? "" : response["error"]["message"].ToString();
            T value = okay ? response["data"].ToObject<T>() : default(T);

            return new Response<T>(responseId, value, okay, error);
        }
    }


}
