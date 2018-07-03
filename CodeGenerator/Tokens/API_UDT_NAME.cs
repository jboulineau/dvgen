using System;
using dvgen.Model;

namespace dvgen.CodeGenerator.Tokens
{
  public class API_UDT_NAME : Token
  {
    public API_UDT_NAME(string tokenString) : base(tokenString) {}

    public override string GetCode(Entity entity)
    {
        return String.Concat('[', "udt_", entity.Name, ']');
    }
  }
}