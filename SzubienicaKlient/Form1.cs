using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SzubienicaKlient
{
    public partial class Form1 : Form
    {

        NetworkStream stream;
        StreamReader reader;
        StreamWriter writer;
        int tura=0;

        //Graphics g;
        //Graphics g2;
        //int b_w = 10, b_h = 10;
        //Brush bc;
        //Brush bb;
        

        public Form1()
        {
            InitializeComponent();
            // g = panel1.CreateGraphics();
            //// g2 = panel2.CreateGraphics();
            // bc = new SolidBrush(Color.Red);
            // bb = new SolidBrush(Color.White);
            
     

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string text = textBox1.Text;
                textBox1.Text = "";
                // string do_wyslania= textBox2.Text+": "+text+ Environment.NewLine;
                string do_wyslania = text;
                writer.WriteLine(do_wyslania);
                string do_odbioru = reader.ReadLine();

                label2.Text = do_odbioru;
                label2.Refresh();
                richTextBox1.Text += reader.ReadLine() + Environment.NewLine;
                richTextBox1.Refresh();
                tura++;
                label3.Text = Convert.ToString(tura);
                czekaj();
                //   do_odbioru = reader.ReadLine();
                // richTextBox1.Text += "Ja: "+do_odbioru + Environment.NewLine;
                //  richTextBox1.Text += "Oczekiwanie na ruch pozostalych graczy"+ Environment.NewLine;
                //string moja_kolej = reader.ReadLine();
                // richTextBox1.Text += moja_kolej+Environment.NewLine;
            }

            catch
            {
                MessageBox.Show("Polaczenie z serwerem lub innymi klientami zostalo utracone. Prograam zostanie zamkniety.");
                // Application.Exit();
                this.Close();
                Environment.Exit(0);

            }
        }

        private void czekaj()
        {
            try
            {
                string moja_kolej = reader.ReadLine();
                richTextBox1.Text += moja_kolej + Environment.NewLine;
            }
            catch
            {
                MessageBox.Show("Polaczenie z serwerem lub innymi klientami zostalo utracone. Program zostanie zamkniety.");
                // Application.Exit();
                this.Close();
                Environment.Exit(0);

            }

            //richTextBox1.Refresh();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = "0";
            richTextBox1.Text = "";
            string nazwa = textBox2.Text;
            string ip = textBox3.Text;
            int port = 1234;
            TcpClient client = new TcpClient(ip, port);
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            label2.Text= reader.ReadLine();
            string moja_kolej = reader.ReadLine();
            writer.WriteLine(nazwa);
            richTextBox1.Text += moja_kolej + Environment.NewLine;
            label2.Refresh();
            richTextBox1.Refresh();
            
            czekaj();
        }

        //private void panel1_MouseMove(object sender, MouseEventArgs e)
        //{
        //    int x = 0;
        //    int w = 0, h = 0;
        //    if (e.Button == MouseButtons.Left) { g.FillEllipse(bc, e.X, e.Y, b_w, b_h); x = 2; }
        //    else if (e.Button == MouseButtons.Right) { g.FillEllipse(bb, e.X, e.Y, b_w, b_h); x = 1; }
        //}
    }
}
