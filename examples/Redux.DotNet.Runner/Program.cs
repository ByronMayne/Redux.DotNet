using ReduxSharp.Redux;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ReduxSharp.Runner
{
    class Program
    {
        public class CheckMateState
        {
            public int ChangelistCount { get; set; } = 0;
            public string Server { get; set; } = "Server";
        }
        static async Task Main(string[] args)
        {
            ReduxConnection reduxConnection = new ReduxConnection("CheckMate", SocketOptions.Default);
            CancellationToken cancellationToken = CancellationToken.None;

            await reduxConnection.ConnectAsync(cancellationToken);

            CheckMateState state = new CheckMateState();


            reduxConnection.UpdateState(state, "Setup");

            int count = 0;

            while (!cancellationToken.IsCancellationRequested)
            {

                count++;

                if(count % 2 == 0)
                {
                    state.ChangelistCount++;
                }
                else
                {
                    state.Server = $"Server {Guid.NewGuid():D}";
                }

                reduxConnection.UpdateState(state, "Update Loop");

                await Task.Delay(5000);
            }

            reduxConnection.Disconnect();
        }
    }
}
