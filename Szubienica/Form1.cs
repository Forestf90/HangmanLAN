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
        TcpListener listener ;
        TcpClient client ;
        NetworkStream stream;
        StreamWriter writer ;
        StreamReader reader;
        string haslo;
        string haslo_odgadniete;
        public Form1()
        {


            InitializeComponent();

        }

 
        private void button1_Click(object sender, EventArgs e)
        {
            haslo = textBox2.Text;
            poczatek();
            port = 1234;
           // string ip = textBox1.Text;
            listener = new TcpListener(IPAddress.Any, port);

            listener.Start();

            client = listener.AcceptTcpClient();
           // client = listener.AcceptTcpClient();
           // client = listener.AcceptTcpClient();
            stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };
            reader = new StreamReader(stream, Encoding.ASCII);

            writer.WriteLine(haslo_odgadniete);


            var dostalem = "";
            while (dostalem != null)
            {
            dostalem = reader.ReadLine();
                if(dostalem!="")
                {
                    char temp;
                    Char.TryParse(dostalem, out temp);
                    if (temp != '\0') { writer.WriteLine(sprawdz(Convert.ToChar(dostalem))); writer.WriteLine(dostalem); }
                    else if (dostalem.GetType() == typeof(string))
                    {
                        writer.WriteLine(haslo_odgadniete);
                        if (dostalem == haslo) writer.WriteLine("Gratuluje wygrales");
                        else writer.WriteLine("Bledne haslo przegrales");
                    }
                    

                }
            

                
               // ThreadPool.QueueUserWorkItem(ThreadProc, client);
                //richTextBox1.Text += dostalem;
            }
        }
       
        public void poczatek()
        {
            StringBuilder temp= new StringBuilder(haslo);

            for(int i=0; i <haslo.Length; i++)
            {
                temp[i] = '_';

            }
            haslo_odgadniete = Convert.ToString(temp);
        }

        public string sprawdz(char litera)
        {
            StringBuilder temp = new StringBuilder(haslo);
            StringBuilder temp2 = new StringBuilder(haslo_odgadniete);

            for (int i = 0; i < haslo.Length; i++)
            {
                if (temp[i] == litera) temp2[i] = litera;

            }
            haslo_odgadniete = Convert.ToString(temp2);

            return haslo_odgadniete;
        }
    }
}
