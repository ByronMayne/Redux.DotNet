using Newtonsoft.Json.Linq;
using Redux.DotNet.DevTools.Monitor;
using Redux.DotNet.DevTools.SocketCluster;
using ReduxSharp;
using ReduxSharp.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Redux.DotNet.DevTools
{
    internal class ReduxConnection
    {
        public delegate void EventDataReceived(JToken stateData);
        private readonly string m_clientName;
        private readonly string m_clientId;
        private readonly Socket m_socket;
        private readonly ILog m_logger;
        private Action<IAction> m_dispatchAction;

        private string m_connectionId;

        /// <summary>
        /// Gets if we are currently connected to the dev tools.
        /// </summary>
        public bool IsConnected { get; private set; }

        public event EventDataReceived StateUpdate;


        public ReduxConnection(DevToolsOptions options, ILog logger)
        {
            m_logger = logger;
            IsConnected = false;
            m_clientName = options.ClientName;
            m_clientId = Guid.NewGuid().ToString("N");
            m_socket = new Socket($"ws://{options.Host}:{options.Port}/socketcluster/", m_logger);
            m_socket.ObjectRecevied += OnMessageReceived;
        }


        public void SetDispatch(Action<IAction> dispatch)
        {
            m_dispatchAction = dispatch;
        }


        public void Connect()
        {
            m_logger.Information("Connecting");
            Task.Run(() => ConnectAsync(CancellationToken.None));
        }


        public async Task ConnectAsync(CancellationToken cancellationToken)
        {
            m_logger.Information("Connecting Socket");

            Response<Socket.Authentication> authResponse = await m_socket.ConnectAsync(cancellationToken);

            if (!authResponse.Okay)
            {
                Debug.Fail($"Auth should not fail with error '{authResponse.Error}'.");
                return;
            }

            m_connectionId = authResponse.Value.Id;
            m_logger.Information($"Connection id set {m_connectionId}.");


            m_logger.Information("Logging In");
            Response<string> loginResult = await m_socket.EmitAsync<string>("login", "master");

            m_logger.Information("Login: {@LoginResult}", loginResult.Value);

            Channel loginChannel = m_socket.CreateChannel(loginResult.Value);

            await loginChannel.SubscribeAsync(cancellationToken);


            IsConnected = true;
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void InitState(object state, string actionName)
            => m_socket.Emit("log", CreatePayload(state, actionName, "INIT"));

        public async Task InitStateAsync<T>(object state, string actionName)
            => await m_socket.EmitAsync<T>("log", CreatePayload(state, actionName, "INIT"));

        public void UpdateState(object state, string actionName)
            => m_socket.Emit("log", CreatePayload(state, actionName, "ACTION"));

        public async Task UpdateStateAsync<T>(T state, string actionName)
            => await m_socket.EmitAsync<T>("log", CreatePayload(state, actionName, "ACTION"));

        private SocketMessage CreatePayload<T>(T state, string actionName, string type)
            => new SocketMessage()
            {
                Type = "ACTION",
                ClientName = m_clientName,
                InstanceId = m_clientId,
                SocketId = m_connectionId,
                Payload = state,
                Action =
                {
                    Type = actionName,
                    TimeStamp = DateTime.Now,
                }
            };

        /// <summary>
        /// Invoked whenver the socket we are using receives a message 
        /// </summary>
        private void OnMessageReceived(SocketResponse socketMessage)
        {
            switch (socketMessage.ContentType)
            {
                case JTokenType.String:
                    ProcessStringMesssage(socketMessage.Content.ToObject<string>());
                    break;
                case JTokenType.Object:
                    ProcessObjectMessage((JObject)socketMessage.Content);
                    break;
            }
        }

        /// <summary>
        /// Invoked to process any messages that contain object data 
        /// </summary>
        private void ProcessObjectMessage(JObject message)
        {
            // We only care about events
            if (!message.ContainsKey("event"))
            {
                return;
            }

            if (m_dispatchAction == null)
            {
                return;
            }

            JObject data = (JObject)message["data"];
            JToken action = data["action"];
            MonitorResponseType monitorType = data["type"].ToObject<MonitorResponseType>();

            switch (monitorType)
            {
                case MonitorResponseType.Dispatch:
                    ActionTypes dispatchType = action["type"].ToObject<ActionTypes>();

                    switch (dispatchType)
                    {
                        // Clicking 'jump' beisde a log
                        case ActionTypes.JumpToAction:
                        // Using the slider events
                        case ActionTypes.JumpToState:
                            m_dispatchAction.Invoke(data.ToObject<JumpToAction>());
                            break;
                        case ActionTypes.ToggleAction:
                            m_dispatchAction.Invoke(data.ToObject<ToggleAction>());
                            break;
                    }

                    //TODO: Dispatch Jump To Action


                    //TODO: Dispatch Toggle Action
                    break;
                case MonitorResponseType.Action:
                    //TODO: Preform Action 
                    m_dispatchAction.Invoke(message.ToObject<ExecuteAction>());
                    break;
            }
        }

        /// <summary>
        /// Invoked to process any message that are just basic strings
        /// </summary>
        private void ProcessStringMesssage(string messageData)
        {
            switch (messageData)
            {
                // Ping pong  (supports both versions)
                case "": m_socket.Send(""); return;
                case "#1": m_socket.Send("#2"); return;
                case null:
                    m_logger.Warning("Messaged was empty from server");
                    return;
            }
        }


    }
}
