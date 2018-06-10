using System;
using System.IO;
using System.Collections.Generic;

namespace dvgen
{
    public class EnvironmentManager
    {
        private IList<String> _directories;
        private ConfigSettings _config;

        /// <Summary>
        /// <c>EnvironmentManager</c> is responsible for various setup tasks related to the environment required by dvgen.
        /// </Summary>
        /// <param name="config">The dvgen application configuration object.</param>
        public EnvironmentManager(ConfigSettings config)
        {
            _config = config;

            // TODO: Should this be more configurable?
            // Set up the list of directories required by the code generator.
            _directories = new List<String> {
                Path.GetFullPath(String.Concat(_config.OutputPath, "/UDT")),
                Path.GetFullPath(String.Concat(_config.OutputPath, "/Procedures")),
                Path.GetFullPath(String.Concat(_config.OutputPath, "/Tables")),
                Path.GetFullPath(String.Concat(String.Concat(_config.OutputPath, "/Tables"), "/Hubs")),
                Path.GetFullPath(String.Concat(String.Concat(_config.OutputPath, "/Tables"), "/Links")),
                Path.GetFullPath(String.Concat(String.Concat(_config.OutputPath, "/Tables"), "/Satellites"))
            };
        }

        /// <Summary>
        /// Creates directories required for storing generated code.
        /// </Summary>
        public void CreateDirectories()
        {
            foreach (var dir in _directories)
            {
                if (!Directory.Exists(dir))
                {
                    if (_config.Verbose) { Console.WriteLine(String.Concat("Creating Directory : ", dir)); }
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    if (_config.Verbose) { Console.WriteLine("Directory {0} exists, skipping. ", dir); }
                }
            }
        }

        public void RemoveDirectories()
        {
            foreach (var dir in _directories)
            {
                if (Directory.Exists(dir))
                {
                    // Remove subdirectories
                    // TODO: Make this recursive so it can handle hierarchies more than one-deep
                    // TODO: Fix minor bug that causes Table subdirectories to be iterated over twice
                    foreach (var subdir in Directory.GetDirectories(dir))
                    {
                        DeleteDirectory(subdir);   
                    }

                    DeleteDirectory(dir);
                }
                else
                {
                    if (_config.Verbose) { Console.WriteLine("Directory {0} does not exist, skipping. ", dir); }
                }
            }
        }

        private void DeleteDirectory(string path)
        {
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                if (_config.Verbose) { Console.WriteLine(String.Concat("Deleting file : ", file)); }
                File.Delete(file);
            }

            if (_config.Verbose) { Console.WriteLine(String.Concat("Deleting directory : ", path)); }            
            Directory.Delete(path);
        }
    }
}