using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ORNEKPANEL;
namespace RentACarServer
{
    public partial class Kirala : Form
    {
        public int gunlukfiyat,kiragun,id;

        public Kirala()
        {
            InitializeComponent();
        }

        private void Kirala_Load(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            dateTimePicker2.MinDate = dateTimePicker1.Value.AddDays(1);
            label11.Text = (kiragun = (dateTimePicker2.Value - dateTimePicker1.Value).Days).ToString();
            label10.Text = (gunlukfiyat * kiragun).ToString();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            label11.Text = (kiragun=(dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days).ToString();
            label10.Text = (gunlukfiyat * kiragun).ToString();
        }

        private void Kirala_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(AdminPanel.manager.ArabaKirala(textBox1.Text, textBox2.Text, id.ToString(),dateTimePicker1.Value.Date.ToString(),dateTimePicker2.Value.Date.ToString(),(kiragun*gunlukfiyat).ToString()))
            {
                MessageBox.Show("Araç Başarıyla Kiralandı", "Harika", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Hide();
            }else
                MessageBox.Show("Araç Kiralanırken Sorun Oluştu", "Tüh", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public void goster(int arabaid,int gunluk,string logo)
        {
            try
            {
                pictureBox1.Image = Image.FromFile(logo);
            }catch(Exception e)
            {

            }
            id = arabaid;
            gunlukfiyat = gunluk;
            label7.Text = gunluk.ToString();
            Show();
        }
    }
}
