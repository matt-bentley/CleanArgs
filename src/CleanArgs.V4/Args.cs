using CleanArgs.Exceptions;
using CleanArgs.Interfaces.Factories;
using CleanArgs.Interfaces.Marshalers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanArgs.V4.Tests")]
namespace CleanArgs
{
    public class Args
    {
        private Dictionary<char, IArgumentMarshaler> _marshalers = new Dictionary<char, IArgumentMarshaler>();
        private readonly IArgumentMarshalerFactory _argumentMarshalerFactory;
        private const char ARGUMENT_PREFIX = '-';

        internal Args(string schema, string[] args, IArgumentMarshalerFactory argumentMarshalerFactory)
        {
            _argumentMarshalerFactory = argumentMarshalerFactory;
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
            _marshalers.Add(elementId, _argumentMarshalerFactory.Create(elementSchema));
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
            return _marshalers.ContainsKey(elementId);
        }

        public void ParseArguments(string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                var argument = args[i];
                int argumentsAfterElement = args.Length - i - 1;
                var argumentValues = GetArgumentValues(args.TakeLast(argumentsAfterElement));
                ParseArgument(argument, argumentValues);
                SkipArgumentValues(ref i, argumentValues.Count);
            }
        }

        private void SkipArgumentValues(ref int i, int argumentValuesCount)
        {
            i += argumentValuesCount;
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
            var marshaler = GetMarshaler(elementId);
            marshaler.Set(values);            
        }

        private IArgumentMarshaler GetMarshaler(char elementId)
        {
            if (ElementIdIsRegistered(elementId))
            {
                return _marshalers[elementId];
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

        public bool GetBoolean(char arg)
        {
            return GetValue<bool>(arg);
        }

        public string GetString(char arg)
        {
            return GetValue<string>(arg);
        }

        public int GetInteger(char arg)
        {
            return GetValue<int>(arg);
        }

        public string[] GetArray(char arg)
        {
            return GetValue<string[]>(arg);
        }

        public T GetValue<T>(char arg)
        {
            var marshaler = GetTypedMarshaler<T>(arg);
            return marshaler.Get();
        }

        private IArgumentMarshaler<T> GetTypedMarshaler<T>(char elementId)
        {
            if (!ElementIdIsRegistered(elementId))
            {
                throw new KeyNotFoundException($"No argument was found for: {elementId}");
            }
            var marshaler = _marshalers[elementId] as IArgumentMarshaler<T>;
            if (marshaler == null)
            {
                throw new KeyNotFoundException($"Argument type is incorrect for: {elementId}");
            }
            return marshaler;
        }
    }
}
