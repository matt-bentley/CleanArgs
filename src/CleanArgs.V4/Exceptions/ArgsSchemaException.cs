namespace CleanArgs.Exceptions
{
    public class ArgsSchemaException : Exception
    {
        public readonly int ArgumentIndex;

        public ArgsSchemaException(string message, Exception innerException, int argumentIndex) : base(message, innerException)
        {
            ArgumentIndex = argumentIndex;
        }
    }
}
