using CleanArgs.V4.Builders;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CleanArgs.Tests
{
    [TestClass]
    public class ArgsBuilderTests
    {
        [TestMethod]
        public void GivenArgsBuilder_WhenDuplicateArgument_ThenThrow()
        {
            var builder = new ArgsBuilder(new string[] { "-l" })
                                .WithBoolean('l');
            Action action = () => builder.WithBoolean('l');
            action.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void GivenArgsBuilder_WhenBuildWithAllTypes_ThenBuild()
        {
            var arguments = new ArgsBuilder(new string[] { "-b", "-i", "10","-s","Hello World!", "-a", "Hello", "World" })
                                .WithBoolean('b')
                                .WithInteger('i')
                                .WithString('s')
                                .WithArray('a')
                                .Build();
            arguments.GetBoolean('b').Should().BeTrue();
            arguments.GetString('s').Should().Be("Hello World!");
            arguments.GetInteger('i').Should().Be(10);
            arguments.GetArray('a').Should().HaveCount(2);
        }
    }
}
