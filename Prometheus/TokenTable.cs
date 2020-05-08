using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Prometheus
{
    class TokenTable
    {

        public static List<Token> tokenTable = new List<Token>();
        public static string mainPattern = null;
        public static Regex mainRegex = null;

        public static readonly List<string> reservadas = new List<string> {
            "for", "if", "switch", "case", "break", "do", "while", "else", "goto", "point",
            "breakpoint","function", "void", "array", "print", "read", "true", "false", "null", "class", "main",
            "int", "real", "string", "char", "bool"
         };

        public static void InitalizeTokenTable()
        {
            tokenTable.Add(new Token("REAL", @"[0-9]+\.[0-9]+"));
            tokenTable.Add(new Token("ENTERO", @"[0-9]+"));
            tokenTable.Add(new Token("INCREMENTO", @"\+\+"));
            tokenTable.Add(new Token("DECREMENTO", @"\-\-"));
            tokenTable.Add(new Token("INCMULT", @"\+="));
            tokenTable.Add(new Token("DECMULT", @"\-="));
            tokenTable.Add(new Token("COMENTARIO", @"//[^\r\n]*|/\*.*\*/"));
            tokenTable.Add(new Token("ARITMETICO", @"[\+\-/*%]"));
            tokenTable.Add(new Token("COMPARACION", @"==|<>|<=|>=|<|>"));
            tokenTable.Add(new Token("LOGICO", @"&&|\|\||!"));
            tokenTable.Add(new Token("CADENA", "\".*\"|\'.*\'"));
            tokenTable.Add(new Token("DELIMITADOR", @"[\{\}\(\)\[\];,\.]"));
            tokenTable.Add(new Token("ASIGNACION", @"="));
            tokenTable.Add(new Token("IDENTIFICADOR", @"\b[_a-zA-Z]+[0-9]*\b"));
            tokenTable.Add(new Token("ESPACIO", @"\s+"));
            tokenTable.Add(new Token("ERROR", @"\&|\|"));
        }

        public static void InitializeMainPattern()
        {
            string groupPatternStructure;
            foreach (Token token in tokenTable)
            {
                if (mainPattern == null)
                {
                    groupPatternStructure = @"(?<" + token.Name + ">" + token.Regex + ")";
                    mainPattern = groupPatternStructure;
                }
                else
                {
                    groupPatternStructure = "|(?<" + token.Name + ">" + token.Regex + ")";
                    mainPattern += groupPatternStructure;
                }
            }
        }

        public static void InitializeMainRegex()
        {
            mainRegex = new Regex(mainPattern, RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.ExplicitCapture);
        }

        public static List<TokenCode> LexicalAnalysis(string code)
        {
            
            Match match = mainRegex.Match(code);
            string[] groupNames = mainRegex.GetGroupNames();
            int[] groupNumbers = mainRegex.GetGroupNumbers();
            int line = 1;
            int column = 1;
            int index = 0;
            List<TokenCode> result = new List<TokenCode>();

            if (!match.Success)
            {
                return new List<TokenCode>();
            }


            while (match.Success)
            {
                for (int i = index; i < match.Index; i++)
                {
                    if (code[i] == '\n')
                    {
                        line++;
                        column = 1;
                        continue;
                    }
                    if (!mainRegex.IsMatch(code[i].ToString()))
                    {
                        result.Add(new TokenCode("ERROR", code[i].ToString(), line, column));
                    }
                    column++;
                }

                for (int i = 1; i < groupNumbers.Length; i++)
                {
                    if (match.Groups[i].Success)
                    {
                        if (reservadas.Contains(match.Value))
                        {
                            result.Add(new TokenCode("RESERVADA", match.Value, line, column));
                        }
                        else
                        {
                            result.Add(new TokenCode(groupNames[i], match.Value, line, column));
                        }
                    }
                }
                index = match.Index;
                match = match.NextMatch();
            }

            return result;
        }
    }
}
