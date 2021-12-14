using CleanArgs.Factories;
using System.Text;

namespace CleanArgs.V4.Builders
{
    public class ArgsBuilder
    {
        private readonly string[] _args;
        private HashSet<char> _elementIds = new HashSet<char>();
        private StringBuilder _schema = new StringBuilder();

        public ArgsBuilder(string[] args)
        {
            _args = args;
        }

        public Args Build()
        {
            return new Args(_schema.ToString(), _args, new ArgumentMarshalerFactory());
        }

        public ArgsBuilder WithBoolean(char arg)
        {
            AddElementId(arg);
            AddSchemaElement(arg.ToString());
            return this;
        }

        public ArgsBuilder WithInteger(char arg)
        {
            AddElementId(arg);
            AddSchemaElement($"{arg}#");
            return this;
        }

        public ArgsBuilder WithString(char arg)
        {
            AddElementId(arg);
            AddSchemaElement($"{arg}*");
            return this;
        }

        public ArgsBuilder WithArray(char arg)
        {
            AddElementId(arg);
            AddSchemaElement($"{arg}[]");
            return this;
        }

        private void AddElementId(char elementId)
        {
            if (_elementIds.Contains(elementId))
            {
                throw new ArgumentException($"Duplicate argument: {elementId}");
            }
            _elementIds.Add(elementId);
        }

        private void AddSchemaElement(string schemaElement)
        {
            if(_schema.Length > 0)
            {
                _schema.Append(',');
            }
            _schema.Append(schemaElement);
        }
    }
}
