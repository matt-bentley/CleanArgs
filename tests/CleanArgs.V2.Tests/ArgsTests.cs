using CleanArgs.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CleanArgs.Tests
{
    [TestClass]
    public class ArgsTests
    {
        [TestMethod]
        public void GivenArgs_WhenBooleanTrue_ThenGetBooleanTrue()
        {
            var args = new Args("l", new string[] { "-l" });
            var value = args.GetBoolean('l');
            value.Should().BeTrue();
        }

        [TestMethod]
        public void GivenArgs_WhenBooleanNotProvided_ThenGetFalse()
        {
            var args = new Args("l", new string[] { });
            var value = args.GetBoolean('l');
            value.Should().BeFalse();
        }

        [TestMethod]
        public void GivenArgs_WhenNoArgumentValue_ThenThrow()
        {
            Action action = () => new Args(",l", new string[] { "-l" });
            action.Should().Throw<ArgsSchemaException>()
                           .Where(e => e.ArgumentIndex == 0)
                           .WithInnerException<ArgumentException>()
                           .WithMessage("No value provided for argument");
        }

        [TestMethod]
        public void GivenArgs_WhenDuplicateArgument_ThenThrow()
        {
            Action action = () => new Args("l,l", new string[] { "-l" });
            action.Should().Throw<ArgsSchemaException>()
                           .WithInnerException<ArgumentException>()
                           .WithMessage("Argument must be unique: l");
        }

        [TestMethod]
        public void GivenArgs_WhenNonLetterArgument_ThenThrow()
        {
            Action action = () => new Args("2", new string[] { "-2" });
            action.Should().Throw<ArgsSchemaException>()
                           .WithInnerException<ArgumentException>()
                           .WithMessage("Argument must be a letter: 2");
        }

        [TestMethod]
        public void GivenArgs_WhenInvalidArgumentId_ThenThrow()
        {
            Action action = () => new Args("l", new string[] { "-li" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<FormatException>()
                           .WithMessage("Argument name is not valid");
        }

        [TestMethod]
        public void GivenArgs_WhenArgumentParseException_ThenAddArgumentName()
        {
            Action action = () => new Args("l", new string[] { "-li" });
            action.Should().Throw<ArgsParseException>()
                           .Where(e => e.ArgumentId == "-li");
        }

        [TestMethod]
        public void GivenArgs_WhenInvalidArgumentPrefix_ThenThrow()
        {
            Action action = () => new Args("l", new string[] { "l" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<FormatException>()
                           .WithMessage("Argument must be prefixed with -");
        }

        [TestMethod]
        public void GivenArgs_WhenBooleanArgumentTypeNotExists_ThenThrow()
        {
            var args = new Args("l*", new string[] { "-l", "test" });
            Action action = () => args.GetBoolean('l');
            action.Should().Throw<KeyNotFoundException>().WithMessage("No argument was found for: l");
        }

        [TestMethod]
        public void GivenArgs_WhenStringArgumentTypeNotExists_ThenThrow()
        {
            var args = new Args("l", new string[] { "-l" });
            Action action = () => args.GetString('l');
            action.Should().Throw<KeyNotFoundException>()
                           .WithMessage("No argument was found for: l");
        }

        [TestMethod]
        public void GivenArgs_WhenArgumentNotExists_ThenThrow()
        {
            Action action = () => new Args("l", new string[] { "-i" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<KeyNotFoundException>().WithMessage("Argument does not exist");
        }

        [TestMethod]
        public void GivenArgs_WhenString_ThenGetString()
        {
            var args = new Args("l*", new string[] { "-l","Hello World!" });
            var value = args.GetString('l');
            value.Should().Be("Hello World!");
        }

        [TestMethod]
        public void GivenArgs_WhenStringNotProvided_ThenGetNull()
        {
            var args = new Args("l*", new string[] { });
            var value = args.GetString('l');
            value.Should().BeNull();
        }

        [TestMethod]
        public void GivenArgs_WhenStringValueNotProvided_ThenThrow()
        {
            Action action = () => new Args("l*", new string[] { "-l" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<ArgumentException>()
                           .WithMessage("No value found");
        }

        [TestMethod]
        public void GivenArgs_WhenStringValueMissed_ThenThrow()
        {
            Action action = () => new Args("l*,i", new string[] { "-l", "-i" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<ArgumentException>()
                           .WithMessage("No value found");
        }

        [TestMethod]
        public void GivenArgs_WhenInteger_ThenGetInteger()
        {
            var args = new Args("l#", new string[] { "-l", "10" });
            var value = args.GetInteger('l');
            value.Should().Be(10);
        }

        [TestMethod]
        public void GivenArgs_WhenIntegerNotProvided_ThenGetZero()
        {
            var args = new Args("l#", new string[] { });
            var value = args.GetInteger('l');
            value.Should().Be(0);
        }

        [TestMethod]
        public void GivenArgs_WhenIntegerValueNotProvided_ThenThrow()
        {
            Action action = () => new Args("l#", new string[] { "-l" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<ArgumentException>()
                           .WithMessage("No value found");
        }

        [TestMethod]
        public void GivenArgs_WhenIntegerValueMissed_ThenThrow()
        {
            Action action = () => new Args("l#,i", new string[] { "-l", "-i" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<ArgumentException>()
                           .WithMessage("No value found");
        }

        [TestMethod]
        public void GivenArgs_WhenIntegerNotNumeric_ThenThrow()
        {
            Action action = () => new Args("l#", new string[] { "-l", "test" });
            action.Should().Throw<ArgsParseException>()
                           .WithInnerException<FormatException>()
                           .WithMessage("Expecting a numeric value but found: 'test'");
        }

        [TestMethod]
        public void GivenArgs_WhenInvalidSchema_ThenThrow()
        {
            Action action = () => new Args("l£", new string[] { "-l", "test" });
            action.Should().Throw<ArgsSchemaException>()
                           .WithInnerException<FormatException>()
                           .WithMessage("Argument schema is invalid");
        }
    }
}
