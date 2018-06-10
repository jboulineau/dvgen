using System;
using System.IO;
using System.Collections.Generic;

namespace dvgen
{
    public class EnvironmentManager
    {

        public Dictionary<string, string> Directories {get; private set;}
        
        //private Dictionary<string, string> Directories;

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
            Directories = new Dictionary<string, string> {
                { "udt",Path.GetFullPath(String.Concat(_config.OutputPath, "/UDT")) },
                { "table", Path.GetFullPath(String.Concat(_config.OutputPath, "/Tables")) },
                { "procedure", Path.GetFullPath(String.Concat(_config.OutputPath, "/Procedures")) },
                { "hub_table", Path.GetFullPath(String.Concat(_config.OutputPath, "/Tables", "/Hubs")) },
                { "link_table", Path.GetFullPath(String.Concat(_config.OutputPath, "/Tables", "/Links")) },
                { "satellite_table", Path.GetFullPath(String.Concat(_config.OutputPath, "/Tables", "/Satellites")) }
            };
        }

        /// <Summary>
        /// Creates directories required for storing generated code.
        /// </Summary>
        public void CreateDirectories()
        {
            foreach (var dir in Directories)
            {
                if (!Directory.Exists(dir.Value))
                {
                    if (_config.Verbose) { Console.WriteLine(String.Concat("Creating Directory : ", dir)); }
                    Directory.CreateDirectory(dir.Value);
                }
                else
                {
                    if (_config.Verbose) { Console.WriteLine("Directory {0} exists, skipping. ", dir); }
                }
            }
        }

        public void RemoveDirectories()
        {
            foreach (var dir in Directories)
            {
                if (Directory.Exists(dir.Value))
                {
                    // Remove subdirectories
                    // TODO: Make this recursive so it can handle hierarchies more than one-deep
                    // TODO: Fix minor bug that causes Table subdirectories to be iterated over twice
                    foreach (var subdir in Directory.GetDirectories(dir.Value))
                    {
                        DeleteDirectory(subdir);
                    }

                    DeleteDirectory(dir.Value);
                }
                else
                {
                    if (_config.Verbose) { Console.WriteLine("Directory {0} does not exist, skipping. ", dir.Value); }
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