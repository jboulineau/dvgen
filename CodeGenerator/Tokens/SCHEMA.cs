using System;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
  public class SCHEMA : Token
  {
    public SCHEMA(string tokenString) : base(tokenString) { }

    public override string GetCode(Entity entity, IDictionary<string,string> args)
    {
      return String.Concat('[', entity.Schema, ']');
    }
  }
}