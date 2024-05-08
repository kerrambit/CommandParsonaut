﻿namespace CommandParsonaut.Core.Types
{
    /// <summary>
    /// Stores one possible result at the moment. User should ask which data is accessible or if the user knows
    /// that e.g. command was parsed successfully, can query the right data right away.
    /// </summary>
    public class ParameterResult
    {
        public int Integer { get; } = 0;
        public double Double { get; } = 0.0;
        public string String { get; } = string.Empty;
        public string Enum { get; } = string.Empty;

        private (bool isAvailable, ParameterType Type) _availableType = (false, ParameterType.Integer);

        public ParameterResult(int param)
        {
            Integer = param;
            _availableType = (true, ParameterType.Integer);
        }

        public ParameterResult(double param)
        {
            Double = param;
            _availableType = (true, ParameterType.Double);
        }

        public ParameterResult(string param, bool isEnum = false)
        {
            if (isEnum)
            {
                Enum = param;
                _availableType = (true, ParameterType.Enum);
            }
            else
            {
                String = param;
                _availableType = (true, ParameterType.String);
            }
        }

        public bool CheckResultAvailability(ParameterType targetType)
        {
            return _availableType.isAvailable && _availableType.Type == targetType;
        }
    }
}