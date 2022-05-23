using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace insoftpro_projesi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            aboneekle aboneekle = new aboneekle();
            aboneekle.StartPosition = FormStartPosition.CenterScreen;

            aboneekle.Show();
            this.Hide();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            endekshesaplama endekshesaplama = new endekshesaplama();
            endekshesaplama.StartPosition = FormStartPosition.CenterScreen;
            endekshesaplama.Show();
            this.Hide();
        }
    }
}
