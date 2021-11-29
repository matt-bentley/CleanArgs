using CleanArgs.Interfaces.Marshalers;

namespace CleanArgs.Interfaces.Factories
{
    public interface IArgumentMarshalerFactory
    {
        IArgumentMarshaler Create(string schema);
    }
}
