using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Szubienica
{
    public partial class Form1 : Form
    {


        int port;
        TcpListener listener;
        TcpClient client;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;

        TcpListener listener1;
        TcpClient client1;
        NetworkStream stream1;
        StreamWriter writer1;
        StreamReader reader1;



        TcpListener listener2;
        TcpClient client2;
        NetworkStream stream2;
        StreamWriter writer2;
        StreamReader reader2;

        string haslo;
        string[] haslo_odgadniete;
        bool[] zwyciezca;
        string[] gracze;


        public Form1()
        {


            InitializeComponent();
            haslo_odgadniete = new string[3];
            zwyciezca = new bool[3];
            gracze = new string[3];
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                haslo = textBox2.Text;
                poczatek();
                port = 1234;
                // string ip = textBox1.Text;
                listener = new TcpListener(IPAddress.Any, port);
                label1.Text = "Oczekiwanie na trzech graczy";
                label1.Refresh();
                listener.Start();

                client = listener.AcceptTcpClient();
                stream = client.GetStream();
                writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
                reader = new StreamReader(stream, Encoding.ASCII);

                writer.WriteLine(haslo_odgadniete[0]);
                writer.WriteLine("Oczekiwanie na dwoch graczy");
                gracze[0] = reader.ReadLine();
                listener.Stop();
                /////////////////////////////////////////////


                listener1 = new TcpListener(IPAddress.Any, port);
                label1.Text = "Oczekiwanie na dwoch graczy";
                label1.Refresh();
                listener1.Start();

                client1 = listener1.AcceptTcpClient();
                stream1 = client1.GetStream();
                writer1 = new StreamWriter(stream1, Encoding.ASCII) { AutoFlush = true };
                reader1 = new StreamReader(stream1, Encoding.ASCII);

                writer1.WriteLine(haslo_odgadniete[1]);
                writer1.WriteLine("Oczekiwanie na jednego gracza");
                gracze[1] = reader1.ReadLine();
                listener1.Stop();
                //////////////////////////////////////

                listener2 = new TcpListener(IPAddress.Any, port);
                label1.Text = "Oczekiwanie na jednego graczy";
                label1.Refresh();
                listener2.Start();

                client2 = listener2.AcceptTcpClient();
                stream2 = client2.GetStream();
                writer2 = new StreamWriter(stream2, Encoding.ASCII) { AutoFlush = true };
                reader2 = new StreamReader(stream2, Encoding.ASCII);

                writer2.WriteLine(haslo_odgadniete[2]);
                writer2.WriteLine("Gra rozpoczeta");
                gracze[2] = reader2.ReadLine();
                listener2.Stop();
                //////////////////////////////////////////

                label1.Text = "Gra rozpoczeta";
                label1.Refresh();


                var dostalem = "";
                while (dostalem != null)
                {
                    listener.Start();
                    writer.WriteLine("Twoj ruch");
                    dostalem = reader.ReadLine();
                    if (dostalem != "")
                    {
                        char temp;
                        Char.TryParse(dostalem, out temp);
                        if (temp != '\0') { writer.WriteLine(sprawdz(Convert.ToChar(dostalem), 0)); writer.WriteLine(dostalem); }
                        else if (dostalem.GetType() == typeof(string))
                        {
                            writer.WriteLine(haslo_odgadniete[0]);
                            if (dostalem == haslo) { writer.WriteLine("Gratuluje wygrales"); zwyciezca[0] = true; }
                            else writer.WriteLine("Bledne haslo przegrales");
                        }


                    }
                    listener.Stop();
                    ///////////////////////////////////////////////////

                    listener1.Start();
                    writer1.WriteLine("Twoj ruch");
                    dostalem = reader1.ReadLine();
                    if (dostalem != "")
                    {
                        char temp;
                        Char.TryParse(dostalem, out temp);
                        if (temp != '\0') { writer1.WriteLine(sprawdz(Convert.ToChar(dostalem), 1)); writer1.WriteLine(dostalem); }
                        else if (dostalem.GetType() == typeof(string))
                        {
                            writer1.WriteLine(haslo_odgadniete[1]);
                            if (dostalem == haslo) { writer1.WriteLine("Zgadles haslo"); zwyciezca[1] = true; }
                            else writer1.WriteLine("Bledne haslo ");
                        }


                    }


                    listener1.Stop();
                    /////////////////////////////////////////////////////////

                    listener2.Start();
                    writer2.WriteLine("Twoj ruch");
                    dostalem = reader2.ReadLine();
                    if (dostalem != "")
                    {
                        char temp;
                        Char.TryParse(dostalem, out temp);
                        if (temp != '\0') { writer2.WriteLine(sprawdz(Convert.ToChar(dostalem), 2)); writer2.WriteLine(dostalem); }
                        else if (dostalem.GetType() == typeof(string))
                        {
                            writer2.WriteLine(haslo_odgadniete[2]);
                            if (dostalem == haslo) { writer2.WriteLine("Zgadles haslo"); zwyciezca[2] = true; }
                            else writer2.WriteLine("Bledne haslo");
                        }


                    }
                    listener2.Stop();
                    sprawdz();
                }
            }
            catch
            {
                MessageBox.Show("Polaczenie z klientami zostalo utracone. Program zostanie zamkniety");
                //Application.Exit();
                this.Close();
                Environment.Exit(0);
            }
        }

        public void poczatek()
        {
            for (int k = 0; k < haslo_odgadniete.Length; k++)
            {
                StringBuilder temp = new StringBuilder(haslo);

                for (int i = 0; i < haslo.Length; i++)
                {
                    temp[i] = '_';

                }
                haslo_odgadniete[k] = Convert.ToString(temp);
            }
        }

        public string sprawdz(char litera, int index)
        {
            StringBuilder temp = new StringBuilder(haslo);
            StringBuilder temp2 = new StringBuilder(haslo_odgadniete[index]);

            for (int i = 0; i < haslo.Length; i++)
            {
                if (temp[i] == litera) temp2[i] = litera;

            }
            haslo_odgadniete[index] = Convert.ToString(temp2);

            return haslo_odgadniete[index];
        }

        public void sprawdz()
            {
            string zwyciezcy="";
            for(int i=0; i<zwyciezca.Length; i++)
            {
                if (zwyciezcy != "" && zwyciezca[i]) zwyciezcy += " oraz " + gracze[i];
                else if (zwyciezca[i])
                {
                    zwyciezcy = gracze[i];
                }
                   
            }
            if (zwyciezcy != "")
            {
                label1.Text = "Gra skonczona";
                label1.Refresh();

                listener.Start();
                writer.WriteLine("Zwyciezca zostal " + zwyciezcy + " haslo to " + haslo);
                listener.Stop();
                listener1.Start();
                writer1.WriteLine("Zwyciezca zostal " + zwyciezcy + " haslo to " + haslo);
                listener1.Stop();
                listener2.Start();
                writer2.WriteLine("Zwyciezca zostal " + zwyciezcy + " haslo to " + haslo);
                listener2.Stop();

                DialogResult result = MessageBox.Show("Czy chcesz rozpoczac od nowa ?",
                "Koniec gry",
                 MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {
                    Close();
                }
                if (result == DialogResult.Yes)
                {
                    
                    Application.Restart();
                    Application.Exit();
                    //this.Close();
                    //Application.Exit();
                    //Environment.ExitCode = 1;
                   // Environment.Exit(1);
                    Environment.Exit(0);
                   // Close();
                }
            }

        }
   
    }
}
