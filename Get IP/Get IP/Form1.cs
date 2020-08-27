using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Get_IP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static FileInfo GetLatestWritenFileFileInDirectory(DirectoryInfo directoryInfo)
        {
            if (directoryInfo == null || !directoryInfo.Exists)
                return null;

            FileInfo[] files = directoryInfo.GetFiles();
            DateTime lastWrite = DateTime.MinValue;
            FileInfo lastWritenFile = null;

            foreach (FileInfo file in files)
            {
                if (file.LastWriteTime > lastWrite)
                {
                    lastWrite = file.LastWriteTime;
                    lastWritenFile = file;
                }
            }
            return lastWritenFile;
        }
        private static string[] ip()
            {
            string rootFolder = @"\\130.7.1.5\sghcairo\00\00-ICT\HELP\";
            var path = new DirectoryInfo(rootFolder);
            string file = rootFolder + GetLatestWritenFileFileInDirectory(path).ToString();
            string text = File.ReadAllText(file);
            string[] info = new string[2];
            foreach (string ln in File.ReadAllLines(file))
            {
                if (ln.Contains("Host Name"))
                {
                    info[1] = ln.Replace("   Host Name . . . . . . . . . . . . : ", "");
                }
                if (ln.Contains("IPv4") && ln.Contains("130.7."))
                {
                    info[0] = ln.Replace("   IPv4 Address. . . . . . . . . . . : ", "").Replace("(Preferred)", "");
                }
            }
            return(info);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text =ip()[0];
            textBox2.Text = ip()[1];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string vnc = @"""C:\\Program Files\\RealVNC\\VNC Viewer\\vncviewer.exe""";
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = string.Format("/c " + vnc + " " + textBox1.Text);
            process.Start();
        }
    }
}
