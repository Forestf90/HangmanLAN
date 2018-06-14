using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Text.RegularExpressions;

using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Szubienica
{
    public partial class Form1 : Form
    {


        int port;
        int klienci = 3;
        TcpListener[] listener;
        TcpClient[] client;
        SslStream[] stream;
        //  StreamWriter writer;
        //   StreamReader reader;

        //TcpListener listener1;
        //TcpClient client1;
        //SslStream stream1;
        //  StreamWriter writer1;
        //  StreamReader reader1;



        //TcpListener listener2;
        //TcpClient client2;
        //SslStream stream2;
        // StreamWriter writer2;
        // StreamReader reader2;

        string haslo;
        string[] haslo_odgadniete;
        bool[] zwyciezca;
        string[] gracze;
        byte[] message;
        bool[] offline;
        string[] adresy;

        static X509Certificate2 serverCertificate = null;

        public Form1()
        {


            InitializeComponent();
            //haslo_odgadniete = new string[klienci];
            //zwyciezca = new bool[klienci];
            //gracze = new string[klienci];

          

            serverCertificate = new X509Certificate2(@"server.pfx" , "instant");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            klienci = Convert.ToInt32(textBox1.Text);

            listener = new TcpListener[klienci];
            client = new TcpClient[klienci];
            stream = new SslStream[klienci];

            haslo_odgadniete = new string[klienci];
            zwyciezca = new bool[klienci];
            gracze = new string[klienci];
            offline = new bool[klienci];
            adresy = new string[klienci];


            try
            {
                haslo = textBox2.Text;
                poczatek();
                port = 1234;
                for (int i = 0; i < klienci; i++)
                {
                    // string ip = textBox1.Text;
                    listener[i] = new TcpListener(IPAddress.Any, port);
                    label1.Text = "Oczekiwanie na "+Convert.ToString(klienci-i)+" graczy";
                    label1.Refresh();
                    listener[i].Server.SendTimeout = 30000;
                    listener[i].Server.ReceiveTimeout = 30000;
                    listener[i].Start();

                    client[i] = listener[i].AcceptTcpClient();
                    stream[i] = new SslStream(client[i].GetStream());
                    stream[i].AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                    // writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
                    //reader = new StreamReader(stream, Encoding.ASCII);
                    message = Encoding.UTF8.GetBytes(haslo_odgadniete[i] + "<EOF>");
                    stream[i].Write(message);

                    message = Encoding.UTF8.GetBytes("Oczekiwanie na " + Convert.ToString(klienci - i) + " graczy<EOF>");
                    stream[i].Write(message);


                    gracze[i] = ReadMessage(stream[i]);
                    adresy[i] = ReadMessage(stream[i]);

                   
                    listener[i].Stop();
                }
                /////////////////////////////////////////////


                //listener[1] = new TcpListener(IPAddress.Any, port);
                //label1.Text = "Oczekiwanie na dwoch graczy";
                //label1.Refresh();
                //listener[1].Start();

                //client[1] = listener[1].AcceptTcpClient();
                //stream[1] = new SslStream(client[1].GetStream());
                //stream[1].AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                ////writer1 = new StreamWriter(stream1, Encoding.ASCII) { AutoFlush = true };
                ////reader1 = new StreamReader(stream1, Encoding.ASCII);

                //message = Encoding.UTF8.GetBytes(haslo_odgadniete[1] + "<EOF>");
                //stream[1].Write(message);

                //message = Encoding.UTF8.GetBytes("Oczekiwanie na jednego gracza<EOF>");
                //stream[1].Write(message);


                //gracze[1] = ReadMessage(stream[1]);
                //listener[1].Stop();
                ////////////////////////////////////////

                //listener[2] = new TcpListener(IPAddress.Any, port);
                //label1.Text = "Oczekiwanie na jednego graczy";
                //label1.Refresh();
                //listener[2].Start();

                //client[2] = listener[2].AcceptTcpClient();
                //stream[2] = new SslStream(client[2].GetStream());
                //stream[2].AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                //// writer2 = new StreamWriter(stream2, Encoding.ASCII) { AutoFlush = true };
                ////reader2 = new StreamReader(stream2, Encoding.ASCII);

                //message = Encoding.UTF8.GetBytes(haslo_odgadniete[2] + "<EOF>");
                //stream[2].Write(message);

                //message = Encoding.UTF8.GetBytes("Gra rozpoczeta<EOF>");
                //stream[2].Write(message);


                //gracze[2] = ReadMessage(stream[2]);
                //listener[2].Stop();
                //////////////////////////////////////////

                label1.Text = "Gra rozpoczeta";
                label1.Refresh();


                var dostalem = "";
                while (dostalem != null)
                {
                    for (int i = 0; i < klienci; i++)
                    {
                       // ponownie:
                        if (!offline[i])
                        {
                            try
                            {
                                listener[i].Start();
                                //writer.WriteLine("Twoj ruch");
                                message = Encoding.UTF8.GetBytes("Twoj ruch<EOF>");
                                stream[i].Write(message);
                                dostalem = ReadMessage(stream[i]);
                                if (dostalem != "")
                                {
                                    char temp;
                                    Char.TryParse(dostalem, out temp);
                                    if (temp != '\0')
                                    {
                                        //writer.WriteLine(sprawdz(Convert.ToChar(dostalem), 0));
                                        //writer.WriteLine(dostalem);

                                        message = Encoding.UTF8.GetBytes(sprawdz(Convert.ToChar(dostalem), i) + "<EOF>");
                                        stream[i].Write(message);
                                        message = Encoding.UTF8.GetBytes(dostalem + "<EOF>");
                                        stream[i].Write(message);
                                    }
                                    else if (dostalem.GetType() == typeof(string))
                                    {
                                        //writer.WriteLine(haslo_odgadniete[0]);
                                        message = Encoding.UTF8.GetBytes(haslo_odgadniete[i] + "<EOF>");
                                        stream[i].Write(message);
                                        if (dostalem == haslo)
                                        {
                                            message = Encoding.UTF8.GetBytes("Zgadles!<EOF>");
                                            stream[i].Write(message);
                                            zwyciezca[i] = true;
                                        }
                                        else
                                        {
                                            message = Encoding.UTF8.GetBytes("Bledne haslo<EOF>");
                                            stream[i].Write(message);
                                        }
                                    }


                                }
                                listener[i].Stop();
                            }
                            catch
                            {
                                wywalony(i);
                               // goto ponownie;
                                i--;
                            }
                        }
                    }

                    ///////////////////////////////////////////////////

                    //listener[1].Start();
                    //message = Encoding.UTF8.GetBytes("Twoj ruch<EOF>");
                    //stream[1].Write(message);
                    //dostalem = ReadMessage(stream[1]);
                    //if (dostalem != "")
                    //{
                    //    char temp;
                    //    Char.TryParse(dostalem, out temp);
                    //    if (temp != '\0')
                    //    {
                    //        //writer1.WriteLine(sprawdz(Convert.ToChar(dostalem), 1));
                    //       // writer1.WriteLine(dostalem);

                    //        message = Encoding.UTF8.GetBytes(sprawdz(Convert.ToChar(dostalem), 1) + "<EOF>");
                    //        stream[1].Write(message);
                    //        message = Encoding.UTF8.GetBytes(dostalem + "<EOF>");
                    //        stream[1].Write(message);
                    //    }
                    //    else if (dostalem.GetType() == typeof(string))
                    //    {
                    //        //writer.WriteLine(haslo_odgadniete[0]);
                    //        message = Encoding.UTF8.GetBytes(haslo_odgadniete[1] + "<EOF>");
                    //        stream[1].Write(message);
                    //        if (dostalem == haslo)
                    //        {
                    //            message = Encoding.UTF8.GetBytes("Zgadles!<EOF>");
                    //            stream[1].Write(message);
                    //            zwyciezca[1] = true;
                    //        }
                    //        else
                    //        {
                    //            message = Encoding.UTF8.GetBytes("Bledne haslo<EOF>");
                    //            stream[1].Write(message);
                    //        }
                    //    }


                    //}


                    //listener[1].Stop();
                    ///////////////////////////////////////////////////////////

                    //listener[2].Start();
                    //message = Encoding.UTF8.GetBytes("Twoj ruch<EOF>");
                    //stream[2].Write(message);
                    //dostalem = ReadMessage(stream[2]);
                    //if (dostalem != "")
                    //{
                    //    char temp;
                    //    Char.TryParse(dostalem, out temp);
                    //    if (temp != '\0')
                    //    {
                    //        //writer2.WriteLine(sprawdz(Convert.ToChar(dostalem), 2));
                    //        //writer2.WriteLine(dostalem);

                    //        message = Encoding.UTF8.GetBytes(sprawdz(Convert.ToChar(dostalem), 2) + "<EOF>");
                    //        stream[2].Write(message);
                    //        message = Encoding.UTF8.GetBytes(dostalem + "<EOF>");
                    //        stream[2].Write(message);
                    //    }
                    //    else if (dostalem.GetType() == typeof(string))
                    //    {
                    //        //writer.WriteLine(haslo_odgadniete[0]);
                    //        message = Encoding.UTF8.GetBytes(haslo_odgadniete[2] + "<EOF>");
                    //        stream[2].Write(message);
                    //        if (dostalem == haslo)
                    //        {
                    //            message = Encoding.UTF8.GetBytes("Zgadles!<EOF>");
                    //            stream[2].Write(message);
                    //            zwyciezca[2] = true;
                    //        }
                    //        else
                    //        {
                    //            message = Encoding.UTF8.GetBytes("Bledne haslo<EOF>");
                    //            stream[2].Write(message);
                    //        }
                    //    }


                    //}
                    //listener[2].Stop();
                    sprawdz();
                    label1.Text = "Gra trwa";
                    label1.Refresh();
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

                message = Encoding.UTF8.GetBytes("Zwyciezca zostal " + zwyciezcy + " haslo to " + haslo+"<EOF>");
                //stream.Write(message);
                for (int i = 0; i < klienci; i++)
                {
                    listener[i].Start();
                    stream[i].Write(message);
                    listener[i].Stop();
                }

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

        private void wywalony(int index)
        {
            DialogResult result = MessageBox.Show("Gracz "+gracze[index]+" utracil polaczenie . Czy chcez na niego zaczekac ? ",
                "Utrata polaczenia",
                 MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                listener[index].Stop();
                client[index].Close();
                stream[index].Close();
                offline[index] = true;
            }
            if (result == DialogResult.Yes)
            {
                listener[index].Stop();
                client[index].Close();
                stream[index].Close();

                        listener[index] = new TcpListener(IPAddress.Any, port);
                        //label1.Text = gracze[index]+" powrocil";
                        //label1.Refresh();
                        listener[index].Start();

                        client[index] = listener[index].AcceptTcpClient();
                        stream[index] = new SslStream(client[index].GetStream());
                        stream[index].AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
                        label1.Text = gracze[index] + " powrocil";
                        label1.Refresh();
                        // writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
                        //reader = new StreamReader(stream, Encoding.ASCII);
                        message = Encoding.UTF8.GetBytes(haslo_odgadniete[index] + "<EOF>");
                        stream[index].Write(message);

                        message = Encoding.UTF8.GetBytes("Gra rozpoczeta<EOF>");
                        stream[index].Write(message);


                        gracze[index] = ReadMessage(stream[index]);
                        adresy[index] = ReadMessage(stream[index]);
                        listener[index].Stop();

               
                

            }

        }

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

            string koncowy = messageData.ToString();
            return koncowy.Remove(koncowy.Length - 5);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            Regex regex = new Regex("[^0-9]+"); ;
            //What should I write here?
            if (!regex.Match(txtBox.Text).Success)
            {
                e.Cancel = true;
            }
            e.Cancel = false;

        }
    }
}
