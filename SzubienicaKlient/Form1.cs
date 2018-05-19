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


        Graphics g;
        Graphics g2;
        int b_w = 10, b_h = 10;
        Brush bc;
        Brush bb;
        

        public Form1()
        {
            InitializeComponent();
            g = panel1.CreateGraphics();
           // g2 = panel2.CreateGraphics();
            bc = new SolidBrush(Color.Red);
            bb = new SolidBrush(Color.White);

     

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            textBox1.Text = "";
            // string do_wyslania= textBox2.Text+": "+text+ Environment.NewLine;
            string do_wyslania = text;
            //Console.Write("Enter to send: ");
            // string lineToSend = Console.ReadLine();
           // Console.WriteLine("Sending to server: " + lineToSend);
            writer.WriteLine(do_wyslania);
            string do_odbioru = reader.ReadLine();
            label2.Text = do_odbioru;
            do_odbioru = reader.ReadLine();
            richTextBox1.Text += do_odbioru + Environment.NewLine;
            //  if (do_odbioru == textBox2.Text+": rysuje") panel1.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ip = textBox3.Text;
            int port = 1234;
            TcpClient client = new TcpClient(ip, port);
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            label2.Text= reader.ReadLine();

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            int x = 0;
            int w = 0, h = 0;
            if (e.Button == MouseButtons.Left) { g.FillEllipse(bc, e.X, e.Y, b_w, b_h); x = 2; }
            else if (e.Button == MouseButtons.Right) { g.FillEllipse(bb, e.X, e.Y, b_w, b_h); x = 1; }
        }
    }
}
