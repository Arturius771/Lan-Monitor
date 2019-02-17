using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace Lan_Monitor {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            try {
                String returnMessage = String.Empty;
                IPAddress[] ipAddresses;

                ipAddresses = Dns.GetHostAddresses(textBox5.Text);

                foreach (IPAddress ip in ipAddresses) {
                    returnMessage = returnMessage + " " + ip + " ";
                }

                textBox1.Text = returnMessage;
            }
            catch(Exception ex) {
                Console.WriteLine(ex.ToString());
                textBox1.Text = ex.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }
    }
}
