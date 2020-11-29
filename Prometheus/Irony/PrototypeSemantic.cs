using Irony.Parsing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Prometheus.Irony
{
    class PrototypeSemantic
    {
        public static List<string> errors = new List<string>();
        private static List<string> functionsIds = new List<string>();
        private static List<string> declared = new List<string>();
        private static List<ParseTreeNode> dfs = new List<ParseTreeNode>();
        private static List<Datatype> datatypes = new List<Datatype>();
        private static int cycleInits;
        private static int cycleEnds;

        public static void semanthicAnalysis(ParseTreeNode root)
        {
            declared.Clear();
            dfs.Clear();
            cycleInits = 0;
            cycleEnds = 0;
            fillList(root);
        }

        private static void DFS(ParseTreeNode root)
        {
            foreach (ParseTreeNode child in root.ChildNodes)
            {
                dfs.Add(child);
                DFS(child);
            }
        }

        private static void fillList(ParseTreeNode root)
        {
            DFS(root);
            for (int i = 0; i < dfs.Count(); i++)
            {
                if (dfs.ElementAt(i).ToString().Equals("DATATYPE"))
                {
                    string id = dfs.ElementAt(i + 2).FindTokenAndGetText();
                    if (exists(id))
                    {
                        errors.Add("Declaración de variable (" + id + ") duplicada");
                    }
                    else
                    {
                        string datatype = dfs.ElementAt(i).FindTokenAndGetText();

                        declared.Add(id);
                        datatypes.Add(new Datatype(datatype, id));
                    }

                }
                else if (dfs.ElementAt(i).ToString().Contains("(ID)"))
                {
                    if (TokenTable.reservadas.Contains(dfs.ElementAt(i).FindTokenAndGetText()))
                    {
                        errors.Add("La variable " + dfs.ElementAt(i).FindTokenAndGetText() + " es una palabra reservada");
                    }
                    else if (
                            !exists(dfs.ElementAt(i).FindTokenAndGetText())
                            && !checkIsFunction(dfs.ElementAt(i).FindTokenAndGetText(), i)
                        )
                    {
                        errors.Add("La variable " + dfs.ElementAt(i).FindTokenAndGetText() + " está siendo usada antes de de declararse");
                    }
                    else if (TokenTable.reservadas.Contains(dfs.ElementAt(i).FindTokenAndGetText()))
                    {
                        errors.Add("La cadena " + dfs.ElementAt(i).FindTokenAndGetText() + " es una palabra reservada");
                    }
                }
                else if (
                            dfs.ElementAt(i).ToString().Contains("(CLASS-ID)")
                            && TokenTable.reservadas.Contains(dfs.ElementAt(i).FindTokenAndGetText())
                        )
                {
                    errors.Add("La clase " + dfs.ElementAt(i).FindTokenAndGetText() + " es una palabra reservada");
                }
                else if (
                            dfs.ElementAt(i).ToString().Contains("(LIBRARY-ID)")
                            && TokenTable.reservadas.Contains(dfs.ElementAt(i).FindTokenAndGetText())
                        )
                {
                    errors.Add("La librería " + dfs.ElementAt(i).FindTokenAndGetText() + " es una palabra reservada");
                }
                else if (
                            dfs.ElementAt(i).ToString().Equals("FOR-STRUCTURE")
                            || dfs.ElementAt(i).ToString().Equals("DOWHILE-STRUCTURE")
                            || dfs.ElementAt(i).ToString().Equals("WHILE-STRUCTURE")
                        )
                {
                    cycleInits++;
                }
                else if (
                            dfs.ElementAt(i).ToString().Equals("END-CYCLE")
                            || dfs.ElementAt(i).ToString().Equals("END-DOWHILE")
                        )
                {
                    cycleEnds++;
                }
                else if (dfs.ElementAt(i).ToString().Equals("BREAK-NONTERMINAL"))
                {
                    if (!checkIsCycle(i, (cycleInits - cycleEnds)))
                    {
                        errors.Add("No es posible usar " + dfs.ElementAt(i).FindTokenAndGetText() + " en ese contexto");
                    }
                }
                else if (dfs.ElementAt(i).ToString().Equals("CONTINUE-NONTERMINAL"))
                {
                    if (!checkIsCycle(i, (cycleInits - cycleEnds)))
                    {
                        errors.Add("No es posible usar " + dfs.ElementAt(i).FindTokenAndGetText() + " en ese contexto");
                    }
                }
                else if (dfs.ElementAt(i).ToString().Equals("VARIABLE-ASSIGN"))
                {
                    string id = dfs.ElementAt(i + 1).FindTokenAndGetText();
                    if (!isUsedCorrectly(id, (i + 3)))
                    {
                        errors.Add("No se puede asignar el valor \""+ dfs.ElementAt(i + 3).FindTokenAndGetText() +"\" a la variable \"" + id + "\"");
                    }
                }
            }
        }

        private static bool exists(string id)
        {
            foreach (string declaredId in declared)
            {
                if (id.Equals(declaredId))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool checkIsFunction(string token, int currentIndex)
        {
            for (int i = currentIndex; i < dfs.Count(); i++)
            {
                if (dfs.ElementAt(i).ToString().Equals("END-FUNCTION"))
                {
                    functionsIds.Add(token);
                    return true;
                }
                else if (
                        dfs.ElementAt(i).ToString().Equals("VARIABLE-ASSIGN")
                        || dfs.ElementAt(i).ToString().Equals("VARIABLE-CREATION")
                        || dfs.ElementAt(i).ToString().Equals("ARRAY-DECLARATION")
                        || dfs.ElementAt(i).ToString().Equals("CONTROL-STRUCTURE")
                        || dfs.ElementAt(i).ToString().Equals("FUNCTION-USE")
                    )
                {
                    return false;
                }
            }
            return false;
        }

        private static bool checkIsCycle(int currentIndex, int closedCycles)
        {
            if (closedCycles == 0)
            {
                return false;
            }
            for (int i = currentIndex; i < dfs.Count(); i++)
            {
                if (
                        dfs.ElementAt(i).ToString().Equals("END-DOWHILE")
                        || dfs.ElementAt(i).ToString().Equals("END-CYCLE")
                    )
                {
                    return true;
                }
                else if (
                        dfs.ElementAt(i).ToString().Equals("VARIABLE-ASSIGN")
                        || dfs.ElementAt(i).ToString().Equals("VARIABLE-CREATION")
                        || dfs.ElementAt(i).ToString().Equals("ARRAY-DECLARATION")
                        || dfs.ElementAt(i).ToString().Equals("CONTROL-STRUCTURE")
                        || dfs.ElementAt(i).ToString().Equals("FUNCTION-USE")
                    )
                {
                    return false;
                }
                else if (
                            dfs.ElementAt(i).ToString().Equals("FOR-STRUCTURE")
                            || dfs.ElementAt(i).ToString().Equals("DOWHILE-STRUCTURE")
                            || dfs.ElementAt(i).ToString().Equals("WHILE-STRUCTURE")
                        )
                {
                    cycleInits++;
                }
                else if (
                            dfs.ElementAt(i).ToString().Equals("END-CYCLE")
                            || dfs.ElementAt(i).ToString().Equals("END-DOWHILE")
                        )
                {
                    cycleEnds++;
                }
            }
            if (cycleInits == cycleEnds)
            {
                return true;
            }
            return false;
        }

        private static bool isUsedCorrectly(string id, int currentIndex)
        {
            string datatype = getDatatypeById(id);
            List<string> rejected = getRejected(datatype);
            for (int i = currentIndex; i < dfs.Count(); i++)
            {
                foreach (string reject in rejected)
                {
                    if (dfs.ElementAt(i).ToString().Contains(reject))
                    {
                        return false;
                    }
                }

                if (dfs.ElementAt(i).ToString().Equals("CODE-BLOCK"))
                {
                    break;
                }
            }
            return true;
        }

        private static string getDatatypeById(string id)
        {
            foreach (Datatype item in datatypes)
            {
                if (item.Id.Equals(id))
                {
                    return item.DataType;
                }
            }
            return "";
        }

        private static List<string> getRejected(string except)
        {
            switch (except)
            {
                case "int":
                    return new List<string>()
                        {
                            "REAL",
                            "STRING",
                            "CHAR",
                            "BOOL-EXPRESSION"
                        };
                case "long":
                    return new List<string>()
                        {
                            "INTEGER",
                            "STRING",
                            "CHAR",
                            "BOOL-EXPRESSION"
                        };
                case "double":
                    return new List<string>()
                       {
                            "INTEGER",
                            "STRING",
                            "CHAR",
                            "BOOL-EXPRESSION"
                        };
                case "float":
                    return new List<string>()
                        {
                            "INTEGER",
                            "STRING",
                            "CHAR",
                            "BOOL-EXPRESSION"
                        };
                case "String":
                    return new List<string>()
                        {
                            "REAL",
                            "INTEGER",
                            "CHAR",
                            "BOOL-EXPRESSION"
                        };
                case "boolean":
                    return new List<string>()
                        {
                            "REAL",
                            "STRING",
                            "CHAR",
                            "INTEGER"
                        };
                case "byte":
                    return new List<string>()
                        {
                            "REAL",
                            "STRING",
                            "CHAR",
                            "BOOL-EXPRESSION"
                        };
                case "char":
                    return new List<string>()
                        {
                            "REAL",
                            "STRING",
                            "INTEGER",
                            "BOOL-EXPRESSION"
                        };
                case "void":
                case "short":
                default:
                    return new List<string>();
            }
        }
    }
}
