namespace ReduxSharp.Wpf
{
    public record ApplicationState(string TextValue, int IntValue)
    {
        public override string ToString()
        {
            return $"Text: '{TextValue}' Int: {IntValue}";
        }
    }
}
