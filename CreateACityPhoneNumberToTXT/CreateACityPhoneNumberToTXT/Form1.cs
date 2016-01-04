using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CreateACityPhoneNumberToTXT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          richTextBox1.AppendText(过滤());
        }

        public string 过滤()
        {
            StringBuilder sb = new StringBuilder();
            var regex = "[\"^>\"]1[0-9]{6}";
            var rawRes="";
            Regex r = new Regex(regex, RegexOptions.IgnoreCase);
            using (var webclient = new WebClient())
            {
                rawRes = webclient.DownloadString("http://www.sjhcx.com/city/hunan/changsha.php"); 
            }
            Match m = r.Match(rawRes);
            while (m.Success)
            {
                sb.AppendLine(m.Value);
                m = m.NextMatch();
            }

            return sb.ToString().Replace(">","");
        }
    }
}
