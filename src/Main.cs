namespace MyApp.Lang;

public class Program
{
 private static Lexer lexer = new Lexer();

  public static void Main()
  {
    string source_code = File.ReadAllText($@"{Globals.DIRECTORY}resources\source.txt");
    foreach (Token token in lexer.Tokenize(source_code))
    {
      Console.WriteLine(token);
    }
  }
}