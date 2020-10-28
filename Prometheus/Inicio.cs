using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Prometheus
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            MainForm abrirPantallaCarga = new MainForm();
            abrirPantallaCarga.Show();
            this.Hide();
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            this.Empezar.BackColor = Color.White;
            this.Empezar.ForeColor = Color.FromArgb(0, 135, 255);
        }

        private void Empezar_MouseLeave(object sender, EventArgs e)
        {
            this.Empezar.BackColor = Color.FromArgb(0, 135, 255);
            this.Empezar.ForeColor = Color.White;
        }

        private void Empezar_Click(object sender, EventArgs e)
        {
            MainForm abrirPantallaCarga = new MainForm();
            abrirPantallaCarga.Show();
            this.Hide();
        }
    }
}
