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
        public void Process(string templatePath, string inputPath, Dictionary<string, string> directories, bool verbose, bool stopOnErrors)
        {
            var generator = new Generator();
            var eRepo = new EntityRepository();
            var tRepo = new TemplateRepository();

            var entities = eRepo.GetEntities(inputPath, verbose, stopOnErrors);
            var templates = tRepo.GetTemplates(templatePath, verbose, directories);

            Console.WriteLine("Generating code ...");

            var count = 0;
            var bar = new ProgressBarSlim(entities.Count);

            foreach (var entity in entities)
            {
                bar.Refresh(count, entity.Name);                

                foreach (var template in templates)
                {
                    var script = generator.Generate(entity, template, verbose);
                    WriteScript(script, directories, verbose);
                }

                count++;
                bar.Refresh(count, entity.Name);
            }
        }

        ///<Summary>
        ///Write the generated script to a file.
        ///</Summary>
        private void WriteScript(Script script, Dictionary<string, string> directories, bool verbose)
        {
            var path = String.Concat(directories.GetValueOrDefault(script.TemplateName), "\\", script.Name, ".sql");
            if (verbose) { Console.WriteLine(String.Concat("Writing script: path")); }
            File.WriteAllText(path, script.Body); //TODO: This should probably have error handling
        }
    }
}