namespace Redux.DotNet.DevTools.SocketCluster
{
    internal enum MessageType
    {
        Authenticated,
        Publish,
        Removetoken,
        Settoken,
        Event,
        Ackreceive
    }
}
