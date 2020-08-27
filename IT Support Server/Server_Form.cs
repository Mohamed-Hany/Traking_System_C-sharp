using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Security.Permissions;

namespace Tracking_System
{
    public partial class Server_Form : Form
    {
        string connetionString = @"Data Source=130.7.1.24;Initial Catalog=Tracking_System;User ID=sa;Password=zxc-1234";
        static IPAddress ipAddr = IPAddress.Any;
        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8081);
        Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);


        public Server_Form()
        {
            InitializeComponent();
        }

        public string ip = "";
        public string host = "";

        public void Get_ID()
        {
            SqlConnection conn = new SqlConnection(connetionString);
            string sql = "SELECT engineer_id FROM Tracking_System.dbo.Engineers_engineers WHERE ip_add = '"+GetLocalIPAddress()+"'";
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader sdr = sqlcom.ExecuteReader();
            sdr.Read();
            ID.Text = sdr["engineer_id"].ToString();
            conn.Close();
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().Contains("130.7.") ) 
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
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
            ip = client_ip.Address.ToString();
            vnc();
            Environment.Exit(0);
        }
        public void vnc()
        {
            /*ClientInfo_Form form = new ClientInfo_Form();
            //form.Show();
           DialogResult con_message = MessageBox.Show(ip,"Incoming Connection", MessageBoxButtons.OKCancel);
            if (con_message == DialogResult.OK)
            {*/
                string vnc = @"""C:\\Program Files\\RealVNC\\VNC Viewer\\vncviewer.exe""";
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.Arguments = string.Format("/c " + vnc + " " + ip);
                process.Start();
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
           //btn_start.Enabled = true;
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
