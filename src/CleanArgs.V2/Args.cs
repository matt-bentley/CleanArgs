using CleanArgs.Exceptions;

namespace CleanArgs
{
    public class Args
    {
        private Dictionary<char, bool> _booleanArgs = new Dictionary<char, bool>();
        private Dictionary<char, string> _stringArgs = new Dictionary<char, string>();
        private Dictionary<char, int> _integerArgs = new Dictionary<char, int>();
        private const char ARGUMENT_PREFIX = '-';

        public Args(string schema, string[] args)
        {
            ParseSchema(schema);
            ParseArguments(args);
        }

        private void ParseSchema(string schema)
        {
            var elements = schema.Split(',');
            for(int i = 0; i < elements.Length; i++)
            {
                try
                {
                    ParseSchemaElement(elements[i]);
                }
                catch (Exception ex)
                {
                    throw new ArgsSchemaException("Error parsing schema", ex, i);
                }
            }
        }

        private void ParseSchemaElement(string element)
        {
            var trimmedElement = TrimSchemaElement(element);
            var elementId = trimmedElement.First();
            ValidateSchemaElement(elementId, trimmedElement);
            var elementSchema = trimmedElement.Substring(1);

            switch (elementSchema)
            {
                case "":
                    _booleanArgs.Add(elementId, default(bool));
                    break;
                case "*":
                    _stringArgs.Add(elementId, default(string));
                    break;
                case "#":
                    _integerArgs.Add(elementId, default(int));
                    break;
                default:
                    throw new FormatException($"Argument schema is invalid");
            }
        }

        private string TrimSchemaElement(string element)
        {
            var trimmedElement = element.Trim();
            if (string.IsNullOrEmpty(trimmedElement))
            {
                throw new ArgumentException($"No value provided for argument");
            }
            return trimmedElement;
        }

        private void ValidateSchemaElement(char elementId, string trimmedElement)
        {
            if (!char.IsLetter(elementId))
            {
                throw new ArgumentException($"Argument must be a letter: {trimmedElement}");
            }

            if (ElementIdIsRegistered(elementId))
            {
                throw new ArgumentException($"Argument must be unique: {elementId}");
            }
        }

        private bool ElementIdIsRegistered(char elementId)
        {
            return _booleanArgs.ContainsKey(elementId)
                || _stringArgs.ContainsKey(elementId)
                || _integerArgs.ContainsKey(elementId);
        }

        public void ParseArguments(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                var argument = args[i];
                int argumentsAfterElement = args.Length - i - 1;
                var argumentValues = GetArgumentValues(args.TakeLast(argumentsAfterElement));
                ParseArgument(argument, argumentValues);

                // Skip looping over the argument values
                i += argumentValues.Count;
            }
        }

        public List<string> GetArgumentValues(IEnumerable<string> args)
        {
            return args.TakeWhile(e => !IsElementId(e))
                       .ToList();
        }

        private void ParseArgument(string argument, List<string> values)
        {
            var trimmedArgument = argument.Trim();
            try
            {
                var elementId = GetArgumentElementId(trimmedArgument);
                ExtractArgumentValues(elementId, values);
            }
            catch (Exception ex)
            {
                throw new ArgsParseException("Error parsing argument", ex, trimmedArgument);
            }
        }

        private void ExtractArgumentValues(char elementId, List<string> values)
        {
            if (_booleanArgs.ContainsKey(elementId))
            {
                ParseBooleanArgument(elementId);
            }
            else if (_stringArgs.ContainsKey(elementId))
            {
                ParseStringArgument(elementId, values);
            }
            else if (_integerArgs.ContainsKey(elementId))
            {
                ParseIntegerArgument(elementId, values);
            }
            else
            {
                throw new KeyNotFoundException($"Argument does not exist");
            }
        }

        private char GetArgumentElementId(string argument)
        {
            if (!IsElementId(argument))
            {
                throw new FormatException($"Argument must be prefixed with {ARGUMENT_PREFIX}");
            }
            var element = argument.Substring(1);
            if (element.Length != 1)
            {
                throw new FormatException($"Argument name is not valid");
            }
            return element.First();
        }

        private bool IsElementId(string element)
        {
            return element.First() == ARGUMENT_PREFIX;
        }

        private void ParseBooleanArgument(char elementId)
        {
            _booleanArgs[elementId] = true;
        }

        private void ParseStringArgument(char elementId, List<string> values)
        {
            ValidateValuesCount(values);
            var elementValue = values.First();

            _stringArgs[elementId] = elementValue;
        }

        private void ParseIntegerArgument(char elementId, List<string> values)
        {
            ValidateValuesCount(values);
            var elementValueString = values.First();

            if (!int.TryParse(elementValueString, out int elementValue))
            {
                throw new FormatException($"Expecting a numeric value but found: '{elementValueString}'");
            }
            _integerArgs[elementId] = elementValue;
        }

        private void ValidateValuesCount(List<string> values)
        {
            if (values.Count == 0)
            {
                throw new ArgumentException($"No value found");
            }
        }

        public bool GetBoolean(char arg)
        {
            if(_booleanArgs.TryGetValue(arg, out bool value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"No argument was found for: {arg}");
            }
        }

        public string GetString(char arg)
        {
            if (_stringArgs.TryGetValue(arg, out string value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"No argument was found for: {arg}");
            }
        }

        public int GetInteger(char arg)
        {
            if (_integerArgs.TryGetValue(arg, out int value))
            {
                return value;
            }
            else
            {
                throw new KeyNotFoundException($"No argument was found for: {arg}");
            }
        }
    }
}
