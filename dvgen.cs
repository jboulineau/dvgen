using System;
using System.IO;
using dvgen.Repositories;
using CommandLineParser.Core;

namespace dvgen
{
    /// <Summary>
    /// Used by CommandLineParser to establish definitions of the allowed program arguments.
    /// </Summary>
    public class CommandLineArguments
    {
        private string _inputPath;
        public string InputPath 
        { 
            get { return _inputPath; }
            set { _inputPath = GetValidatedPath(value); } 
        }

        private string _outputPath;
        public string OutputPath 
        { 
            get { return _outputPath; }
            set { _outputPath = GetValidatedPath(value); } 
        }

        public bool ValidationErrors { get; private set; }
        
        #region Methods

        /// <Summary>
        /// A method to validate filesystem paths passed as arguments.
        /// </Summary>
        private string GetValidatedPath(string path)
        {
            var _absPath = Path.GetFullPath(path);

            if (!Directory.Exists(_absPath))
            {
                Console.WriteLine(String.Format("The path does not exist: {0}", _absPath));
                ValidationErrors = true;
                return null;
            }
            return _absPath;
        }
        #endregion
    }

    public class Program
    {
        private static FluentCommandLineParser<CommandLineArguments> parser = new FluentCommandLineParser<CommandLineArguments>();

        /// <Summary>
        /// dvgen is a code generator for the objects required to implement a database in the Data Vault pattern.
        /// </Summary>
        /// <remarks>
        /// Documentation for CommandLineParser found here: https://github.com/valeravorobjev/CommandLineParser/tree/master/CommandLineParser.Core
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
            EntityRepository er = new EntityRepository();
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
        }
    }
}