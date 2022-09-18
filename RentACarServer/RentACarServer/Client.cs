using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using RENTACARKUTUPHANE;

namespace RentACarServer
{
    public class Client
    {
        StreamWriter wr;
        StreamReader rd;
        Socket soket;
        NetworkStream stream;
        Thread THRpaketAl;
        BinaryFormatter bf;
        DataBaseManager dbmanager = new DataBaseManager();
        public bool connected,giris;
        User user;
        public Client(Socket soket)
        {
            this.soket = soket;
            stream = new NetworkStream(soket);
            wr = new StreamWriter(stream);
            rd = new StreamReader(stream);
            bf = new BinaryFormatter();
            connected = true;
            THRpaketAl = new Thread(new ThreadStart(paketAl));
            THRpaketAl.Start();
        }
        public void paketAl()
        {
            while(soket.Connected)
            {
                try
                {
                    object paket = bf.Deserialize(stream);
                    if (paket != null)
                    {
                        IletisimPaketi ipaket = (IletisimPaketi)paket;
                        if (ipaket.tip == IletisimPaketi.PAKETTIPI.LOGIN)
                        {
                            if(dbmanager.kullaniciVarmi(ipaket.user))
                            {
                                if (dbmanager.sifreKontrol(ipaket.user))
                                {
                                    ipaket.tip = IletisimPaketi.PAKETTIPI.CEVAP;
                                    ipaket.cevap = true;
                                    ipaket.detay = "Giriş Başarılı !";
                                    user = dbmanager.getUserByTc(ipaket.user.Tc);
                                    MessageBox.Show(user.Tc);
                                    giris = true;
                                    dbmanager.setUserStatus(true, ipaket.user.Tc);
                                }
                                else
                                {
                                    ipaket.tip = IletisimPaketi.PAKETTIPI.CEVAP;
                                    ipaket.cevap = false;
                                    ipaket.detay = "Hatalı Şifre !";
                                }
                            }
                            else
                            {
                                ipaket.tip = IletisimPaketi.PAKETTIPI.CEVAP;
                                ipaket.cevap = false;
                                ipaket.detay = "Kullanıcı Yok !";
                            }
                        }else if(ipaket.tip == IletisimPaketi.PAKETTIPI.SIGNUP)
                        {
                            if(!dbmanager.kullaniciVarmi(ipaket.user))
                            {
                                dbmanager.kayitEkle(ipaket.user);
                                ipaket.tip = IletisimPaketi.PAKETTIPI.CEVAP;
                                ipaket.cevap = true;
                                ipaket.detay = "Başarılı Kayıt !";
                            }
                            else
                            {
                                ipaket.tip = IletisimPaketi.PAKETTIPI.CEVAP;
                                ipaket.cevap = false;
                                ipaket.detay = "Kullanıcı Mevcut !";
                            }
                        }else if(ipaket.tip == IletisimPaketi.PAKETTIPI.KIRAISTEK)
                        {
                            ipaket.cevap = dbmanager.ArabaKirala(user.Tc, user.Telno, ipaket.ID, ipaket.teslim, ipaket.geriteslim, ipaket.tahsilat);
                            ipaket.detay = "Araç kiralandı !";
                        }
                        if(ipaket.tip==IletisimPaketi.PAKETTIPI.CEVAP)
                            bf.Serialize(stream, ipaket);
                    }
                    Thread.Sleep(500);
                }catch(Exception e)
                {
                    clientOl();
                }
            }
        }
        public void clientOl()
        {
            MessageBox.Show("Client düştü !");
            wr.Close();
            rd.Close();
            soket.Close();
            stream.Close();
            connected = false;
            dbmanager.setUserStatus(false, user.Tc);
        }
        public void paketYolla(IletisimPaketi ipaket)
        {
            try
            {
                bf.Serialize(stream, ipaket);
            }catch(Exception e)
            {
                MessageBox.Show("Paket yollanırken sorun oluştu !");
                clientOl();
            }
        }
    }
}
