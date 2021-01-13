namespace ReduxSharp.Redux
{
    public record SocketOptions
    {
        public static SocketOptions Default { get; } = new SocketOptions();

        public bool IsSecure { get; init; }
        public string HostName { get; init; }
        public int Port { get; init; }
        public bool AutoReconnect { get; init; }
        
        public SocketOptions()
        {
            IsSecure = true;
            HostName = "localhost";
            Port = 8000;
            AutoReconnect = true;
        }
    }
}
