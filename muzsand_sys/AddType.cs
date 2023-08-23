using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace muzsand_sys
{
    public partial class AddType : Form
    {
        string quary;
        public SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bin\DataBase\hardwareDb.mdf;Integrated Security=True;Connect Timeout=30");
        public SqlCommand cmd;

        public AddType()
        {
            InitializeComponent();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtType.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string type = txtType.Text;
                quary = "Insert into types values('" + type + "')";
                cmd = new SqlCommand(quary, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("New type added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    txtType.Text = "";

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            }
        }
        }
}
