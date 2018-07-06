using System;
using dvgen.Model;
using System.Collections.Generic;

namespace dvgen.CodeGenerator.Tokens
{
  ///<Summary>
  ///Each implementation of the Token class contains both the string representing a token as found in a template as well as the logic required to
  ///translate the token into generated code.
  ///</Summary>
  public abstract class Token
  {
    public string TokenString { get; protected set; }

    ///<Summary>
    ///Having the string set here rather than hardcoded makes it potentially more configurable down the road. For now, using <c>TokenFactory</c> allows 
    ///for at least centralizing the hard coding that needs to occur.
    ///</Summary>
    public Token(string tokenString)
    {
      TokenString = tokenString;
    }

    public abstract string GetCode(Entity entity,IDictionary<string,string> args);

    protected string GetEntityColumnList(IList<Column> cols, bool typed, bool removeTrailingComma = true)
    {
      var output = new List<string>();

      foreach (var c in cols)
      {
        if (typed)
        {
          output.Add(String.Concat('[', c.Name, ']', " ", c.DataType, ","));
        }
        else
        {
          output.Add(String.Concat('[', c.Name, ']', ","));
        }
      }

      if (removeTrailingComma)
      {
        output[output.Count - 1] = output[output.Count - 1].Replace(',', ' ');
      }

      return String.Join("", output.ToArray());
    }
  }
}