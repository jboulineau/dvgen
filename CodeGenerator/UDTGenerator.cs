using System;
using System.IO;
using dvgen.Model;
using Rhetos.Utilities;

namespace dvgen.CodeGenerator
{
    public class UDTGenerator : Generator 
    {
        public UDTGenerator(ConfigSettings config) : base(config, ScriptTypeEnum.UDT) {}
        
        public Script GenerateScript(Entity entity)
        {
            var udtName = String.Concat("udt_",entity.Name);            
            _replacer.Replace("{%UDT_NAME%}",udtName);

            _replacer.Replace("{%TYPED_COL_LIST%}",GetTypedColumnList(entity));
            
            Console.WriteLine(_replacer.ToString());

            // {%UDT_NAME%}
            // {%TYPED_COL_LIST%}

            return new Script()
            {
                Type = ScriptTypeEnum.UDT,
                Body = _replacer.ToString()
            };
        }
    }
}