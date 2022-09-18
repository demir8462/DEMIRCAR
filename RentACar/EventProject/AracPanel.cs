using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EventProject;
using RENTACARKUTUPHANE;
namespace RentACar
{
    
    public partial class AracPanel : Form
    {
        public Client client;
        Thread veriTHR;
        int gunlukkira,tahsil;
        public AracPanel()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

        }

        private void AracPanel_Load(object sender, EventArgs e)
        {
            client = Form1.client;
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
            veriTHR = new Thread(new ThreadStart(veriAl));
            veriTHR.Start();

        }
        public void veriAl()
        {
            while(true)
            {
                client.paketAl();
                if(client.paket != null)
                {
                    if(client.paket.tip == IletisimPaketi.PAKETTIPI.LVI)
                    {
                        tabloDoldur(client.paket.LVI);
                    }else if(client.paket.tip == IletisimPaketi.PAKETTIPI.CEVAP)
                    {
                        MessageBox.Show(client.paket.detay);
                    }
                }
                Thread.Sleep(5000);
            }
        }
        public void tabloDoldur(List<string[]> LVIS)
        {
            listView1.Items.Clear();
            if(listView1.SelectedItems.Count == 0)
                pictureBox1.Image = Image.FromFile(RCK.logoBul("rentacar"));
            foreach (string[] LVI in LVIS)
            {
                listView1.Items.Add(new ListViewItem(LVI));
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                pictureBox1.Image = Image.FromFile(RCK.logoBul(listView1.SelectedItems[0].SubItems[1].Text));
                label4.Text = listView1.SelectedItems[0].SubItems[1].Text;
                label5.Text = listView1.SelectedItems[0].SubItems[2].Text;
                gunlukkira = int.Parse(listView1.SelectedItems[0].SubItems[3].Text);
                tahsil = (dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days * gunlukkira;
                label7.Text = tahsil.ToString();
            }
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                tahsil = (dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days * gunlukkira;
                label7.Text = tahsil.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show(Form1.client.paket.detay, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            client.paket.tip = IletisimPaketi.PAKETTIPI.KIRAISTEK;
            client.paket.ID = listView1.SelectedItems[0].SubItems[0].Text;
            client.paket.teslim = dateTimePicker1.Value.Date.ToString();
            client.paket.geriteslim = dateTimePicker2.Value.Date.ToString();
            client.paket.tahsilat = tahsil.ToString();
            client.paketYolla(client.paket);
            // kacgun
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
            if (listView1.SelectedItems.Count > 0)
            {
                tahsil = (dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days * gunlukkira;
                label7.Text = tahsil.ToString();
            }
        }
    }
}
