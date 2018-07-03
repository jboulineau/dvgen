using System;
using System.Linq;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
  public class HUB_TABLE_TYPED_COLUMN_LIST : Token
  {
    public HUB_TABLE_TYPED_COLUMN_LIST(string tokenString) : base(tokenString) { }

    public override string GetCode(Entity entity)
    {
      return GetHubColumnList(entity.Columns,true,false);
    }

    ///<Summary>
    ///Calls GetEntityColumnList after filtering the columns to only what is part of a hub table.
    ///</Summary>
    private string GetHubColumnList(IList<Column> cols, bool typed, bool removeTrailingComma = true)
    {
      List<Column> hub_cols = cols.Where(c => c.PrimaryKey).ToList();
      return base.GetEntityColumnList(hub_cols, typed, removeTrailingComma);
    }
  }
}