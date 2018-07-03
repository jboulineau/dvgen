using System;
using System.IO;
using dvgen.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using Newtonsoft.Json.Schema.Generation;

namespace dvgen.Repositories
{
    public class TemplateRepository
    {
        public IList<Template> GetTemplates (string path, bool verbose, Dictionary<string,string> templateConfig)
        {
            var output = new List<Template> ();

            // Defensive. This should never happen if validation is happening properly in ConfigSettings
            if (!Directory.Exists (path))
            {
                throw new IOException (String.Concat ("The specified template path does not exist: ", path));
            }

            foreach (var file in Directory.EnumerateFiles (path))
            {
                output.Add (new Template ()
                {
                    // TODO: Depending upon the filename seems dangerous
                    Name = Path.GetFileNameWithoutExtension (file),
                    Body = String.Join ("", File.ReadAllLines (file)),
                    OutputDirectory = templateConfig.GetValueOrDefault(Path.GetFileNameWithoutExtension(file))
                });
            }
            return output;
        }

        public IList<TemplateSetting> GetTemplateSettings (string path, bool verbose)
        {
            var generator = new JSchemaGenerator ();

            if (!File.Exists (path))
            {
                throw new IOException (String.Concat ("The specified template settings path does not exist: ", path));
            }

            var schema = generator.Generate (typeof (Templates));
            var json = String.Join ("", File.ReadAllLines (path));
            var errors = ValidateJSON (json, schema);

            if (errors.Count != 0)
            {
                Console.WriteLine (String.Concat ("Ivalid dvgen configuration file : ", path));
                foreach (var e in errors)
                {
                    Console.WriteLine (e);
                }

                // TODO: This should be handled by caller
                throw new Exception (String.Concat ("Ivalid template-settings.json configuration file : ", path));
            }
            else
            {
                var templates = JsonConvert.DeserializeObject<Templates> (json); // TODO: Can you deserialize directly to a Collection?
                return templates.Settings;
            }
        }

        private class Templates
        {
            public IList<TemplateSetting> Settings { get; set; }
        }

        /// <Summary>
        /// Convenience function to encapsulate logic required to validate that a file contains valid Entity configuration data.
        /// </Summary>
        /// <param name="json">A string of json to be validated.</param>
        /// <param name="schema">A string containing a json schema against which the <paramref name="json" /> parameter is to be validated.
        private IList<string> ValidateJSON (string json, JSchema schema)
        {
            //TODO: This is duplicated in EntityRepository.
            IList<string> errorMessages;

            try
            {
                JObject jo = JObject.Parse (json);
                jo.IsValid (schema, out errorMessages);
                return errorMessages;
            }
            catch (Exception e) // the file isn't something that can be interpreted as json
            {
                return new List<string> { e.Message };
            }
        }
    }
}