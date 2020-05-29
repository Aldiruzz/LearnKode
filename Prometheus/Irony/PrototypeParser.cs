using System;
using Irony.Parsing;

namespace Prometheus.Irony
{
    class PrototypeParser : Grammar
    {
        public static string errors = "";
        public static string treeText = "";
        private static int cont = 0;
    
        public static bool Parse(string text)
        {

            PrototypeGrammar grammar = new PrototypeGrammar();
            LanguageData language = new LanguageData(grammar);
            Parser parser = new Parser(language);
            ParseTree tree = parser.Parse(text);
            ParseTreeNode root = tree.Root;

            if (root == null)
            {
                foreach (var error in tree.ParserMessages)
                {
                    errors += "Error: " + error.Message + " at line: " + (error.Location.Line + 1) + ", column: " + error.Location.Column + "\r\n";
                }
                return false;
            }
            TravelAST(root);
            return true;
        }

        private static void TravelAST(ParseTreeNode children)
        {
            foreach (ParseTreeNode child in children.ChildNodes)
            {
                treeText += "Nodo[" + cont.ToString()  + "]: " + child.ToString() + "\r\n";
                cont++;
                TravelAST(child);
            }
        }
    }
}
