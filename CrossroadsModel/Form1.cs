using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace CrossroadsModel
{
    public partial class Form1 : Form
    {
        Thread t;
        public Form1(Thread t)
        {
            InitializeComponent();
            this.t = t;
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            t.Abort();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            t.Suspend();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (t.ThreadState == ThreadState.Suspended)
                t.Resume();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            t.Abort();
            Close();
        }
    }
}