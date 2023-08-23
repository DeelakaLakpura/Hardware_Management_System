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
using ZXing;

namespace muzsand_sys
{
    public partial class Form1 : Form
    {


        public SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\bin\DataBase\hardwareDb.mdf;Integrated Security=True;Connect Timeout=30");
        int chechBatchNo = 0;
        int chechQuantity = 0;
        float chechPrice = 0;
        float chechNewpirce = 0;
        SqlDataReader dr;




        public Form1()
        {
            InitializeComponent();
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

        public void refresh()
        {
            string sql = "Select batchNo as 'Batch No',itemName as 'Item Name', quantity as 'Quantity', oldPrice as 'Price',newPrice as 'New Price',code as 'Code',type as 'Type' from stock";
            SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
            DataSet ds = new DataSet();
            sda.Fill(ds, "stock");
            DataGride.DataSource = ds.Tables[0];
        }
        private void clear()
        {
            TxtBatchNo.Text = "";
            TxtItemName.Text = "";
            TxtPrice.Text = "";
            TxtNewPrice.Text = "";
            TxtQty.Text = "";
            TxtCode.Text = "";
            CbCat.Text = "";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Categories();
            refresh();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddType at = new AddType();
            at.Show();
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            SoldItems SLD = new SoldItems();
            SLD.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (TxtBatchNo.Text == "" && TxtItemName.Text == "" && TxtQty.Text == "" && TxtPrice.Text == "" && TxtCode.Text == "" && CbCat.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Chech batch no,quantity,price,new price is intigers
            else if (!int.TryParse(TxtBatchNo.Text, out chechBatchNo))
            {
                MessageBox.Show("Batch number only containe number, Please check!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!int.TryParse(TxtQty.Text, out chechQuantity))
            {
                MessageBox.Show("Quantity only containe number, Please check!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!float.TryParse(TxtPrice.Text, out chechNewpirce))
            {
                MessageBox.Show("Please check price", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!float.TryParse(TxtNewPrice.Text, out chechNewpirce))
            {
                MessageBox.Show("New price only containe number, Please check!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int batchNo = int.Parse(TxtBatchNo.Text);
                string name = TxtItemName.Text;
                int quantity = int.Parse(TxtQty.Text);
                float price = float.Parse(TxtPrice.Text);
                float newPrice = float.Parse(TxtNewPrice.Text);
                string code = TxtCode.Text;
                string type = CbCat.Text;

                String quary;


                quary = "Insert into stock values('" + batchNo + "','" + name + "','" + quantity + "','" + price + "','" + newPrice + "','" + code + "','" + type + "')";
                SqlCommand cmd = new SqlCommand(quary, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("New item saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    clear();
                    refresh();
                }

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (TxtBatchNo.Text == "")
            {
                MessageBox.Show("Please Enter Barcode Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                picBarcode.Image = writer.Write(TxtBatchNo.Text);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (TxtBatchNo.Text == "" && TxtItemName.Text == "" && TxtQty.Text == "" && TxtPrice.Text == "" && TxtNewPrice.Text == "" && CbCat.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int batchNo = int.Parse(TxtBatchNo.Text);
                String quary;
                quary = "delete from stock where batchNo='" + batchNo + "'";
                SqlCommand cmd = new SqlCommand(quary, conn);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item deleted!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    clear();
                    refresh();
                }
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
                    TxtBatchNo.Text = id;
                    TxtItemName.Text = reader[1].ToString();
                    TxtQty.Text = reader[2].ToString();
                    TxtPrice.Text = reader[3].ToString();
                    TxtNewPrice.Text = reader[4].ToString();
                    TxtCode.Text = reader[5].ToString();
                    CbCat.Text = reader[6].ToString();

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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string name = TxtSearch.Text;
            if (name == "")
            {
                MessageBox.Show("Please enter item name", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                string sql = "Select batchNo as 'Batch No',itemName as 'Item Name', quantity as 'Quantity', oldPrice as 'Price',newPrice as 'New Price' from stock where itemName like '%" + name + "%'";
                SqlDataAdapter sda = new SqlDataAdapter(sql, conn);
                DataSet ds = new DataSet();
                sda.Fill(ds, "Stock");
                DataGride.DataSource = ds.Tables[0];
                TxtSearch.Text = "";
                getDetails(name);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (TxtBatchNo.Text == "" && TxtItemName.Text == "" && TxtQty.Text == "" && TxtNewPrice.Text == "" && TxtCode.Text == "" && CbCat.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!int.TryParse(TxtBatchNo.Text, out chechBatchNo))
            {
                MessageBox.Show("Batch number only containe number, Please check!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!int.TryParse(TxtQty.Text, out chechQuantity))
            {
                MessageBox.Show("Quantity only containe number, Please check!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!float.TryParse(TxtPrice.Text, out chechNewpirce))
            {
                MessageBox.Show("Please check price", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!float.TryParse(TxtNewPrice.Text, out chechNewpirce))
            {
                MessageBox.Show("New price only containe number, Please check!", "Warrning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int batchNo = int.Parse(TxtBatchNo.Text);
                string name = TxtItemName.Text;
                int quantity = int.Parse(TxtQty.Text);
                float price = float.Parse(TxtPrice.Text);
                float newPrice = float.Parse(TxtNewPrice.Text);
                string code = TxtCode.Text;
                string type = CbCat.Text;

                String quary = "update stock set itemName='" + name + "',quantity='" + quantity + "',oldPrice='" + price + "',newPrice='" + newPrice + "',code='" + code + "',type='" + type + "' where batchNo='" + batchNo + "'";
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
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
            }
        private void Categories()
        {
            SqlCommand cmd = new SqlCommand();
            conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM types";
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                CbCat.Items.Add(dr["type"]);
            }
            conn.Close();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {

            Categories();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (TxtBatchNo.Text == "" && TxtItemName.Text == "" && TxtQty.Text == "" && TxtPrice.Text == "" && TxtNewPrice.Text == "" && TxtCode.Text == "")
            {
                MessageBox.Show("Please fill all fields!", "Missing", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                using (billPrint print = new billPrint())
                {
                    print.itemName = TxtItemName.Text;
                    print.price = TxtNewPrice.Text;
                    print.code = TxtCode.Text;
                    print.batchNo = TxtBatchNo.Text;
                    print.ShowDialog();
                }
            }
        }

        private void guna2ImageButton1_Click_1(object sender, EventArgs e)
        {
            Categories();
        }
    }
    }
