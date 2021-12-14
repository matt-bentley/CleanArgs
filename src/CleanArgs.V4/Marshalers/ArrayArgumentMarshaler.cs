using CleanArgs.Marshalers.Abstract;

namespace CleanArgs.Marshalers
{
    internal class ArrayArgumentMarshaler : ArgumentMarshaler<string[]>
    {
        public ArrayArgumentMarshaler() : base(0,10)
        {

        }

        public override string[] Parse(List<string> values)
        {
            return values.ToArray();
        }
    }
}
