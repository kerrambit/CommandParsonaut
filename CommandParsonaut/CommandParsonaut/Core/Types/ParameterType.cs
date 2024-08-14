namespace CommandParsonaut.Core.Types
{
    /// <summary>
    /// Lists all possible parameter types used by Command class.
    /// </summary>
    public enum ParameterType
    {
        Integer,
        IntegerRange,
        Double,
        DoubleRange,
        String,
        Enum,
        Uri,
        Email
    }
}
