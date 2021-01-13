namespace Redux.DotNet.DevTools.SocketCluster
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


}
