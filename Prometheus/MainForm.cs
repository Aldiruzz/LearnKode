using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Prometheus
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        private void Analize_Text(object sender, EventArgs e)
        {
            AnalizeCode();
        }

        private void AnalizeCode()
        {
            dgvLexer.Rows.Clear();
            foreach (TokenCode token in TokenTable.LexicalAnalysis(tbCode.Text))
            {
                if (token.Name == "ERROR")
                {
                    dgvLexer.Rows.Add(token.Name, token.Lexeme, token.Line, token.Column);
                }
                else
                {
                    if (token.Name != "ESPACIO" && token.Name != "IGNORE")
                    {
                        dgvLexer.Rows.Add(token.Name, token.Lexeme, token.Line, token.Column);
                    }
                }
            }
        }

        private void btnNewFile_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm newForm = new MainForm();
            newForm.Closed += (s, args) => this.Close();
            newForm.Show();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                FileName = "Select a file",
                Filter = "Prototype files (*.pr)|*.pr",
                Title = "Open your code"
            };
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                StreamReader sr = new StreamReader(dialog.FileName);
                tbCode.Text = sr.ReadToEnd();
            }
        }
    }
}
