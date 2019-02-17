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

        private void Button1_Click(object sender, EventArgs e) {
            FindIP();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void FindIP() {
            String returnMessage = String.Empty;
            IPAddress[] ipAddresses;

            try {
                ipAddresses = Dns.GetHostAddresses(textBox5.Text);

                foreach (IPAddress ip in ipAddresses) {
                    returnMessage = returnMessage + " " + ip + " ";
                    PingIP(ip);
                }

                textBox1.Text = returnMessage;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
                textBox1.Text = ex.ToString();
            }
        }//takes the hostname, find the IP addresses, and then pings each one and adds it to the result box

        private void PingIP(IPAddress ip) {
            Ping ping = new Ping();
            PingReply reply;

            reply = ping.Send(ip);

            if(reply.Status == IPStatus.Success) {
                textBox3.Text = textBox3.Text  + "Ping to " + textBox5.Text + " " + ip.ToString() + "[" + reply.ToString() + "] " + "SUCCESSFUL - " + "Response delay = " + reply.RoundtripTime.ToString() + "ms" + "\r\n";
            }
            else {
                textBox3.Text = textBox3.Text + "Ping to " + textBox5.Text + " " + ip.ToString() + "[" + reply.ToString() + "] " + "FAILED - " + "Response delay = " + reply.RoundtripTime.ToString() + "ms" + "\r\n";
            }
        }//takes the IP addresses from FindIP() and then creates a report on their pingability

        private void TextBox5_Click(object sender, EventArgs e) {
            textBox5.Text = "";
        }

        private void Form1_Resize(object sender, EventArgs e) {
            if(this.WindowState == FormWindowState.Minimized) {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }
    }
}
