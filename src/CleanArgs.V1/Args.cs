using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArgs
{
    public class Args
    {
        private Dictionary<char, bool> _booleanArgs = new Dictionary<char, bool>();
        private Dictionary<char, string> _stringArgs = new Dictionary<char, string>();
        private Dictionary<char, int> _integerArgs = new Dictionary<char, int>();

        public Args(string schema, string[] args)
        {
            ParseSchema(schema);
            ParseArguments(args);
        }

        private void ParseSchema(string schema)
        {
            var elements = schema.Split(',');
            int index = 0;
            foreach(var element in elements)
            {
                var trimmedElement = element.Trim();
                if (string.IsNullOrEmpty(trimmedElement))
                {
                    throw new ArgumentException($"No value provided for arguement: [{index}]");
                }

                var elementId = trimmedElement.First();

                if (!char.IsLetter(elementId))
                {
                    throw new ArgumentException($"Argument must be a letter: {trimmedElement}");
                }

                if (_booleanArgs.ContainsKey(elementId) 
                    || _stringArgs.ContainsKey(elementId)
                    || _integerArgs.ContainsKey(elementId))
                {
                    throw new ArgumentException($"Argument must be unique: {elementId}");
                }

                var elementSchema = element.Substring(1);

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
                        throw new FormatException($"Argument schema is invalid: {elementSchema}");
                }

                index++;
            }
        }

        public void ParseArguments(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                var trimmedArgument = args[i].Trim();
                if(trimmedArgument.First() == '-') // TODO: change to a const
                {
                    // this is an elementId
                    var element = trimmedArgument.Substring(1);
                    if(element.Length != 1)
                    {
                        throw new FormatException($"Argument name is not valid: {trimmedArgument}");
                    }

                    var elementId = element.First();

                    if (_booleanArgs.ContainsKey(elementId))
                    {
                        _booleanArgs[elementId] = true;
                    }
                    else if (_stringArgs.ContainsKey(elementId))
                    {
                        var elementValueIndex = i + 1;
                        if(elementValueIndex > (args.Length - 1))
                        {
                            throw new ArgumentException($"No value found for: {trimmedArgument}");
                        }
                        var elementValue = args[elementValueIndex];

                        if(elementValue.First() == '-')
                        {
                            throw new ArgumentException($"No value found for: {trimmedArgument}");
                        }

                        _stringArgs[elementId] = elementValue;
                    }
                    else if (_integerArgs.ContainsKey(elementId))
                    {
                        var elementValueIndex = i + 1;
                        if (elementValueIndex > (args.Length - 1))
                        {
                            throw new ArgumentException($"No value found for: {trimmedArgument}");
                        }
                        var elementValueString = args[elementValueIndex];

                        if (elementValueString.First() == '-')
                        {
                            throw new ArgumentException($"No value found for: {trimmedArgument}");
                        }

                        if (!int.TryParse(elementValueString, out int elementValue))
                        {
                            throw new FormatException($"Expecting a numeric value for: {trimmedArgument} but found: '{elementValueString}'");
                        }
                        _integerArgs[elementId] = elementValue;
                    }
                    else
                    {
                        throw new KeyNotFoundException($"Argument does not exist: {trimmedArgument}");
                    }
                }
                else
                {
                    // this is an elementValue
                }
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
