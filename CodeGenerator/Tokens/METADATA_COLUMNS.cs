using System;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
    public class METADATA_COLUMNS : Token
    {
        public METADATA_COLUMNS(string tokenString) : base(tokenString) { }

        public override string GetCode(Entity entity)
        {
            var cols = new List<Column>
                {
                    new Column { Name = "_SOURCE", DataType = "VARCHAR(100)", PrimaryKey = false },
                    new Column { Name = "_LOAD_DATE", DataType = "DATETIME2", PrimaryKey = false },
                    new Column { Name = "_START_DATE", DataType = "DATETIME2", PrimaryKey = false },
                    new Column { Name = "_END_DATE", DataType = "DATETIME2", PrimaryKey = false }
                };

            return base.GetEntityColumnList(cols, true, true);
        }
    }
}