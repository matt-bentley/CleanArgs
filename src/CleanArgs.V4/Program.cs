using CleanArgs.V4.Builders;

args = new string[] { "-b", "-s", "Hello World!", "-i", "10" };
var arguments = new ArgsBuilder(args)
                    .WithBoolean('b')
                    .WithInteger('i')
                    .WithString('s')
                    .Build();

Console.WriteLine(arguments.GetBoolean('b')); // True
Console.WriteLine(arguments.GetInteger('i')); // 10
Console.WriteLine(arguments.GetString('s')); // Hello World!
Console.WriteLine(arguments.GetValue<string>('s')); // Hello World!