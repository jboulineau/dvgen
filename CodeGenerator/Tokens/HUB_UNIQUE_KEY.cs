using System.Text;
using System.Linq;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
    public class HUB_UNIQUE_KEY : Token
    {
        public HUB_UNIQUE_KEY(string tokenString) : base(tokenString) { }

        public override string GetCode(Entity entity, IDictionary<string,string> args)
        {
            var cols = entity.Columns
                        .Where(c => c.PrimaryKey)
                        .Select(c => c.Name)
                        .ToArray();
            
            var joinedCols = string.Join(",",cols);

            return string.Format("INDEX [ix_u_{0}] UNIQUE NONCLUSTERED ON ({1})",entity.Name.ToLowerInvariant(),joinedCols);
        }
    }
}