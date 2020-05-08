using System;
using System.Windows.Forms;

namespace Prometheus
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TokenTable.InitalizeTokenTable();
            TokenTable.InitializeMainPattern();
            TokenTable.InitializeMainRegex();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
