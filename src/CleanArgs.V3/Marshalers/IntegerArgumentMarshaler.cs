using CleanArgs.Marshalers.Abstract;

namespace CleanArgs.Marshalers
{
    public class IntegerArgumentMarshaler : ArgumentMarshaler<int>
    {
        public IntegerArgumentMarshaler() : base(1)
        {

        }

        public override int Parse(List<string> values)
        {
            var elementValueString = values.First();

            if (!int.TryParse(elementValueString, out int elementValue))
            {
                throw new FormatException($"Expecting a numeric value but found: '{elementValueString}'");
            }
            return elementValue;
        }
    }
}
