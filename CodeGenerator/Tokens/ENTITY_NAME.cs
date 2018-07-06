using System.Collections.Generic;
using dvgen.Model;

namespace dvgen.CodeGenerator.Tokens
{
  public class ENTITY_NAME : Token
  {
    public ENTITY_NAME(string tokenString) : base(tokenString) { }

    public override string GetCode(Entity entity, IDictionary<string,string> args)
    {
      return entity.Name;
    }
  }
}