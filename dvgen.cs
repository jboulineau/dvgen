using System;
using CommandLineParser.Core;

namespace dvgen
{
    public class CommandLineArguments 
    {
        public int test {get; set;}
    }

    public class Program
    {
        private static FluentCommandLineParser<CommandLineArguments> parser = new FluentCommandLineParser<CommandLineArguments>();
        
        static void Main(string[] args)
        {
            SetupArgs();

            var result = parser.Parse(args);

            if(!result.HelpCalled)
            {
                Console.WriteLine("Hello World!");
            }

            Console.WriteLine(result.ErrorText);
        }

        private static void SetupArgs()
        {
            parser.SetupHelp("?", "help")
            .Callback(text => Console.WriteLine(text))
            .UseForEmptyArgs();

            parser.Setup(a => a.test)
                .As('t',"test")
                .WithDescription("This is a test")
                .Required();
        }
    }
}