using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Frontend
{
    public partial class Form1 : Form
    {
        public MySqlConnection Connection { get; set; }
        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var con = new MySqlConnection(ConfigurationManager.ConnectionStrings["PaulsData"].ConnectionString))
            {
                con.Open();
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = "insert into Bruger (Name) values (@name)";
                    cmd.Parameters.AddWithValue("@Name", textBox1.Text);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
