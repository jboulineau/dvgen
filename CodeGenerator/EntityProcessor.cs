using System;
using Konsole;
using System.IO;
using dvgen.Model;
using dvgen.Repositories;
using System.Collections.Generic;

namespace dvgen.CodeGenerator
{
    public class EntityProcessor
    {
        /// <Summary>
        /// <c>EntityProcessor</c> is responsible for control flow of creating all of the generated output.
        /// </Summary>
        /// <param name="entities">A collection of <c>Entity</c> objects for which code is to be generated.</param>
        /// <param name="config">The dvgen configuration object.</param>
        public void Process(string templatePath, string inputPath, Dictionary<string, string> directories, bool verbose)
        {
            var generator = new Generator();
            var eRepo = new EntityRepository();
            var tRepo = new TemplateRepository();

            var entities = eRepo.GetEntities(inputPath, verbose);
            var templates = tRepo.GetTemplates(templatePath, verbose);

            Console.WriteLine("Generating code ...");
            var bar = new ProgressBarSlim(entities.Count);

            var count = 0;

            foreach (var entity in entities)
            {
                bar.Refresh(count, entity.Name);

                foreach (var template in templates)
                {
                    var script = generator.Generate(entity, template, verbose);
                    WriteScript(script, directories);
                    // if (verbose) { Console.WriteLine(script.Body); }
                }

                count++;
                bar.Refresh(count, entity.Name);
            }
        }

        private void WriteScript(Script script, Dictionary<string, string> directories)
        {
            var path = String.Concat(GetOutputDir(script.Type, directories), "/", script.Name, ".sql");
            File.WriteAllText(path, script.Body);
        }

        private String GetOutputDir(ScriptType type, Dictionary<string, string> directories)
        {
            var output = "";

            // TODO: Fix this awful mess
            if (type == ScriptType.api_udt || type == ScriptType.hub_udt || type == ScriptType.link_udt) 
                { output = directories.GetValueOrDefault("udt"); }
            else if (type == ScriptType.hub_table || type == ScriptType.link_table || type == ScriptType.satellite_table) 
                { output = directories.GetValueOrDefault("table"); }
            else if (type == ScriptType.hub_insert || type == ScriptType.hub_delete || type == ScriptType.link_delete || type == ScriptType.link_insert 
                    || type == ScriptType.satellite_delete || type == ScriptType.satellite_insert) 
                { output = directories.GetValueOrDefault("procedure"); }

            return output;
        }
    }
}