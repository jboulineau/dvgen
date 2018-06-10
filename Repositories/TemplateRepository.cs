using System;
using System.IO;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.Repositories
{
    public class TemplateRepository
    {
        public IList<Template> GetTemplates(string path, bool verbose)
        {
            var output = new List<Template>();

            // Defensive. This should never happen if validation is happening properly in ConfigSettings
            if (!Directory.Exists(path))
            {
                throw new IOException(String.Concat("The specified template path does not exist: ", path));
            }

            foreach (var file in Directory.EnumerateFiles(path))
            {
                output.Add(new Template()
                {
                    // TODO: Depending upon the filename seems dangerous
                    Name = Path.GetFileNameWithoutExtension(file),
                    Body = String.Join("", File.ReadAllLines(file))
                });
            }
            return output;
        }
    }
}