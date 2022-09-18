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
    public partial class DuzenleForm : Form
    {
        public DataTable dt = new DataTable();
        public DuzenleForm()
        {
            InitializeComponent();
        }

        private void DuzenleForm_Load(object sender, EventArgs e)
        {
            
        }
        public void formGetir(DataTable oldveri)
        {
            dt = oldveri;
            veriGoster(dt);
            Show();
        }
        void veriGoster(DataTable tablo)
        {
            textBox1.Text = tablo.Rows[0][1].ToString();
            textBox2.Text = tablo.Rows[0][2].ToString();
            textBox3.Text = tablo.Rows[0][3].ToString();
            textBox4.Text = tablo.Rows[0][4].ToString();
            textBox5.Text = tablo.Rows[0][5].ToString();
            textBox6.Text = tablo.Rows[0][6].ToString();
            textBox7.Text = tablo.Rows[0][7].ToString();
            textBox8.Text = tablo.Rows[0][8].ToString();
            textBox9.Text = tablo.Rows[0][9].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            veriGoster(dt);
        }

        private void DuzenleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(AdminPanel.manager.aracGuncelle(dt.Rows[0][0].ToString(), textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text, textBox7.Text, textBox8.Text, textBox9.Text))
            {
                MessageBox.Show("Düzenleme Başarılı", "Harika", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }else
                MessageBox.Show("Bir sorun var !", "Tüh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Hide();
        }
    }
}
