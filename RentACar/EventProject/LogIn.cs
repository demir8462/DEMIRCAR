using EventProject;
using RENTACARKUTUPHANE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace RentACar
{
    public partial class LogIn : Form
    {
        string oldtc;
        
        public LogIn()
        {
            InitializeComponent();
        }

        private void LogIn_Load(object sender, EventArgs e)
        {
            if (Form1.user == null)
                Form1.user = new User();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            Form1.uygform.Show(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1.user.Tc = textBox1.Text;
            Form1.user.Pass = textBox2.Text;
            Form1.client.GirisYap(Form1.user);
            while(true)
            {
                Form1.client.paketAl();
                if(Form1.client.paket != null)
                {
                    if(Form1.client.paket.cevap)
                    {
                        Hide();
                        Form1.panel = new AracPanel();
                        
                        Form1.panel.Show();
                    }
                    else
                    {
                        MessageBox.Show(Form1.client.paket.detay, "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    break;
                }
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (Form1.stringKontroller(textBox1.Text, Form1.rakamlar, KONTROLTIPI.SAYI))
            {
                oldtc = textBox1.Text;

            }
            else
            {
                textBox1.Text = oldtc;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
