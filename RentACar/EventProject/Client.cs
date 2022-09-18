using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using RENTACARKUTUPHANE;

namespace RentACar
{
    public class Client
    {
        TcpClient client;
        NetworkStream stream;
        StreamWriter writer;
        StreamReader reader;
        public BinaryFormatter bf;
        public IletisimPaketi paket;
        public Client(string ip="127.0.0.1",int port=2525)
        {
            try
            {
                client = new TcpClient(ip, port);
                if (client.Connected)
                {
                    stream = client.GetStream();
                    writer = new StreamWriter(stream);
                    reader = new StreamReader(stream);
                    bf = new BinaryFormatter();
                    paket = new IletisimPaketi();

                }
            }catch(Exception e)
            {
                MessageBox.Show("SUNUCULAR KAPALI !");
                Environment.Exit(0);
            }
        }
        public void GirisYap(User user)
        {
            paket.tip = IletisimPaketi.PAKETTIPI.LOGIN;
            paket.user = user;
            bf.Serialize(stream,paket);
            
        }
        public void KayitOl(User user)
        {
            paket.tip = IletisimPaketi.PAKETTIPI.SIGNUP;
            paket.user = user;
            bf.Serialize(stream, paket);
        }
        public void paketAl()
        {
            paket =(IletisimPaketi)bf.Deserialize(stream);
        }
        public void paketYolla(IletisimPaketi paket)
        {
            bf.Serialize(stream, paket);
        }
    }
    
}
