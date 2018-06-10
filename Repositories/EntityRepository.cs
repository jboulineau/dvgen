using System;
using System.IO;
using dvgen.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using Konsole;

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

            Console.WriteLine("Loading input files ... ");

            var files = Directory.GetFiles(path);            
            var bar = new ProgressBar(files.Length);
            
            // Read each file in the provided path
            foreach(var f in files)
            {
                bar.Refresh(count, f);

                try 
                {
                    var json = String.Join("", File.ReadAllLines(f));
                    var entity = JsonConvert.DeserializeObject<Entity>(json);                    
                    entities.Add(entity);
                }
                catch
                {
                    if (_config.Verbose) { Console.WriteLine(String.Format("{0} is not a dvgen configuration file.",f)); }
                }

                count++;
                bar.Refresh(count, f);
            }

            return entities;
        }
    }
}