using RENTACARKUTUPHANE;
using RentACarServer;
using System.Data;

namespace ORNEKPANEL
{
    public partial class AdminPanel : Form
    {
        public static DataBaseManager manager = new DataBaseManager();
        DataTable tablo = new DataTable();
        DataTable kisiTablo;
        Thread listeTHR;
        AracEkle eklePanel = new AracEkle();
        DuzenleForm duzenleForm = new DuzenleForm();
        Kirala kiraform = new Kirala();
        List<string[]> ClientTabloList = new List<string[]>();
        IletisimPaketi tabloPaket = new IletisimPaketi();
        public AdminPanel()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            listeTHR = new Thread(new ThreadStart(tabloGuncelle));
            listeTHR.Start();
            tabloPaket.LVI = new List<string[]>();
        }
        void tabloGuncelle()
        {
            while(true)
            {
                // ############### ARAÇ VERÝLERÝ ############################
                tablo = manager.queryCek("SELECT * FROM Arabalar", tablo); // ARAÇ VERÝLERÝNÝ
                listView1.Items.Clear();
                pictureBox1.Image = Image.FromFile(RCK.logoBul("rentacar"));
                tabloPaket.LVI.Clear();
                for (int i = 0; i < tablo.Rows.Count; i++)
                {
                    if(tablo.Rows[i][1].ToString() == "Musteri") // ARAÇ MÜÞTERÝDE ÝSE SÜRESÝ DOLUMUÞ MU KONTROL ET
                    {
                        if (manager.sureDoluMu(tablo.Rows[i][0].ToString()))
                        {
                            manager.userArac(tablo.Rows[i][0].ToString(), tablo.Rows[i][7].ToString(), false);
                            manager.aracGuncelle(tablo.Rows[i][0].ToString(), "BAKIMDA", "--", "--", tablo.Rows[i][4].ToString(), tablo.Rows[i][5].ToString(), "--", "--", tablo.Rows[i][8].ToString(), "--"); // TESLIM ALINDIGINI FARK ETTIM VERI TABANINDAN ARABAYI GUNCELLEDIM
                            
                        }
                    }else if(tablo.Rows[i][1].ToString() == "Bos") // CLIENTE GONDERMEK UZERE VERI PAKETI HAZIRLA VE GONDER
                    {
                        tabloPaket.tip = IletisimPaketi.PAKETTIPI.LVI;
                        string[] Plvi = { tablo.Rows[i][0].ToString(), tablo.Rows[i][4].ToString(), tablo.Rows[i][5].ToString(), tablo.Rows[i][8].ToString() };
                        tabloPaket.LVI.Add(Plvi);
                    }
                    string[] lvi = { tablo.Rows[i][0].ToString(), tablo.Rows[i][1].ToString(), tablo.Rows[i][2].ToString(), tablo.Rows[i][3].ToString(), tablo.Rows[i][4].ToString(), tablo.Rows[i][5].ToString(), tablo.Rows[i][6].ToString(), tablo.Rows[i][7].ToString(), tablo.Rows[i][8].ToString(), tablo.Rows[i][9].ToString() };
                    listView1.Items.Add(new ListViewItem(lvi));
                }
                foreach (Client item in Form1.server.clients) // AKTÝF MÜÞTERÝLERE BOÞ ARAÇLARI GÖNDER
                {
                    if (item.giris)
                        item.paketYolla(tabloPaket);
                }
                string araclar="";
                // ################# AKTÝF KÝÞÝ VERÝLERÝNÝ ÇEK VE YAZ ###################
                listView3.Items.Clear();
                kisiTablo = manager.aktifMusteriler();
                for (int i = 0; i < kisiTablo.Rows.Count; i++)
                {
                    araclar = araclarById(kisiTablo.Rows[i][6].ToString());
                    string[] LVI = { kisiTablo.Rows[i][0].ToString(), kisiTablo.Rows[i][1].ToString(), kisiTablo.Rows[i][2].ToString(), kisiTablo.Rows[i][3].ToString(), kisiTablo.Rows[i][6].ToString() ,araclar};
                    listView3.Items.Add(new ListViewItem(LVI));
                }
                // ################# HERKESÝ YAZ ######################
                listView2.Items.Clear();
                kisiTablo = manager.butunMusteriler();
                
                for (int i = 0; i < kisiTablo.Rows.Count; i++)
                {
                    araclar = araclarById(kisiTablo.Rows[i][6].ToString());
                    string[] LVI = { kisiTablo.Rows[i][0].ToString(), kisiTablo.Rows[i][1].ToString(), kisiTablo.Rows[i][2].ToString(), kisiTablo.Rows[i][3].ToString(), kisiTablo.Rows[i][6].ToString(),araclar };
                    listView2.Items.Add(new ListViewItem(LVI));
                }
                Thread.Sleep(5000);
            }
        }
        public string araclarById(string idsfromtablo)
        {
            string[] aracIds;
            string araclar = "";
            aracIds = idsfromtablo.Split(",");
            if (aracIds.Length > 1)
            {
                araclar = "";
                for (int j = 1; j < listView1.Items.Count; j++)
                {
                    if (aracIds.Contains(listView1.Items[j].SubItems[0].Text))
                    {
                        araclar += listView1.Items[j].SubItems[4].Text + " " + listView1.Items[j].SubItems[5].Text + ";";
                    }
                }
            }
            return araclar;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            eklePanel.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir araç seçin !","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            duzenleForm.formGetir(manager.queryCek("SELECT * FROM arabalar WHERE ID='"+ listView1.SelectedItems[0].SubItems[0].Text + "'", duzenleForm.dt));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir araç seçin !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            manager.aracGuncelle(listView1.SelectedItems[0].SubItems[0].Text, "BAKIMDA", listView1.SelectedItems[0].SubItems[2].Text, listView1.SelectedItems[0].SubItems[3].Text, listView1.SelectedItems[0].SubItems[4].Text, listView1.SelectedItems[0].SubItems[5].Text, listView1.SelectedItems[0].SubItems[6].Text, listView1.SelectedItems[0].SubItems[7].Text, listView1.SelectedItems[0].SubItems[8].Text, listView1.SelectedItems[0].SubItems[9].Text);
        }
       
        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir araç seçin !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            manager.userArac(listView1.SelectedItems[0].SubItems[0].Text, listView1.SelectedItems[0].SubItems[7].Text,false);
            manager.aracGuncelle(listView1.SelectedItems[0].SubItems[0].Text, "Bos", "--", "--", listView1.SelectedItems[0].SubItems[4].Text, listView1.SelectedItems[0].SubItems[5].Text,"--", "--", listView1.SelectedItems[0].SubItems[8].Text, "--");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir araç seçin !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(listView1.SelectedItems[0].SubItems[1].Text!="Bos")
            {
                MessageBox.Show("Bu araç kiralanamaz !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            kiraform.goster(int.Parse(listView1.SelectedItems[0].SubItems[0].Text), int.Parse(listView1.SelectedItems[0].SubItems[8].Text),RCK.logoBul(listView1.SelectedItems[0].SubItems[4].Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Bir araç seçin !", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listView1.SelectedItems[0].SubItems[1].Text != "Musteri")
            {
                MessageBox.Show("Bu araç müþteride deðil!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DateTime  teslim;
            teslim = Convert.ToDateTime(listView1.SelectedItems[0].SubItems[3].Text);
            int gun = (teslim.Date - DateTime.Now.Date).Days;
            int refund = gun * int.Parse(listView1.SelectedItems[0].SubItems[8].Text);
            manager.userArac(listView1.SelectedItems[0].SubItems[0].Text, listView1.SelectedItems[0].SubItems[7].Text, false);
            manager.aracGuncelle(listView1.SelectedItems[0].SubItems[0].Text, "BAKIMDA", "--", "--", listView1.SelectedItems[0].SubItems[4].Text, listView1.SelectedItems[0].SubItems[5].Text, listView1.SelectedItems[0].SubItems[6].Text, listView1.SelectedItems[0].SubItems[7].Text, listView1.SelectedItems[0].SubItems[8].Text, (refund*-1).ToString());
            MessageBox.Show("Geri ödenecek bedel : " + refund.ToString() + "; ARABA BAKIMA GONDERILIYOR", "REFUND", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                panelGuncelle();
                if (listView1.SelectedItems.Count > 0)
                    pictureBox1.Image = Image.FromFile(RCK.logoBul(listView1.SelectedItems[0].SubItems[4].Text));
                else
                    pictureBox1.Image = Image.FromFile(RCK.logoBul("rentacar"));
            }catch(Exception Ee)
            {
                
            }
        }
        
        void panelGuncelle()
        {
            label2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            label4.Text = listView1.SelectedItems[0].SubItems[2].Text;
            label6.Text = listView1.SelectedItems[0].SubItems[3].Text;
            label8.Text = listView1.SelectedItems[0].SubItems[4].Text;
            label10.Text = listView1.SelectedItems[0].SubItems[5].Text;
        }
    }
}