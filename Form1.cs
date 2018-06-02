using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UserLedTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static int count = 0;
        System.Timers.Timer time;
        private void Form1_Load(object sender, EventArgs e)
        {
            time = new System.Timers.Timer();
            time.Interval = 1000;
            time.AutoReset = true;
            time.Elapsed += new System.Timers.ElapsedEventHandler(InitTimer);
            time.Enabled = true;
        }

        public void InitTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            count++;
            ledTest2.num = count;
        }
    }
}
