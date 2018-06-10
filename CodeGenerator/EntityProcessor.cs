using System;
using Konsole;
using System.IO;
using dvgen.Model;
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
        public void Process(ICollection<Entity> entities, ConfigSettings config)
        {
            var _udtGen = new UDTGenerator();

            // TODO: To be safe, make sure all directories have been prepared based on config.
            Console.WriteLine("Generating code ...");
            var bar = new ProgressBarSlim(entities.Count);

            var count = 0;

            foreach (var entity in entities)
            {
                bar.Refresh(count, entity.Name);

                if (config.Verbose) { Console.WriteLine(String.Concat("Generating UDT script for ", entity.Name)); }
                var udt = _udtGen.GenerateScript(entity, config);

                count++;
                bar.Refresh(count, entity.Name);
            }
        }

        private void WriteScript(Script script)
        {

        }
    }
}