using System;
using System.IO;
using dvgen.Model;
using Rhetos.Utilities;

namespace dvgen.CodeGenerator
{
    public class UDTGenerator : IGenerator
    {
        public Script GenerateScript(Entity entity, ConfigSettings config)
        {
            var _text = "foo";

            var _template = String.Join("", File.ReadAllLines(String.Concat(config.TemplatePath, "/UDT")));
            Console.WriteLine(_template);
            
            return new Script()
            {
                Type = ScriptTypeEnum.UDT,
                Body = _text
            };
        }
    }
}