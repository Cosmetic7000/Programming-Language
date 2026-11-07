public enum TokenType
{
  Number,
  Identifier,
  Equals,
  OpenBracket,
  CloseBracket,
  BinaryOperator,
  Comparator,
  Var
}

public struct Token
{
  public string Value;
  public TokenType Type;

  public Token(string value, TokenType type)
  {
    this.Value = value;
    this.Type = type;
  }
}


public class Lexer
{
  public Dictionary<string, TokenType> Keywords = [];

  public List<Token> Tokenize(string source_code)
  {
    List<Token> tokens = [];
    char[] src = source_code.ToCharArray();

    // Build each token until end of file
    int i = 0;
    while (i < source_code.Length)
    {
      TokenType?token_type = GetTokenType(src[i]);

      if (token_type != null)
      {
        tokens.Add(new Token(src[i].ToString(), (TokenType)token_type));
        i++;
      }
      else
      {
        // Handle multi-character tokens

        // Build number token
        if (char.IsDigit(src[i]))
        {
          string num = "";
          while (i < source_code.Length && char.IsDigit(src[i])) {
            num += src[i];
            i++;
          }
          
          tokens.Add(new Token(num, TokenType.Number));
        }
        // Build indentifier token
        else if (char.IsLetter(src[i]))
        {
          string ident = "";
          while (i < source_code.Length && (char.IsLetter(src[i]) || char.IsDigit(src[i]))) {
            ident += src[i];
            i++;
          }

          // Check for reserved keywords
          TokenType reserved;
          if (!Keywords.TryGetValue(ident, out reserved))
          {
            tokens.Add(new Token(ident, TokenType.Identifier));
          }
          else
          {
            tokens.Add(new Token(ident, (TokenType)reserved));
          }
        }
        else if (IsSkippable(src[i])) {i++;}
        else
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine("Error: Unrecognised character found in code: ", src[0]);
          Console.ResetColor();
          Environment.Exit(1);
        }
      }
    }

    return tokens;
  }


  public static bool IsSkippable(char character)
  {
    return character == ' ' || character == '\t' || character == '\n';
  }


  public static TokenType? GetTokenType(char character)
  {
    switch (character)
      {
        case '(': return TokenType.OpenBracket;
        case ')': return TokenType.CloseBracket;
        case '+' or '-' or '*' or '/': return TokenType.BinaryOperator;
        case '=': return TokenType.Equals;
        default: return null;
      }
  }
}