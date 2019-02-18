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
using System.IO;

namespace Lan_Monitor {
    public partial class Form1 : Form {

        string hostName = Dns.GetHostName();
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        public Form1() {
            InitializeComponent();
            GetLocalInformation();
        }//starts the form, and displays local machine information

        private void GetLocalInformation() {
            textBox6.Text = "Localhost: " + hostName;
            textBox7.Text = String.Empty;
            foreach (DriveInfo d in allDrives) {
                textBox7.Text = textBox7.Text + "Drive " + d.Name + "\r\n";
                textBox7.Text = textBox7.Text + "  Drive type: " + d.DriveType + "\r\n";
                if (d.IsReady == true) {
                    textBox7.Text = textBox7.Text + "  Volume label: " + d.VolumeLabel + "\r\n";
                    textBox7.Text = textBox7.Text + "  File system: " + d.DriveFormat + "\r\n";
                    textBox7.Text = textBox7.Text + "  Available space to current user: " + d.AvailableFreeSpace + " bytes" + "\r\n";
                    textBox7.Text = textBox7.Text + "  Total available space: " + d.TotalFreeSpace + " bytes" + "\r\n";
                    textBox7.Text = textBox7.Text + "  Total size of drive: " + d.TotalSize + " bytes" + "\r\n";
                }
            }
        }

        private void Button1_Click(object sender, EventArgs e) {
            FindIP();
        }//finds the hostname

        private void Form1_Load(object sender, EventArgs e) {

        }//load

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
        }//clears the textbox

        private void Form1_Resize(object sender, EventArgs e) {
            if(this.WindowState == FormWindowState.Minimized) {
                Hide();
                notifyIcon1.Visible = true;
            }
        }//minimize and hide in notification area

        private void notifyIcon1_DoubleClick(object sender, EventArgs e) {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }//maximizes from notification area

        private void button2_Click(object sender, EventArgs e) {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;

            if (saveFile.ShowDialog() == DialogResult.OK) {
                File.WriteAllText(saveFile.FileName, textBox6.Text + "\r\n" + textBox7.Text + "\r\n" + textBox5.Text + "\r\n" + textBox3.Text);
            }
        }
    }
}
