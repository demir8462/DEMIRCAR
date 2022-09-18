using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
namespace RentACarServer
{
    public class Server
    {
        TcpListener listener;
        StreamWriter wr;
        StreamReader rd;
        Thread baglantiTHR,clientControlTHR;
        public List<Client> clients = new List<Client>();
        public List<Client> oluclients = new List<Client>();
        public void startListen(int port)
        {
            listener = new TcpListener(port);
            listener.Start();
            baglantiTHR = new Thread(new ThreadStart(baglantiKabulEt));
            baglantiTHR.Start();
            clientControlTHR = new Thread(new ThreadStart(clientKontrolEt));
            clientControlTHR.Start();
        }
        void baglantiKabulEt()
        {
            while(true)
            {
                clients.Add(new Client(listener.AcceptSocket()));
            }
        }
        void clientKontrolEt()
        {
            while(true)
            {
                foreach (Client item in clients)
                {
                    if (!item.connected)
                        oluclients.Add(item);
                }
                foreach (Client item in oluclients)
                {
                    clients.Remove(item);
                }
                Thread.Sleep(5000);
            }
        }
    }
}
