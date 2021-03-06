using System.Text;
using System.Collections.Generic;

namespace dvgen.Model
{
    public class Entity
    {
      public string Name;
      public string Schema;
      public string Type;
      public IList<Column> Columns;
      public string[] Links;

      // Mostly just for testing
      // public override string ToString()
      // {
      //   StringBuilder sb = new StringBuilder();
      //   sb.AppendLine(string.Concat("Name : ", Name));
      //   sb.AppendLine(string.Concat("Type : ", Type));

      //   sb.AppendLine("Columns :");        
      //   foreach(Column c in Columns)
      //   {
      //     sb.AppendLine(string.Concat("    Name : ", c.Name ));
      //     sb.AppendLine(string.Concat("    DataType : ", c.DataType));
      //     sb.AppendLine(string.Concat("    PrimaryKey : ", c.PrimaryKey));
      //   }

      //   sb.AppendLine("Links :");
      //   for(int i = 0; i < Links.Length; i++)
      //   {
      //     sb.AppendLine(string.Concat("    Name : ", Links[i] ));
      //   }

      //   return sb.ToString();
      // }
    }
}