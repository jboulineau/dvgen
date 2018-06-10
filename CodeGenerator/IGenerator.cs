using dvgen.Model;

namespace dvgen.CodeGenerator
{
    public interface IGenerator
    {
         Script GenerateScript(Entity entity, ConfigSettings config);
    }
}