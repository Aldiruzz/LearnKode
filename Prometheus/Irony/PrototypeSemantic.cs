using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prometheus.Irony
{
    class PrototypeSemantic
    {
        public static List<string> errors = new List<string>();
        public static List<string> declared = new List<string>();
        private static List<ParseTreeNode> dfs = new List<ParseTreeNode>();

        public static void semanthicAnalysis(ParseTreeNode root)
        {
            declared.Clear();
            dfs.Clear();
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

        public static void fillList(ParseTreeNode root)
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
                        declared.Add(id);
                    }

                }
                else
                {
                    // variable (ID)
                    if (dfs.ElementAt(i).ToString().Contains("(ID)"))
                    {
                        if (!exists(dfs.ElementAt(i).FindTokenAndGetText()))
                        {
                            errors.Add("La variable " + dfs.ElementAt(i).FindTokenAndGetText() + " está siendo usada antes de de declararse");
                        }
                    }
                }
            }
        }

        public static bool exists(string id)
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
    }
}
