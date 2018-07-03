using dvgen.Model;

namespace dvgen.CodeGenerator.Tokens
{
  public class ENTITY_NAME : Token
  {
    public ENTITY_NAME(string tokenString) : base(tokenString) { }

    public override string GetCode(Entity entity)
    {
      return entity.Name;
    }
  }
}