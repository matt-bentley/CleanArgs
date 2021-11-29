using System;

namespace CleanArgs.Exceptions
{
    public class ArgsParseException : Exception
    {
        public readonly string ArgumentId;

        public ArgsParseException(string message, Exception innerException, string argumentId) : base(message, innerException)
        {
            ArgumentId = argumentId;
        }
    }
}
