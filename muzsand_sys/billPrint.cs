using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;

namespace muzsand_sys
{
    public partial class billPrint : Form
    {
        public string itemName;
        public string batchNo;
        public string code;
        public string price;
        public billPrint()
        {
            InitializeComponent();
        }

        private void print(Panel pnl)
        {
            PrinterSettings ps = new PrinterSettings();
            panelPrint = pnl;
            getprintare(pnl);
            printPreviewDialog1.Document = printDocument1;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            printPreviewDialog1.ShowDialog();

        }

        private Bitmap memoryimg;

        private void getprintare(Panel pnl)
        {
            memoryimg = new Bitmap(pnl.Width, pnl.Height);
            pnl.DrawToBitmap(memoryimg, new Rectangle(0, 0, pnl.Width, pnl.Height));
        }



        private void billPrint_Load(object sender, EventArgs e)
        {
            lblBatchNo.Text = batchNo;
            lblName.Text = itemName;
            lblPrice.Text = price;
            lblCode.Text = code;

            BarcodeWriter writer = new BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
            picBarcode.Image = writer.Write(lblBatchNo.Text);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Rectangle pageare = e.PageBounds;
            e.Graphics.DrawImage(memoryimg, (pageare.Width / 2) - (this.panelPrint.Width / 2), this.panelPrint.Location.Y);
        }



        private void btnPrint_Click(object sender, EventArgs e)
        {
            print(this.panelPrint);
        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            print(this.panelPrint);
        }
    }
}
