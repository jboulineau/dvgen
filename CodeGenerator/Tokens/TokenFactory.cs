using System;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
    public static class TokenFactory
    {
        public static List<Token> GetTokens()
        {
            var output = new List<Token>();

            // Base tokens
            output.Add(new SCHEMA("{%SCHEMA%}"));
            output.Add(new ENTITY_NAME("{%ENTITY_NAME%}"));
            output.Add(new METADATA_COLUMNS("{%METADATA_COLUMNS%}"));

            // API Tokens
            output.Add(new API_UDT_NAME("{%API_UDT_NAME%}"));

            // Hub Tokens
            output.Add(new HUB_TABLE_NAME("{%HUB_TABLE_NAME%}"));
            output.Add(new HUB_TABLE_TYPED_COLUMN_LIST("{%HUB_TABLE_TYPED_COLUMN_LIST%}"));
                        
            return output;
        }
    }
}

            // // API tokens
            // replacer.Replace("{%API_UDT_NAME%}", String.Concat('[', "udt_", entity.Name, ']'));
            // replacer.Replace("{%API_TYPED_COL_LIST%}", GetEntityColumnList(entity.Columns, true, true));

            // // Hub tokens
            // replacer.Replace("{%HUB_UDT_NAME%}", String.Concat("udt_", entity.Name, "_h"));
            // replacer.Replace("{%HUB_TABLE_NAME%}", String.Concat('[', entity.Name, "_h", ']'));
            // replacer.Replace("{%HUB_INSERT_PROC_NAME%}", String.Concat("usp_", entity.Name, "_insert_h"));
            // replacer.Replace("{%HUB_TABLE_TYPED_COLUMN_LIST%}", GetHubColumnList(entity.Columns,true,false));