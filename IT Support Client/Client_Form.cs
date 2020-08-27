using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Net;

namespace Tracking_System_Client
{
    public partial class Client_Form : Form
    {
        public Client_Form()
        {
            InitializeComponent();
        }

        string connetionString = @"Data Source=130.7.1.24;Initial Catalog=Tracking_System;User ID=sa;Password=zxc-1234";
        Int32 port = 8081;
        public string host = "";
        public string ip = "";
        public string macAddresses = "";
        public string get_host()
        {
            host = Dns.GetHostEntry(ip).HostName;
            return host;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().Contains("130.7."))
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
        private string GetMAC()
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.Name.Contains("Ethernet") && (nic.OperationalStatus == OperationalStatus.Up))
                {
                    macAddresses = nic.GetPhysicalAddress().ToString();
                }
            }
            return macAddresses;
        }
        private string Get_eng_name()
        {
            string engineer_id = txt_1.Text;
            SqlConnection conn = new SqlConnection(connetionString);
            string sql = "SELECT name FROM Tracking_System.dbo.Engineers_engineers WHERE engineer_id = '" + engineer_id + "'";
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader sdr = sqlcom.ExecuteReader();
            sdr.Read();
            string engineer_name = sdr["name"].ToString(); ;
            conn.Close();
            return engineer_name.ToString();
        }
        private string Server_Ip()
        {
            string engineer_id = txt_1.Text;
            SqlConnection conn = new SqlConnection(connetionString);
            string sql = "SELECT ip_add FROM Tracking_System.dbo.Engineers_engineers WHERE engineer_id = '"+engineer_id+"'";
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            conn.Open();
            SqlDataReader sdr = sqlcom.ExecuteReader();
            sdr.Read();
            string server = sdr["ip_add"].ToString(); ;
            conn.Close();
            return server.ToString();
        }

        public void Save_session()
        {
            SqlConnection conn = new SqlConnection(connetionString);
            string sql = "insert into Tracking_System.dbo.Sessions_session_info(engineer_name,ip_add,mac,user_name) values(@engineer_name,@ip_add,@mac,@user_name)";
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@engineer_name", Get_eng_name());
            sqlcom.Parameters.Add("@ip_add",GetLocalIPAddress());
            sqlcom.Parameters.Add("@mac",GetMAC());
            sqlcom.Parameters.Add("@user_name", Environment.UserName);
            conn.Open();
            sqlcom.ExecuteNonQuery();
            conn.Close();
        }

        private void btn_1_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient(Server_Ip(), port);
                Save_session();
            }
            catch (Exception)
            {
                
                MessageBox.Show("Please Call 1010 To Get Support ID!") ;
            }
        }

        private void TxetBox_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_1_Click(this, new EventArgs());
            }
        }
    }
}
