using System;
using System.IO;

namespace dvgen
{
    /// <Summary>
    /// Used by CommandLineParser to establish definitions of the allowed program arguments.
    /// </Summary>
    public class ConfigSettings
    {

        public bool Overwrite { get; set; }
        public bool Clean { get; set; }
        public bool Verbose { get; set; }
        public bool StopOnValidationError { get; set; }
        
        public string TemplatePath
        {
            get { return _templatePath; }
            set { _templatePath = GetValidatedPath(value); }
        }
        public string InputPath
        {
            get { return _inputPath; }
            set { _inputPath = GetValidatedPath(value); }
        }

        public string OutputPath
        {
            get { return _outputPath; }
            set { _outputPath = GetValidatedPath(value); }
        }
        
        public bool ValidationErrors { get; private set; }
        private string _inputPath;
        private string _outputPath;
        private string _templatePath;

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
    }
}