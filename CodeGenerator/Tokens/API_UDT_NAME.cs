using System;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
  public class API_UDT_NAME : Token
  {
    public API_UDT_NAME(string tokenString) : base(tokenString) {}

    public override string GetCode(Entity entity, IDictionary<string,string> args)
    {
        return String.Concat('[', "udt_", entity.Name, ']');
    }
  }
}