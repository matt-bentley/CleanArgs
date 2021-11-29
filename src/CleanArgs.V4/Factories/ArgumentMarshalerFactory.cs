using CleanArgs.Interfaces.Factories;
using CleanArgs.Interfaces.Marshalers;
using CleanArgs.Marshalers;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CleanArgs.V4.Tests")]
namespace CleanArgs.Factories
{
    internal class ArgumentMarshalerFactory : IArgumentMarshalerFactory
    {
        public IArgumentMarshaler Create(string schema)
        {
            return schema switch
            {
                "" => new BooleanArgumentMarshaler(),
                "*" => new StringArgumentMarshaler(),
                "#" => new IntegerArgumentMarshaler(),
                "[]" => new ArrayArgumentMarshaler(),
                _ => throw new FormatException($"Argument schema is invalid")
            };
        }
    }
}
