namespace CleanArgs.Interfaces.Marshalers
{
    public interface IArgumentMarshaler<T> : IArgumentMarshaler
    {
        new T Get();
    }

    public interface IArgumentMarshaler
    {
        object Get();
        void Set(List<string> values);
    }
}
