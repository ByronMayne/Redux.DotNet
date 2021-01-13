namespace Redux.DotNet.DevTools.SocketCluster
{
    internal record EmitResult(object error, object data)
    {
        /// <summary>
        /// Gets if there was an error that happened with this emit
        /// </summary>
        public bool Successful => error == null;
    }
}
