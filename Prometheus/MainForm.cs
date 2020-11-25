using Prometheus.Irony;
using System;
using System.Drawing;
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
                    if (token.Name != "SPACE" && token.Name != "IGNORE")
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

        private void btnParse_Click(object sender, EventArgs e)
        {
            tbSintaxResult.Text = "";
            PrototypeParser.errors = "";
            PrototypeParser.treeText = "";
            bool result = PrototypeParser.Parse(tbCode.Text);
            string resultText;
            if (result)
            {
                tbSintaxResult.ForeColor = Color.RoyalBlue;
                resultText = "No errors found\r\nCorrect analysis\r\n";
                resultText += "\r\n=====TREE=====\r\n";
                resultText += PrototypeParser.treeText;
            }
            else
            {
                tbSintaxResult.ForeColor = Color.Red;
                resultText = PrototypeParser.errors;
            }
            tbSintaxResult.Text = resultText;

            checkSemanthic();
        }

        private void checkSemanthic()
        {
            string resultText = "\r\n";
            foreach (string possibleError in PrototypeSemantic.declared)
            {
                if (possibleError.Contains("está") || possibleError.Contains("Declaración"))
                {
                    resultText += possibleError + "\r\n";
                }
            }
            if (resultText.Equals(""))
            {
                tbSemanticResult.ForeColor = Color.RoyalBlue;
                resultText = "No errors found\r\nCorrect analysis\r\n";
            }
            else
            {
                tbSemanticResult.ForeColor = Color.Red;
            }
            tbSemanticResult.Text = resultText;
        }


        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Inicio abrirPantallaCarga = new Inicio();
            abrirPantallaCarga.Show();
            this.Hide();
        }
    }
}
