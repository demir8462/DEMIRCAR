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
    public partial class AracEkle : Form
    {
        public AracEkle()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(AdminPanel.manager.aracEkle(textBox1.Text, textBox2.Text, textBox3.Text))
            {
                MessageBox.Show("Araç Başarıyla Eklendi","Harika",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }else
                MessageBox.Show("Eklenemedi!", "Tüh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close();
        }

        private void AracEkle_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
