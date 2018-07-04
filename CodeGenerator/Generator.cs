using System;
using System.IO;
using System.Text;
using System.Linq;
using dvgen.Model;
using Rhetos.Utilities;
using System.Collections.Generic;
using dvgen.CodeGenerator.Tokens;
using PoorMansTSqlFormatterLib;

namespace dvgen.CodeGenerator
{
  public class Generator
  {
    ///<Summary> 
    ///The <c>Generate</c> method is responsible for token replacement. Given a template file and an Entity object it translates tokens
    ///it finds while parsing the template file with the correct code.
    ///</Summary>
    ///<param name="entity">The entity model object for which code is to be generated</param>
    ///<param name="verbose"> Determines whether verbose output should be written to the console.</param>
    ///<param name="template">The code template for th type of code object to be generated.</param>
    public Script Generate(Entity entity, Template template, bool verbose)
    {
      if (verbose) { Console.WriteLine(String.Format("Generating {0} script for {1}", template.Name, entity.Name)); }

      // Create a new FastReplacer and hydrate it with the body of the template
      var replacer = new FastReplacer("{%", "%}");
      var lines = template.Body.Split(Environment.NewLine, StringSplitOptions.None);
      foreach (string line in lines)
      {
        replacer.Append(line);
      }

      // Attempt all known token replacement operations.
      // Since not all tokens are found in every template it would be faster to separate by template type. 
      // This is, however, much simpler.
      var tokens = TokenFactory.GetTokens();

      foreach (var token in tokens)
      {
        replacer.Replace(token.TokenString, token.GetCode(entity));
      }

      var formatter = new SqlFormattingManager();
      var scriptBody = formatter.Format(replacer.ToString());

      return new Script
      {
        OutputDirectory = template.OutputDirectory,
        Name = String.Concat(entity.Name, "_", template.Name),
        TemplateName = template.Name,
        Body = scriptBody //replacer.ToString()
      };
    }
  }
}