using System;
using System.IO;
using System.Text;
using dvgen.Model;
using Rhetos.Utilities;

namespace dvgen.CodeGenerator
{
    public abstract class Generator
    {
        protected FastReplacer _replacer;
        protected ConfigSettings _config;
        protected string _template;

        protected Generator(ConfigSettings config, ScriptTypeEnum type)
        {
            _config = config;
            SetTemplate(type);
            CreateReplacer();
        }

        protected string GetTypedColumnList(Entity entity)
        {
            var sb = new StringBuilder();

            foreach (var c in entity.Columns)
            {
                sb.AppendLine(String.Concat(c.Name, " ", c.DataType, ","));
            }

            return sb.ToString();
        }

        protected string GetUntypedColumnList(Entity entity)
        {
            var sb = new StringBuilder();

            foreach (var c in entity.Columns)
            {
                sb.AppendLine(String.Concat(c.Name, " ", c.DataType, ","));
            }

            return sb.ToString();
        }

        private void SetTemplate(ScriptTypeEnum type)
        {
            if (type == ScriptTypeEnum.UDT)
            {
                _template = GetTemplate("udt");
            }
            else if (type == ScriptTypeEnum.Hub)
            {
                _template = GetTemplate("hub");
            }
        }

        private string GetTemplate(string name)
        {
            var path = Path.GetFullPath(String.Concat(_config.TemplatePath, "/", name));

            if (File.Exists(path))
            {
                if (_config.Verbose) { Console.WriteLine(String.Concat("Loading template file : ", path)); }
                return String.Join("", File.ReadAllLines(path));
            }
            else
            {
                throw new Exception(String.Format("Template expected at {0} was not found", path));
            }
        }

        /// <Summary>
        /// Convenience method to instantiate a FastReplacer option and 'hydrate' it with the lines of the template file.
        /// At the completion of execution, token replacement can begin.
        /// </Summary>
        private void CreateReplacer()
        {
            _replacer = new FastReplacer("{%", "%}");
            Console.WriteLine(_template);

            var lines = _template.Split(Environment.NewLine, StringSplitOptions.None);

            foreach (string line in lines)
            {
                _replacer.Append(line);
            }
        }
    }
}