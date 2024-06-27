using System;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using static System.Drawing.Printing.PrinterSettings;
using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;

namespace Devzec_printing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void clearAll()
        {
            dataGridView1.Rows.Clear();
            comboBox2.Text = "";
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();
            textBox10.Clear();
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            textBox14.Clear();
            textBox1.Clear();
            textBox15.Clear();
            comboBox1.Text = "";
        }
        public void clearProduct()
        {
            comboBox2.Text = "";
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
        }



        public void balance()
        {
            var netAmount = int.Parse(textBox9.Text);
            var cash = int.Parse(textBox13.Text);
            var balance = cash - netAmount;
            textBox7.Text = balance.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection(@"Data Source=HUBMAN;Initial Catalog=DevzecPrinting;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Product", con);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    comboBox2.Items.Add(dr["itemCode"].ToString());
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var itemCode = comboBox2.Text;
                var description = textBox3.Text;
                var grossAmount = int.Parse(textBox4.Text);
                var quantity = int.Parse(textBox5.Text);
                var discount = int.Parse(textBox6.Text);

                var totalAmount = grossAmount * quantity;
                var netAmount = (grossAmount * quantity) - discount;

                //update the data in the data grid view

                dataGridView1.Rows.Add(itemCode, description, grossAmount, quantity, totalAmount, discount, netAmount);

                clearProduct();

                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;

                //calculate the total amount
                int sum = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum += int.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                }
                textBox15.Text = sum.ToString();

                //calculate the total discount
                int sum1 = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum1 += int.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString());
                }
                textBox12.Text = sum1.ToString();

                //calculate the total net amount
                int sum2 = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    sum2 += int.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                }
                textBox9.Text = sum2.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            

        }

        private void button2_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clearAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox10.Clear();
            textBox11.Clear();
            textBox8.Clear();
            comboBox1.Text = "";
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HUBMAN;Initial Catalog=DevzecPrinting;Integrated Security=True");   
            con.Open();

            //get paratameters from the database
            SqlCommand cmd = new SqlCommand("select * from printParameters", con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            var PaperSize = dr["PaperSize"].ToString();

            if (PaperSize == "A4")
            {
                //A4 paper size

                var companyName = dr["Name"].ToString();
                var companyAddress = dr["Address"].ToString();
                var companyContact = dr["Phone"].ToString();
                var companySlogan = dr["Slogan"].ToString();
                var companyLogo = System.Drawing.Image.FromFile(dr["Logo"].ToString());
                var font = dr["Font"].ToString();
                var separator = dr["Separator"].ToString();


                var time = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                var fontSize = 13;
                var cashier = textBox8.Text;
                var receiptNo = textBox1.Text;
                var orderNo = textBox14.Text;
                var customerName = textBox13.Text;
                var customerPhone = textBox10.Text;

                var totalAmount = textBox15.Text;
                var totalDiscount = textBox12.Text;
                var netAmount = textBox9.Text;
                var cash = textBox13.Text;
                var balance = textBox7.Text;

                //the paper size
                PaperSize paperSize = new PaperSize("papersize", 827, 1169);
                var paperWidth = paperSize.Width;
                var paperHeight = paperSize.Height;
                var brush = Brushes.Purple;


                //design the receipt
                e.Graphics.DrawImage(companyLogo, 10, 10, 100, 100);
                e.Graphics.DrawString(companyName, new Font(font, 35, FontStyle.Bold), brush, new Point(200, 10));
                e.Graphics.DrawString(companyAddress, new Font(font, 15, FontStyle.Regular), brush, new Point(325, 65));
                e.Graphics.DrawString(companyContact, new Font(font, 15, FontStyle.Regular), brush, new Point(350, 90));
                e.Graphics.DrawString(companySlogan, new Font(font, 15, FontStyle.Regular), brush, new Point(300, 120));

                e.Graphics.DrawString("Date: " + time, new Font(font, 15, FontStyle.Regular), brush, new Point(15, 150));
                e.Graphics.DrawString("Recipt No: " + receiptNo, new Font(font, 15, FontStyle.Regular), brush, new Point(600, 150));

                e.Graphics.DrawLines(new Pen(brush), new Point[] { new Point(10, 180), new Point(paperWidth - 10, 180) });

                //draw the receipt body
                e.Graphics.DrawString("Description", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(20, 190));
                e.Graphics.DrawString("Gross Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(170, 190));
                e.Graphics.DrawString("Quantity", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(320, 190));
                e.Graphics.DrawString("Total Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(400, 190));
                e.Graphics.DrawString("Discount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(550, 190));
                e.Graphics.DrawString("Net Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(700, 190));


                e.Graphics.DrawLine(new Pen(brush), new Point(10, 220), new Point(paperWidth - 10, 220));

                //draw the items to the receipt body using a loop
                //if the items are more than 10, then create a new page and continue printing
                int yPos = 240;

                int count = 0;
                int itemsPerPage = 10;
                int itemsLeft = dataGridView1.Rows.Count - count;
                int currentPage = 1;

                if (currentPage == 1)
                {
                    for (int i = 0; i < itemsPerPage; i++)
                    {
                        if (count < dataGridView1.Rows.Count)
                        {
                            e.Graphics.DrawString(dataGridView1.Rows[count].Cells[1].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(20, yPos));
                            e.Graphics.DrawString(dataGridView1.Rows[count].Cells[2].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(170, yPos));
                            e.Graphics.DrawString(dataGridView1.Rows[count].Cells[3].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(320, yPos));
                            e.Graphics.DrawString(dataGridView1.Rows[count].Cells[4].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(400, yPos));
                            e.Graphics.DrawString(dataGridView1.Rows[count].Cells[5].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(550, yPos));
                            e.Graphics.DrawString(dataGridView1.Rows[count].Cells[6].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(700, yPos));

                            yPos += 30;
                            count++;
                        }
                        else
                        {
                            e.HasMorePages = false;
                            break; // No more items to print
                        }
                    }

                    if (itemsLeft > 10)
                    {
                        e.HasMorePages = true;
                    }
                    else
                    {
                        e.HasMorePages = false;
                        currentPage = 1;
                    }
                }
                else if (currentPage == 2)
                {
                    // Print the remaining items starting from the 11th item
                    for (int i = 10; i < dataGridView1.RowCount; i++)
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[count].Cells[1].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(20, yPos));
                        e.Graphics.DrawString(dataGridView1.Rows[count].Cells[2].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(170, yPos));
                        e.Graphics.DrawString(dataGridView1.Rows[count].Cells[3].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(320, yPos));
                        e.Graphics.DrawString(dataGridView1.Rows[count].Cells[4].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(400, yPos));
                        e.Graphics.DrawString(dataGridView1.Rows[count].Cells[5].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(550, yPos));
                        e.Graphics.DrawString(dataGridView1.Rows[count].Cells[6].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(700, yPos));

                        yPos += 30;
                        count++;
                    }

                    e.HasMorePages = false;
                    currentPage = 1;
                }


                e.Graphics.DrawLine(new Pen(brush), new Point(10, yPos), new Point(paperWidth - 10, yPos));

                e.Graphics.DrawString("Total Amount: " + totalAmount, new Font(font, 25, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 30));
                e.Graphics.DrawString("Discount: " + totalDiscount, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 80));
                e.Graphics.DrawString("Net Amount: " + netAmount, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 110));
                e.Graphics.DrawString("Cash: " + cash, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 140));
                e.Graphics.DrawString("Balance: " + balance, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 170));

                e.Graphics.DrawLine(new Pen(brush), new Point(10, yPos + 200), new Point(paperWidth - 10, yPos + 200));

                e.Graphics.DrawString("Thank you for shopping with us", new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(300, yPos + 230));
                e.Graphics.DrawString("Please come again", new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(370, yPos + 260));
            }
            
            else if (PaperSize == "Thermal")

            {
                //Thermal paper size

                var companyName = dr["Name"].ToString();
                var companyAddress = dr["Address"].ToString();
                var companyContact = dr["Phone"].ToString();
                var companySlogan = dr["Slogan"].ToString();
                var companyLogo = System.Drawing.Image.FromFile(dr["Logo"].ToString());
                var font = dr["Font"].ToString();
                var separator = dr["Separator"].ToString();


                var time = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                var fontSize = 5;
                var cashier = textBox8.Text;
                var receiptNo = textBox1.Text;
                var orderNo = textBox14.Text;
                var customerName = textBox13.Text;
                var customerPhone = textBox10.Text;

                var totalAmount = textBox15.Text;
                var totalDiscount = textBox12.Text;
                var netAmount = textBox9.Text;
                var cash = textBox13.Text;
                var balance = textBox7.Text;

                //thermal printer paper size
                PaperSize paperSize = new PaperSize ("papersize", 285, 600);
                var paperWidth = paperSize.Width;
                var paperHeight = paperSize.Height;
                var brush = Brushes.Purple;


                //design the receipt
                e.Graphics.DrawImage(companyLogo, 10, 10, 30, 30);
                e.Graphics.DrawString(companyName, new Font(font, 11, FontStyle.Bold), brush, new Point(70, 10));
                e.Graphics.DrawString(companyAddress, new Font(font, 5, FontStyle.Regular), brush, new Point(120, 30));
                e.Graphics.DrawString(companyContact, new Font(font, 5, FontStyle.Regular), brush, new Point(125, 40));
                e.Graphics.DrawString(companySlogan, new Font(font, 5, FontStyle.Regular), brush, new Point(115, 50));

                e.Graphics.DrawString(time, new Font(font, 5, FontStyle.Regular), brush, new Point(10, 70));
                e.Graphics.DrawString("Recipt No: " + receiptNo, new Font(font, 5, FontStyle.Regular), brush, new Point(200, 70));

                e.Graphics.DrawLines(new Pen(brush), new Point[] { new Point(10, 80), new Point(paperWidth - 10, 80) });

                //draw the receipt body
                e.Graphics.DrawString("Description", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(10, 90));
                e.Graphics.DrawString("Gross Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(70, 90));
                e.Graphics.DrawString("Quantity", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(120, 90));
                e.Graphics.DrawString("Total Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(150, 90));
                e.Graphics.DrawString("Discount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(200, 90));
                e.Graphics.DrawString("Net Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(235, 90));


                e.Graphics.DrawLine(new Pen(brush), new Point(10, 100), new Point(paperWidth - 10, 100));

                //draw the items to the receipt body using a loop
                int yPos = 110;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //add a length check to the description
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Length > 20)
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 15), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(10, yPos));
                    }
                    else
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(10, yPos));
                    }
                    yPos++;

                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(70, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(120, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[4].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(150, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[5].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(200, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[6].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(235, yPos));


                    yPos += 10;
                }

                e.Graphics.DrawLine(new Pen(brush), new Point(10, yPos), new Point(paperWidth - 10, yPos));
                

                e.Graphics.DrawString("Total Amount: " + totalAmount, new Font(font, 8, FontStyle.Bold), Brushes.Black, new Point(10, yPos + 10));
                e.Graphics.DrawString("Discount: " + totalDiscount, new Font(font, 5, FontStyle.Bold), Brushes.Black, new Point(10, yPos + 25));
                e.Graphics.DrawString("Net Amount: " + netAmount, new Font(font, 5, FontStyle.Bold), Brushes.Black, new Point(10, yPos + 35));
                e.Graphics.DrawString("Cash: " + cash, new Font(font, 5, FontStyle.Bold), Brushes.Black, new Point(10, yPos + 45));
                e.Graphics.DrawString("Balance: " + balance, new Font(font, 5, FontStyle.Bold), Brushes.Black, new Point(10, yPos + 55));

                e.Graphics.DrawLine(new Pen(brush), new Point(10, yPos + 70), new Point(paperWidth - 10, yPos + 70));

                e.Graphics.DrawString("Thank you for shopping with us", new Font(font, 5, FontStyle.Bold), Brushes.Black, new Point(100, yPos + 80));
                e.Graphics.DrawString("Please come again", new Font(font, 5, FontStyle.Bold), Brushes.Black, new Point(120, yPos + 90));

            }

            else
            {
                //Default paper size

                var companyName = dr["Name"].ToString();
                var companyAddress = dr["Address"].ToString();
                var companyContact = dr["Phone"].ToString();
                var companySlogan = dr["Slogan"].ToString();
                var companyLogo = System.Drawing.Image.FromFile(dr["Logo"].ToString());
                var font = dr["Font"].ToString();
                var separator = dr["Separator"].ToString();


                var time = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
                var fontSize = 13;
                var cashier = textBox8.Text;
                var receiptNo = textBox1.Text;
                var orderNo = textBox14.Text;
                var customerName = textBox13.Text;
                var customerPhone = textBox10.Text;

                var totalAmount = textBox15.Text;
                var totalDiscount = textBox12.Text;
                var netAmount = textBox9.Text;
                var cash = textBox13.Text;
                var balance = textBox7.Text;

                //default paper size

                var paperSize = DefaultSize;
                var paperWidth = paperSize.Width;
                var paperHeight = paperSize.Height;
                var brush = Brushes.Purple;


                //design the receipt
                e.Graphics.DrawImage(companyLogo, 10, 10, 100, 100);
                e.Graphics.DrawString(companyName, new Font(font, 35, FontStyle.Bold), brush, new Point(200, 10));
                e.Graphics.DrawString(companyAddress, new Font(font, 15, FontStyle.Regular), brush, new Point(325, 65));
                e.Graphics.DrawString(companyContact, new Font(font, 15, FontStyle.Regular), brush, new Point(350, 90));
                e.Graphics.DrawString(companySlogan, new Font(font, 15, FontStyle.Regular), brush, new Point(300, 120));

                e.Graphics.DrawString("Date: " + time, new Font(font, 15, FontStyle.Regular), brush, new Point(15, 150));
                e.Graphics.DrawString("Recipt No: " + receiptNo, new Font(font, 15, FontStyle.Regular), brush, new Point(600, 150));

                e.Graphics.DrawLines(new Pen(brush), new Point[] { new Point(10, 180), new Point(paperWidth - 10, 180) });

                //draw the receipt body
                e.Graphics.DrawString("Description", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(20, 190));
                e.Graphics.DrawString("Gross Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(170, 190));
                e.Graphics.DrawString("Quantity", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(320, 190));
                e.Graphics.DrawString("Total Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(400, 190));
                e.Graphics.DrawString("Discount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(550, 190));
                e.Graphics.DrawString("Net Amount", new Font(font, fontSize, FontStyle.Bold), Brushes.Black, new Point(700, 190));


                e.Graphics.DrawLine(new Pen(brush), new Point(10, 220), new Point(paperWidth - 10, 220));

                //draw the items to the receipt body using a loop
                int yPos = 240;

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    //add a length check to the description
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString().Length > 20)
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString().Substring(0, 15), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(20, yPos));
                    }
                    else
                    {
                        e.Graphics.DrawString(dataGridView1.Rows[i].Cells[1].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(20, yPos));
                    }
                    yPos++;

                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[2].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(190, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[3].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(340, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[4].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(430, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[5].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(570, yPos));
                    e.Graphics.DrawString(dataGridView1.Rows[i].Cells[6].Value.ToString(), new Font(font, fontSize, FontStyle.Regular), Brushes.Black, new Point(720, yPos));


                    yPos += 30;
                }

                e.Graphics.DrawLine(new Pen(brush), new Point(10, yPos), new Point(paperWidth - 10, yPos));

                e.Graphics.DrawString("Total Amount: " + totalAmount, new Font(font, 25, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 30));
                e.Graphics.DrawString("Discount: " + totalDiscount, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 80));
                e.Graphics.DrawString("Net Amount: " + netAmount, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 110));
                e.Graphics.DrawString("Cash: " + cash, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 140));
                e.Graphics.DrawString("Balance: " + balance, new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(20, yPos + 170));

                e.Graphics.DrawLine(new Pen(brush), new Point(10, yPos + 200), new Point(paperWidth - 10, yPos + 200));

                e.Graphics.DrawString("Thank you for shopping with us", new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(300, yPos + 230));
                e.Graphics.DrawString("Please come again", new Font(font, 15, FontStyle.Bold), Brushes.Black, new Point(370, yPos + 260));
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox13.Text == "")
            {
                MessageBox.Show("Please enter the amount paid");
            }
            else
            {
                balance();

                //preview the receipt
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.ShowDialog();
              

            }
           
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //print the receipt
            printDocument1.Print();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            printDocument1.Print();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=HUBMAN;Initial Catalog=DevzecPrinting;Integrated Security=True");

            string itemCode = comboBox2.Text;
            try
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("Select * from Product where itemCode ='" + itemCode + "'", con);

                SqlDataReader myR = cmd.ExecuteReader();
                if (myR.HasRows)
                {
                    while (myR.Read())
                    {
                        textBox3.Text = myR["description"].ToString();
                        textBox4.Text = myR["grossAmount"].ToString();
                        textBox6.Text = myR["discount"].ToString();
                    }
                }
                else
                {
                    MessageBox.Show("Sorry, No record from this Item code..");
                }

                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                con.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           //clear the button clicked row in the datagridview    
           dataGridView1.Rows.RemoveAt(e.RowIndex);
         
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            { 
            var orderNo = int.Parse(textBox14.Text);
            var receiptNo = int.Parse(textBox1.Text);
            var customerNo = int.Parse(textBox11.Text);
            var paymentType = comboBox1.Text;
            var cashier = textBox8.Text;
            var date = DateTime.Now;
            var totalAmount = int.Parse(textBox15.Text);
            var discount = int.Parse(textBox12.Text);
            var netAmount = int.Parse(textBox9.Text);
            var cash = int.Parse(textBox13.Text);
            var balance = int.Parse(textBox7.Text);
            var Status = "Complete";

            //insert the data to the database
            SqlConnection con = new SqlConnection(@"Data Source=HUBMAN;Initial Catalog=DevzecPrinting;Integrated Security=True");
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Orders(orderNo,receiptNo,date,customerNo,paymentType,cashier,totalAmount,discountAmount,netAmount,cashAmount,balanceAmount,status) VALUES('" + orderNo + "','" + receiptNo + "','" + date + "','" + customerNo + "','" + paymentType + "','" + cashier + "','" + totalAmount + "','" + discount + "','" + netAmount + "','" + cash + "','" + balance + "','" + Status + "')", con);
            cmd.ExecuteNonQuery();

                MessageBox.Show("Order saved successfully");

                con.Close();

            clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                var orderNo = int.Parse(textBox14.Text);
                var receiptNo = int.Parse(textBox1.Text);
                var customerNo = int.Parse(textBox11.Text);
                var paymentType = comboBox1.Text;
                var cashier = textBox8.Text;
                var date = DateTime.Now;
                var totalAmount = int.Parse(textBox15.Text);
                var discount = int.Parse(textBox12.Text);
                var netAmount = int.Parse(textBox9.Text);
                var cash = int.Parse(textBox13.Text);
                var balance = int.Parse(textBox7.Text);
                var Status = "Hold";

                //insert the data to the database
                SqlConnection con = new SqlConnection(@"Data Source=HUBMAN;Initial Catalog=DevzecPrinting;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO Orders(orderNo,receiptNo,date,customerNo,paymentType,cashier,totalAmount,discountAmount,netAmount,cashAmount,balanceAmount,status) VALUES('" + orderNo + "','" + receiptNo + "','" + date + "','" + customerNo + "','" + paymentType + "','" + cashier + "','" + totalAmount + "','" + discount + "','" + netAmount + "','" + cash + "','" + balance + "','" + Status + "')", con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Order saved successfully");



                con.Close();

                clearAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }

        }
    }
}