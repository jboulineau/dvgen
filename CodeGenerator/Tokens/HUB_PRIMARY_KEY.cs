using System;
using System.Text;
using dvgen.Model;

namespace dvgen.CodeGenerator.Tokens
{
    public class HUB_PRIMARY_KEY : Token
    {
        public HUB_PRIMARY_KEY(string tokenString) : base(tokenString) { }

        public override string GetCode(Entity entity)
        {
            return string.Format("CONSTRAINT [cl_{0}_pk] PRIMARY KEY CLUSTERED ({1})", entity.Name.ToLowerInvariant(), string.Concat('[', entity.Name, "Id]"));
        }
    }
}