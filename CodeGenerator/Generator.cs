using System;
using System.IO;
using System.Text;
using dvgen.Model;
using Rhetos.Utilities;
using System.Collections.Generic;

namespace dvgen.CodeGenerator
{
    public class Generator
    {
        ///<Summary> 
        ///The <c>Generate</c> method is responsible for token replacement. Given a template file and an Entity object it translates tokens
        ///it finds while parsing the template file with the correct code.
        ///</Summary>
        ///<param name="entity">The entity model object for which code is to be generated</param>
        ///<param name="verbose"> Determines whether verbose output should be written to the console.</param>
        ///<param name="template">The code template for the type of code object to be generated.</param>
        public Script Generate(Entity entity, Template template, bool verbose)
        {
            if (verbose) { Console.WriteLine(String.Format("Generating {0} script for {1}", template.Name, entity.Name)); }

            var replacer = GetReplacer(template.Body);

            // Attempt all known token replacement operations.
            // TODO: Since not all tokens are found in every template it would be faster to separate by template type.
            // This is, however, much simpler.

            // Base tokens
            replacer.Replace("{%SCHEMA%}", String.Concat('[', entity.Schema.ToLower(), ']'));

            // API tokens
            replacer.Replace("{%API_UDT_NAME%}", String.Concat('[', "udt_", entity.Name, ']'));
            replacer.Replace("{%API_TYPED_COL_LIST%}", GetEntityColumnList(entity, true, false));

            // Hub tokens
            replacer.Replace("{%HUB_UDT_NAME%}", String.Concat("udt_", entity.Name, "_h"));
            replacer.Replace("{%HUB_TABLE_NAME%}", String.Concat(entity.Name, "_h"));
            replacer.Replace("{%HUB_INSERT_PROC_NAME%}", String.Concat("usp_", entity.Name, "_insert_h"));

            return new Script
            {
                OutputDirectory = template.OutputDirectory,
                    Name = String.Concat(entity.Name, "_", template.Name),
                    TemplateName = template.Name,
                    Body = replacer.ToString()
            };
        }

        private string GetEntityColumnList(Entity entity, bool typed, bool removeTrailingComma = true)
        {
            var cols = new List<string>();

            foreach (var c in entity.Columns)
            {
                if (typed)
                {
                    cols.Add(String.Concat(c.Name, " ", c.DataType, ","));
                }
                else
                {
                    cols.Add(String.Concat(c.Name, ","));
                }
            }

            if (removeTrailingComma)
            {
                cols[cols.Count - 1] = cols[cols.Count - 1].Replace(',', ' ');
            }

            return String.Join("", cols.ToArray());
        }

        /// <Summary>
        /// Convenience method to instantiate a FastReplacer option and 'hydrate' it with the lines of the template file.
        /// At the completion of execution, token replacement can begin.
        /// </Summary>
        private FastReplacer GetReplacer(string template)
        {
            var output = new FastReplacer("{%", "%}");

            var lines = template.Split(Environment.NewLine, StringSplitOptions.None);

            foreach (string line in lines)
            {
                output.Append(line);
            }

            return output;
        }
    }
}