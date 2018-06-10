using System;
using System.IO;
using dvgen.Model;
using dvgen.Repositories;
using dvgen.CodeGenerator;
using CommandLineParser.Core;
using System.Collections.Generic;

namespace dvgen
{
    public class Program
    {
        private static EnvironmentManager _envManager;
        /// <Summary>
        /// dvgen is a code generator for the objects required to implement a database in the Data Vault pattern.
        /// </Summary>
        /// <remarks>
        /// Dependencies -        
        /// JSON.NET : https://www.newtonsoft.com/json
        /// Konsole : https://github.com/goblinfactory/konsole
        /// JSON.NET Schema : https://www.newtonsoft.com/jsonschema        
        /// CommandLineParser : https://github.com/valeravorobjev/CommandLineParser/tree/master/CommandLineParser.Core
        /// Rhetos FastReplacer : https://github.com/Rhetos/Rhetos/blob/master/Source/Rhetos.Utilities/FastReplacer.cs
        /// </remarks>
        static void Main(string[] args)
        {
            // Configure FluentCommandLineParser
            // NOTE: Argument validation is handled in the ConfigSettings class
            var parser = SetupParser();            
            var result = parser.Parse(args);

            // No-op if the help menu is requested. 
            if (result.HelpCalled) { return; }
            if (result.HasErrors) { Console.WriteLine(result.ErrorText); }
            // Quit if parameter validation issues occurred.
            if (parser.Object.ValidationErrors || result.HasErrors) { return; }
           
            // Now that we have configuration, set up object(s) that we need.
            _envManager = new EnvironmentManager(parser.Object);

            // 'Clean' is a special case option. Execute a 'clean' then return
            if (parser.Object.Clean)
            {
                _envManager.RemoveDirectories();
                return;
            }
           
            // Do work           
            PreProcess(parser.Object);
            Process(parser.Object);
            PostProcess(parser.Object);
        }

        private static void PreProcess(ConfigSettings config)
        {
            _envManager.CreateDirectories();
        }

        private static void Process(ConfigSettings config)
        {
            // Generate code
            var _processor = new EntityProcessor();            
            _processor.Process(config.TemplatePath, config.InputPath, _envManager.Directories, config.Verbose);
        }

        private static void PostProcess(ConfigSettings config)
        {
            
        }

        /// <Summary>
        /// This method configures CommandLineParser to handle arguments passed to the program.
        /// </Summary>
        private static FluentCommandLineParser<ConfigSettings> SetupParser()
        {
            var parser = new FluentCommandLineParser<ConfigSettings>();

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
                .As('v', "verbose")
                .SetDefault(false)
                .WithDescription("Determines if verbose output will be printed. Default value: 'false'");

            parser.Setup(a => a.Clean)
                .As('c', "clean")
                .SetDefault(false)
                .WithDescription("Recursively deletes output directories.");

            parser.Setup(a => a.Overwrite)
                .As('w', "overwrite")
                .SetDefault(true)
                .WithDescription("Overwrite any files with the same name as generated output. Default value: true");

            parser.Setup(a => a.TemplatePath)
                .As('t', "template")
                .SetDefault("templates")
                .WithDescription("The location where script templates are stored. Default value: 'templates'");
            
            return parser;
        }
    }
}