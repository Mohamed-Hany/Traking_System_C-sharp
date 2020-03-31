using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Data.SqlClient;

namespace Tracking_System_Client
{
    public partial class Client_Form : Form
    {
        public Client_Form()
        {
            InitializeComponent();
        }

        string connetionString = @"Data Source=192.168.1.100;Initial Catalog=Tracking_System;User ID=sa;Password=zxc-1234";
        Int32 port = 8081;

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

        private void btn_1_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient(Server_Ip(), port);
            }
            catch (Exception)
            {
                
                MessageBox.Show("Server not started") ;
            } 
        }
    }
}
