using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Form1 : Form
    {
        Contact.Server server;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null && server.Running)
                server.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            button1.Enabled = false;
            server = new Contact.Server(textBox1.Text);
            server.Start();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            richTextBox1.Text = server.Log;
        }
    }
}
