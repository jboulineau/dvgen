using System;
using System.IO;
using dvgen.Model;
using Newtonsoft.Json;

namespace dvgen.Repositories
{
    public class EntityRepository
    {        
        public void LoadEntities(string path)
        {
            foreach(string f in Directory.EnumerateFiles(path))
            {
                var json = String.Join("",File.ReadAllLines(f));
                var table = JsonConvert.DeserializeObject<Table>(json);
                Console.WriteLine(table.Name);
            }
        }
    }
}