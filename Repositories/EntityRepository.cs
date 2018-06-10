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

            // Read each file in the provided path
            foreach (var f in files)
            {
                bar.Refresh(count, f);

                var json = String.Join("", File.ReadAllLines(f));

                if(ValidateJSON(json,schema))
                {
                    var entity = JsonConvert.DeserializeObject<Entity>(json);
                    entities.Add(entity);
                }
                else
                {
                    if (_config.Verbose) { Console.WriteLine(String.Format("{0} is not a dvgen configuration file.", f)); }
                }
                
                // TODO: Implement Entity validation (i.e. data types, etc.)
                
                count++;
                bar.Refresh(count, f);
            }

            return entities;
        }

        private bool ValidateJSON(string json, JSchema schema)
        {
            IList<string> errorMessages;
            JObject jo = JObject.Parse(json);
            return jo.IsValid(schema, out errorMessages);
        }
    }
}