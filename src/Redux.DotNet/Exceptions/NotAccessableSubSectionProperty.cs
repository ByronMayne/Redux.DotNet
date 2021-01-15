namespace Redux.DotNet.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a property that is used for a sub section selector does not match the requirements
    /// </summary>
    public class NotAccessableSubSectionProperty : ReduxSharpException
    {
        public NotAccessableSubSectionProperty(string message) : base(message)
        {
        }
    }
}
