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
        /// <Summary>
        /// <c>LoadEntities</c> reads all of the files in the provided path and attempts to deserialize JSON data into the Entity model.
        /// </Summary>     
        public List<Entity> GetEntities(string path, bool verbose)
        {
            var count = 0;
            var entities = new List<Entity>();

            // Create a json schema for the Entity class
            var generator = new JSchemaGenerator();
            var schema = generator.Generate(typeof(Entity));

            Console.WriteLine(String.Concat("Loading input files from ", path, " ..."));
            var files = Directory.GetFiles(path);

            var bar = new ProgressBar(files.Length);

            foreach (var f in files)
            {
                if (verbose) { Console.WriteLine(String.Concat("Loading input file : ", f)); }
                
                bar.Refresh(count, Path.GetFileName(f));

                var json = String.Join("", File.ReadAllLines(f));

                if (ValidateJSON(json, schema))
                {
                    var entity = JsonConvert.DeserializeObject<Entity>(json);
                    entities.Add(entity); // TODO: Implement Entity validation (i.e. data types, etc.)
                }
                else
                {
                    if (verbose) { Console.WriteLine(String.Concat("Ivalid dvgen configuration file : ", f)); }
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
            try
            {
                JObject jo = JObject.Parse(json);
                return jo.IsValid(schema, out errorMessages);
            }
            catch // the file isn't something that can be interpreted as json
            {
                return false;
            }
        }
    }
}