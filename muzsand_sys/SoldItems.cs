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
    public partial class SoldItems : Form
    {
        public SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bin\DataBase\hardwareDb.mdf;Integrated Security=True;Connect Timeout=30");


        public SoldItems()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dialog = dialog = MessageBox.Show("Do you really want to close the program?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.No)
            {

            }
            else
            {
                this.Close();

            }
        }

        private void getDetails(string name)
        {
            try
            {
                string quary = "Select * from stock where itemName like '%" + name + "%'";
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlCommand cmd = new SqlCommand(quary, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader[0].ToString();
                    txtBatchNo.Text = id;
                    txtItemName.Text = reader[1].ToString();
                    lblQuantity.Text = reader[2].ToString();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(" " + ex);

            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string name = txtSearch.Text;
            if (name == "")
            {
                MessageBox.Show("Please enter item name", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string sql = "Select batchNo as 'Batch No',itemName as 'Item Name', quantity as 'Quantity' from stock where itemName like '%" + name + "%'";
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                sda.Fill(ds, "stock");
                dataGridView1.DataSource = ds.Tables[0];
                txtSearch.Text = "";
                getDetails(name);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (txtBatchNo.Text == "" && txtItemName.Text == "" && txtSoldItems.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int newQun, OldQun, sold = int.Parse(txtSoldItems.Text);
                OldQun = int.Parse(lblQuantity.Text);
                int batchNo = int.Parse(txtBatchNo.Text);

                newQun = OldQun - sold;

               String quary = "update stock set quantity='" + newQun + "' where batchNo='" + batchNo + "'";
               SqlCommand cmd = new SqlCommand(quary, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refresh();
                    clear();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Batch number alrady inserted, Please give diferant batch number", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    conn.Close();
                }

            }

        }
        private void refresh()
        {
            string sql = "Select batchNo as 'Batch No',itemName as 'Item Name', quantity as 'Quantity' from stock";
            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            sda.Fill(ds, "Stock");
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void clear()
        {
            txtBatchNo.Text = "";
            txtItemName.Text = "";
            txtSoldItems.Text = "";
            lblQuantity.Text = "0";
        }

    }
}
