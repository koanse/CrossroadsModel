using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CrossroadsModel
{
    public partial class ParamsForm : Form
    {
        public float lambdaAudi, lambdaBus, lambdaTruck;
        public int sleepTime;
        public bool visualisation;
        public ParamsForm()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                lambdaAudi = float.Parse(textBox1.Text);
                lambdaBus = float.Parse(textBox2.Text);
                lambdaTruck = float.Parse(textBox3.Text);
                sleepTime = int.Parse(textBox4.Text);
                visualisation = checkBox1.Checked;
                DialogResult = DialogResult.OK;
                Close();
            }
            catch
            {
                MessageBox.Show("Ошибка при задании параметров", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}