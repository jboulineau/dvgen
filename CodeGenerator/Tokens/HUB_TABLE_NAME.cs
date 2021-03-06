using System;
using System.Collections.Generic;
using dvgen.Model;

namespace dvgen.CodeGenerator.Tokens
{
  public class HUB_TABLE_NAME : Token
  {
    public HUB_TABLE_NAME(string tokenString) : base(tokenString) { }

    public override string GetCode(Entity entity, IDictionary<string,string> args)
    {
      return String.Concat('[', entity.Name, "_h", ']');
    }
  }
}