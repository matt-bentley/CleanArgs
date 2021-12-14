using CleanArgs;

args = new string[] { "-b", "-s", "Hello World!", "-i", "10" };
var arguments = new Args("b,s*,i#", args);

Console.WriteLine(arguments.GetBoolean('b')); // True
Console.WriteLine(arguments.GetInteger('i')); // 10
Console.WriteLine(arguments.GetString('s')); // Hello World!}