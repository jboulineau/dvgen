using System;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
    public class HUB_METADATA_COLUMNS : Token
    {
        public HUB_METADATA_COLUMNS(string tokenString) : base(tokenString) { }

        public override string GetCode(Entity entity, IDictionary<string,string> args)
        {
            var cols = new List<Column>
                {
                    new Column { Name = "_SOURCE", DataType = "VARCHAR(100)", PrimaryKey = false },
                    new Column { Name = "_LOAD_DATE", DataType = "DATETIME2", PrimaryKey = false },
                };

            return base.GetEntityColumnList(cols, true, true);
        }
    }
}