using System;
using Konsole;
using System.IO;
using dvgen.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using Newtonsoft.Json.Schema.Generation;

namespace dvgen.Repositories
{
    public class EntityRepository
    {
        private ConfigSettings _config;

        public EntityRepository(ConfigSettings config)
        {
            _config = config;
        }

        /// <Summary>
        /// <c>LoadEntities</c> reads all of the files in the provided path and attempts to deserialize JSON data into the Entity model.
        /// </Summary>     
        public List<Entity> LoadEntities(string path)
        {
            var entities = new List<Entity>();
            var count = 0;

            // Create a json schema for the Entity class
            var generator = new JSchemaGenerator();
            var schema = generator.Generate(typeof(Entity));

            Console.WriteLine("Loading input files ... ");
            var files = Directory.GetFiles(path);

            var bar = new ProgressBar(files.Length);

            foreach (var f in files)
            {
                if (_config.Verbose) { Console.WriteLine(String.Concat("Loading input file : ", f)); }
                
                bar.Refresh(count, Path.GetFileName(f));

                var json = String.Join("", File.ReadAllLines(f));

                if (ValidateJSON(json, schema))
                {
                    var entity = JsonConvert.DeserializeObject<Entity>(json);
                    entities.Add(entity); // TODO: Implement Entity validation (i.e. data types, etc.)
                }
                else
                {
                    if (_config.Verbose) { Console.WriteLine(String.Format("{0} is not a valid dvgen configuration file.", f)); }
                }

                count++;
                bar.Refresh(count, Path.GetFileName(f));
            }

            return entities;
        }

        /// <Summary>
        /// Convenience function to encapsulate logic required to validate that a file contains valid Entity configuration data.
        /// </Summary>
        /// <param name="json">A string of json to be validated.</param>
        /// <param name="schema">A string containing a json schema against which the <paramref name="json" /> parameter is to be validated.
        private bool ValidateJSON(string json, JSchema schema)
        {
            IList<string> errorMessages;
            JObject jo = JObject.Parse(json);
            return jo.IsValid(schema, out errorMessages);
        }
    }
}