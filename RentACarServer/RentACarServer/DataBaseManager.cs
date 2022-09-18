using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RENTACARKUTUPHANE;
namespace RentACarServer
{
    public class DataBaseManager
    {
        MySqlConnection baglan = new MySqlConnection("Server=127.0.0.1; Database=demircar; uid=root; Password=123123;");
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataReader reader;
        DataTable tablo = new DataTable();
        DataTable islemciTablo = new DataTable();
        public DataTable kisiTablo = new DataTable();
        User user = new User();
        public DataBaseManager()
        {
            cmd.Connection = baglan;
            adapter.SelectCommand = cmd;
        }
        public DataTable queryCek(string cmd,DataTable tablo)
        {
            this.cmd.CommandText = cmd;
            baglan.Open();
            tablo.Clear();
            adapter.Fill(tablo);
            baglan.Close();
            
            return tablo;
        }
        public bool kullaniciVarmi(User user)
        {
            if(queryCek("SELECT * FROM users WHERE Tc='"+user.Tc+"';",islemciTablo).Rows.Count != 0)
            {
                return true;
            }
            return false;
        }
        public bool kullaniciVarmi(String Tc)
        {
            if (queryCek("SELECT * FROM users WHERE Tc='" + Tc + "';", islemciTablo).Rows.Count != 0)
            {
                return true;
            }
            return false;
        }
        public bool kayitEkle(User user)
        {
            string cmdtext = "INSERT INTO users (Isim,Tc,Telno,Pass) VALUES(@Isim,@Tc,@Telno,@Pass);";
            cmd.CommandText = cmdtext;
            baglan.Open();
            cmd.Parameters.AddWithValue("@Isim", user.Name);
            cmd.Parameters.AddWithValue("@Tc", user.Tc);
            cmd.Parameters.AddWithValue("@Telno", user.Telno);
            cmd.Parameters.AddWithValue("@Pass", user.Pass);
            if (cmd.ExecuteNonQuery() == 1)
            {
                baglan.Close();
                return true;
            }else
            {
                baglan.Close();
                return false;
            }
        }
        public bool sifreKontrol(User user)
        {
            string cmdtext = "SELECT Pass FROM users WHERE Tc='"+user.Tc+"';";
            DataTable userr = queryCek(cmdtext,islemciTablo);
            if (userr != null)
            {
                if (userr.Rows[0][4].ToString() == user.Pass)
                    return true;
            }
            return false;
        }
        public DataTable aktifMusteriler()
        {
            queryCek("SELECT * FROM users WHERE Online='Online';",kisiTablo);
            return kisiTablo;
        }
        public DataTable butunMusteriler()
        {
            queryCek("SELECT * FROM users;", kisiTablo);
            return kisiTablo;
        }
        public void setUserStatus(bool online,string TC)
        {
            string cmdtext = "UPDATE users SET Online='";
            if (online)
                cmdtext += "Online'";
            else
                cmdtext += "Offline'";
            cmdtext += " WHERE Tc='" + TC + "';";
            baglan.Open();
            MessageBox.Show(cmdtext);
            cmd.CommandText = cmdtext;
            cmd.ExecuteNonQuery();
            baglan.Close();
        }
        public User getUserByTc(string tc)
        {
            queryCek("SELECT * FROM users WHERE Tc='" + tc + "';", islemciTablo);
            if (islemciTablo.Rows.Count > 0)
            {
                user.Name = islemciTablo.Rows[0][1].ToString();
                user.Tc = islemciTablo.Rows[0][2].ToString();
                user.Telno = islemciTablo.Rows[0][3].ToString();
            }
            return user;
        }
        public bool aracEkle(string marka,string model,string kira)
        {
            string cmdtext = "INSERT INTO arabalar (DURUM,MARKA,MODEL,GUNLUKKIRA) VALUES(@DURUM,@MARKA,@MODEL,@GUNLUKKIRA);";
            cmd.CommandText = cmdtext;
            baglan.Open();
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@DURUM", "Bos");
            cmd.Parameters.AddWithValue("@MARKA", marka);
            cmd.Parameters.AddWithValue("@MODEL", model);
            cmd.Parameters.AddWithValue("@GUNLUKKIRA", kira);
            if (cmd.ExecuteNonQuery() == 1)
            {
                baglan.Close();
                return true;
            }
            else
            {
                baglan.Close();
                return false;
            }
        }
        public bool aracGuncelle(string ID,string durum,string verilis,string teslim,string marka,string model,string telno,string tc,string kira,string tahsil)
        {
            string cmdtext = "UPDATE arabalar SET DURUM='"+durum+ "',VERILIS='" + verilis + "',TESLIM='" + teslim + "',MARKA='" + marka + "',MODEL='" + model + "',TELNO='" + telno+ "',TC='" + tc+ "',GUNLUKKIRA='" + kira + "',TAHSILAT='" + tahsil+ "' WHERE ID="+ID+";";
            cmd.CommandText = cmdtext;
            baglan.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                baglan.Close();
                return true;
            }else
            {
                baglan.Close();
                return false;
            }
        }
        public bool ArabaKirala(string tc,string telno,string aracID,string verilis,string teslim,string tahsilat)
        {
            string cmdTXT = "UPDATE arabalar SET DURUM='Musteri',TC='" + tc + "',TELNO='" + telno + "',VERILIS='"+verilis+"',TESLIM='"+teslim+"',TAHSILAT='"+tahsilat+"' WHERE ID='" + aracID + "';";
            if(!kullaniciVarmi(tc))
                return false;
            cmd.CommandText = cmdTXT;
            baglan.Open();
            if (cmd.ExecuteNonQuery() == 1)
            {
                baglan.Close();
                return userArac(aracID, tc, true);
            }
            else
            {
                baglan.Close();
                return false;
            }
        }
        public bool userArac(string aracId,string tc,bool ekle)
        {
            string cmdTXT;
            queryCek("SELECT Araclar FROM users WHERE Tc='" + tc + "'", islemciTablo);
            string araclar = islemciTablo.Rows[0][0].ToString();
            baglan.Open();
            if (ekle)
            {
                cmdTXT = "UPDATE users SET Araclar='" + araclar + "," + aracId + "' WHERE Tc='" + tc + "';";
                cmd.CommandText = cmdTXT;
                if(cmd.ExecuteNonQuery() == 1)
                {
                    baglan.Close();
                    return true;
                }else
                {
                    baglan.Close();
                    return false;
                }    
            }
            else
            {
                araclar = araclar.Replace(","+aracId, "");
                cmdTXT = "UPDATE users SET Araclar='" + araclar +  "' WHERE Tc='" + tc + "';";
                cmd.CommandText = cmdTXT;
                if (cmd.ExecuteNonQuery() == 1)
                {
                    baglan.Close();
                    return true;
                }
                else
                {
                    baglan.Close();
                    return false;
                }
            }
        }
        public bool sureDoluMu(string id)
        {
            string cmdtxt = "SELECT * FROM arabalar WHERE ID='" + id + "';";
            queryCek(cmdtxt, tablo);
            DateTime teslim;
            if (tablo.Rows.Count > 0)
            {
                teslim = Convert.ToDateTime(tablo.Rows[0][3]);
                if ((teslim.Date - DateTime.Now.Date).Days <= 0)
                    return true;
            }
            
            return false;
        }
        
    }
}
