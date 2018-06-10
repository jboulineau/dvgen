using System;
using System.IO;
using dvgen.Repositories;
using CommandLineParser.Core;

namespace dvgen
{
    public class Program
    {
        private static FluentCommandLineParser<ConfigSettings> parser = new FluentCommandLineParser<ConfigSettings>();

        /// <Summary>
        /// dvgen is a code generator for the objects required to implement a database in the Data Vault pattern.
        /// </Summary>
        /// <remarks>
        /// Dependencies -
        /// CommandLineParser : https://github.com/valeravorobjev/CommandLineParser/tree/master/CommandLineParser.Core
        /// Konsole : https://github.com/goblinfactory/konsole
        /// </remarks>
        static void Main(string[] args)
        {
            SetupArgs();

            var result = parser.Parse(args);

            // No-op if the help menu is requested.
            if (result.HelpCalled) { return; }
            if (result.HasErrors) { Console.WriteLine(result.ErrorText); }
            if (parser.Object.ValidationErrors) { return; }

            LoadEntities(parser.Object.InputPath);
        }

        private static void LoadEntities(string path)
        {
            EntityRepository er = new EntityRepository(parser.Object);
            er.LoadEntities(path);
        }

        /// <Summary>
        /// This method configures CommandLineParser to handle arguments passed to the program.
        /// </Summary>
        private static void SetupArgs()
        {
            // Instructs CommandLineParser to display formatted help text for every available argument.
            parser.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text));

            parser.Setup(a => a.InputPath)
                .As('i', "input")
                .SetDefault("input")
                .WithDescription("The path to the directory where JSON entity configuration files are located. Default value: 'input'");

            parser.Setup(a => a.OutputPath)
                .As('o', "output")
                .SetDefault("output")
                .WithDescription("The path to the directory where generated files will be written. Default value: 'output'");

            parser.Setup(a => a.Verbose)
                .As('v',"verbose")
                .SetDefault(false)
                .WithDescription("Determines if verbose output will be printed. Default value: 'false'");
        }
    }
}