using CleanArgs.Marshalers.Abstract;

namespace CleanArgs.Marshalers
{
    internal class StringArgumentMarshaler : ArgumentMarshaler<string>
    {
        public StringArgumentMarshaler() : base(1)
        {

        }

        public override string Parse(List<string> values)
        {
            return values.First();
        }
    }
}
