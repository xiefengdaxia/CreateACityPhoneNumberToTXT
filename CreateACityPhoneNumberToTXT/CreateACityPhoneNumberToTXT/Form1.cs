using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace CreateACityPhoneNumberToTXT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Queue<string> queue;
        public StringBuilder sb = new StringBuilder();
        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.AppendText(GetPhoneStartNumber());
            MessageBox.Show("共计:" + queue.Count.ToString() + "个号段！");
            Thread worker = new Thread(delegate()
            {
                while (queue.Count > 0)
                {
                    string tmp = queue.Dequeue();
                    for (int i = 0; i < 10000; i++)
                    {
                        sb.AppendLine(tmp + i.ToString().PadLeft(4, '0'));
                    }
                    saveTxt(sb.ToString().Replace(">", ""), textBox1.Text.Split('/')[5].Replace(".php", "") + "PhoneNumber");
                    sb.Remove(0, sb.Length);
                }
                System.Diagnostics.Process.Start("explorer.exe", txtpath);
            });
            worker.IsBackground = true;
            worker.Start(); 
        }

        public string GetPhoneStartNumber()
        {
            StringBuilder sb = new StringBuilder();
            var regex = "[\"^>\"]1[0-9]{6}";
            var rawRes="";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            using (var webclient = new WebClient())
            {
                rawRes = webclient.DownloadString(textBox1.Text); 
            }
            Match m = r.Match(rawRes);
            while (m.Success)
            {
                sb.AppendLine(m.Value);
                queue.Enqueue(m.Value);
                m = m.NextMatch();
            }

            return sb.ToString().Replace(">","");
        }
        public static object locker = new object();
        public static string txtpath = Application.StartupPath + "\\";
        public static void saveTxt(string vaule, string txtName)
        {
            lock (locker)
            {

                if (File.Exists(txtpath + txtName + ".txt"))
                {
                    StreamWriter streamWriter = File.AppendText(txtpath + txtName + ".txt");
                    streamWriter.WriteLine(vaule);
                    streamWriter.Close();
                }
                else
                {
                    StreamWriter streamWriter = File.CreateText(txtpath + txtName + ".txt");
                    streamWriter.WriteLine(vaule);
                    streamWriter.Close();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            queue = new Queue<string>();
        }
    }
}
