using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;


namespace Tracking_System
{
    public partial class Server_Form : Form
    {
        string connetionString = @"Data Source=192.168.1.100;Initial Catalog=Tracking_System;User ID=sa;Password=zxc-1234";
        string state = "";
        static IPAddress ipAddr = IPAddress.Any;
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8081);
        Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


        public Server_Form()
        {
            InitializeComponent();
        }
        public void Get_ID()
        {
            SqlConnection conn = new SqlConnection(connetionString);
            string sql = "SELECT engineer_id FROM Tracking_System.dbo.Engineers_engineers WHERE ip_add = '192.168.1.100'";
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader sdr = sqlcom.ExecuteReader();
            sdr.Read();
            ID.Text = sdr["engineer_id"].ToString(); ;
            conn.Close();
        }
        public void ServerThread()
        {
            Thread ServerTh = new Thread(new ThreadStart(Server_start));
            ServerTh.Start();
        }
                
        public void Server_start()
        {

            listener.Bind(localEndPoint);
            listener.Listen(0);
            Socket clientSocket = listener.Accept();
            IPEndPoint client_ip = clientSocket.RemoteEndPoint as IPEndPoint;
            string ip = client_ip.Address.ToString();
            string vnc = @"""C:\\Program Files\\RealVNC\\VNC Viewer\\vncviewer.exe""";
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe"; // add full path
            process.StartInfo.Arguments = string.Format("/c " + vnc + " " + ip);
            process.Start();
            listener.Close();

     
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
            lbl_start.Visible = true;
            lbl_stop.Visible = false;
            lbl_ID.Enabled = true;
            ID.Enabled = true;
            ID.Visible = true;
            Get_ID();
            ServerThread();
        }
        
        private void btn_stop_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
            /*btn_stop.Enabled = false;
            btn_start.Enabled = true;
            lbl_ID.Enabled = false;
            ID.Enabled = false;
            lbl_stop.Visible = true;*/
        }
    }
}
