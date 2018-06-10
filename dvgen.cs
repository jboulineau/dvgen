using System;
using System.IO;
using dvgen.Model;
using dvgen.Repositories;
using dvgen.CodeGenerator;
using CommandLineParser.Core;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace dvgen
{
    public class Program
    {
        private static FluentCommandLineParser<ConfigSettings> _parser = new FluentCommandLineParser<ConfigSettings>();

        /// <Summary>
        /// dvgen is a code generator for the objects required to implement a database in the Data Vault pattern.
        /// </Summary>
        /// <remarks>
        /// Dependencies -
        /// CommandLineParser : https://github.com/valeravorobjev/CommandLineParser/tree/master/CommandLineParser.Core
        /// Konsole : https://github.com/goblinfactory/konsole
        /// JSON.NET Schema : https://www.newtonsoft.com/jsonschema
        /// JSON.NET : https://www.newtonsoft.com/json
        /// </remarks>
        static void Main(string[] args)
        {
            JSchemaGenerator generator = new JSchemaGenerator();

            JSchema schema = generator.Generate(typeof(Entity));
            
            SetupArgs();            
            // NOTE: Argument validation is handled in the ConfigSettings class
            var result = _parser.Parse(args); 

            // No-op if the help menu is requested. 
            if (result.HelpCalled) { return; }
            if (result.HasErrors) { Console.WriteLine(result.ErrorText); } 
            // Quit if parameter validation issues occurred.
            if (_parser.Object.ValidationErrors || result.HasErrors) { return; } 

            // Now that we have configuration, set up some objects that we need.
            var _envManager = new EnvironmentManager(_parser.Object);
            var _entityRepo = new EntityRepository(_parser.Object);            
            var _processor = new EntityProcessor();

            // 'Clean' is a special case option. Execute a 'clean' then return
            if (_parser.Object.Clean)
            {
                _envManager.RemoveDirectories();
                return;
            }

            // Setup the environment
            _envManager.CreateDirectories();

            // Read JSON input files and create objects
            List<Entity> entities = _entityRepo.LoadEntities(_parser.Object.InputPath); 
            // Generate code
            _processor.Process(entities,_parser.Object);
        }

        private static void LoadEntities(string path)
        {
            
        }

        /// <Summary>
        /// This method configures CommandLineParser to handle arguments passed to the program.
        /// </Summary>
        private static void SetupArgs()
        {
            // Instructs CommandLineParser to display formatted help text for every available argument.
            _parser.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text));

            _parser.Setup(a => a.InputPath)
                .As('i', "input")
                .SetDefault("input")
                .WithDescription("The path to the directory where JSON entity configuration files are located. Default value: 'input'");

            _parser.Setup(a => a.OutputPath)
                .As('o', "output")
                .SetDefault("output")
                .WithDescription("The path to the directory where generated files will be written. Default value: 'output'");

            _parser.Setup(a => a.Verbose)
                .As('v',"verbose")
                .SetDefault(false)
                .WithDescription("Determines if verbose output will be printed. Default value: 'false'");

            _parser.Setup(a => a.Clean)
                .As('c',"clean")
                .SetDefault(false)
                .WithDescription("Recursively deletes output directories.") ;

            _parser.Setup(a => a.Overwrite)
                .As('w',"overwrite")
                .SetDefault(true)
                .WithDescription("Overwrite any files with the same name as generated output.");
            
            _parser.Setup(a => a.TemplatePath)
                .As('t',"template")
                .SetDefault("templates")
                .WithDescription("The location where script templates are stored. Default value: 'templates'");
        }
    }
}