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
using System.Net.Security;


using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SzubienicaKlient
{
    public partial class Form1 : Form
    {

        SslStream stream;
        //StreamReader reader;
        //StreamWriter writer;
        int tura=0;
        byte[] message;
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

            //RemoteCertificateValidationCallback cotojest = new RemoteCertificateValidationCallback(true);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string text = textBox1.Text;
                textBox1.Text = "";
                // string do_wyslania= textBox2.Text+": "+text+ Environment.NewLine;
                string do_wyslania = text;
                message = Encoding.UTF8.GetBytes(do_wyslania + "<EOF>");
                stream.Write(message);
                string do_odbioru = ReadMessage(stream);

                label2.Text = do_odbioru;
                label2.Refresh();
                richTextBox1.Text += ReadMessage(stream) + Environment.NewLine;
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
                string moja_kolej = ReadMessage(stream);
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
            
            stream = new SslStream(client.GetStream(), false , new RemoteCertificateValidationCallback(ValidateCert));
            stream.AuthenticateAsClient("Szubienica");

           // reader = new StreamReader(stream);
           // writer = new StreamWriter(stream) { AutoFlush = true };

            label2.Text= ReadMessage(stream);
            string moja_kolej = ReadMessage(stream);
            message = Encoding.UTF8.GetBytes(nazwa+ "<EOF>");
            stream.Write(message);


            message = Encoding.UTF8.GetBytes(GetLocalIPAddress() + "<EOF>");
            stream.Write(message);

            //writer.WriteLine(nazwa);
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
        static string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the server.
            // The end of the message is signaled using the
            // "<EOF>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for EOF.
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            string koncowy= messageData.ToString();
            return koncowy.Remove(koncowy.Length -5);
        }



        public static bool ValidateCert(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // Uncomment this lines to disallow untrusted certificates.
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //    return true;
            //else
            //    return false;

            return true; // Allow untrusted certificates.
        }
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }



}
