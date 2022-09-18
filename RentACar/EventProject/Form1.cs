using RentACar;
using RENTACARKUTUPHANE;

namespace EventProject
{
    public enum KONTROLTIPI
    {
        TEXT,SAYI
    }
    public partial class Form1 : Form
    {
        public static string rakamlar = "0123456789";
        string oldiism, oldno, oldtc;
        public static Form1 uygform;
        public static AracPanel panel;
        LogIn logIn;
        public static Client client;
        public static User user;
        IletisimPaketi ipaket;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uygform = (Form1)this;
            logIn = new LogIn();
            client = new Client();
            user = new User();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (stringKontroller(textBox2.Text, rakamlar, KONTROLTIPI.SAYI))
            {
                oldtc = textBox2.Text;
                
            }
            else
            {
                textBox2.Text = oldtc;
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (stringKontroller(textBox3.Text, rakamlar, KONTROLTIPI.SAYI))
            {
                oldno = textBox3.Text;

            }
            else
            {
                textBox3.Text = oldno;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length != 0 && textBox2.Text.Length == 11 && textBox3.Text.Length == 11)
            {
                if(kayitOl())
                {
                    Hide();
                    panel = new AracPanel();
                    panel.Show();
                }
                
            }else
            {
                MessageBox.Show("Butun bilgileri doldurun !","EKSÝK BÝLGÝ",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hide();
            logIn.Show();
        }

        public static bool stringKontroller(string metin,string kontrol ,KONTROLTIPI tip)
        {
            if(tip == KONTROLTIPI.TEXT)
            {
                foreach (char item in kontrol)
                {
                    if(metin.Contains(item))
                    { return true; }
                }
            }else
            {
                foreach (char item in metin)
                {
                    if(!kontrol.Contains(item))
                    {
                        return false;
                    }
                }
                return true;
            }
            
            return false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        { 
            if (stringKontroller(textBox1.Text, rakamlar,KONTROLTIPI.TEXT))
            {
                textBox1.Text = oldiism;
            }else
            {
                oldiism = textBox1.Text;
            }
        }
        public bool kayitOl()
        {
            user.Name = textBox1.Text;
            user.Tc = textBox2.Text;
            user.Telno = textBox3.Text;
            user.Pass = textBox4.Text;
            client.KayitOl(user);
            while(true)
            {
                client.paketAl();
                if(client.paket !=null)
                {
                    if(client.paket.tip == IletisimPaketi.PAKETTIPI.CEVAP)
                    {
                        if(client.paket.cevap)
                        {
                            MessageBox.Show(client.paket.detay, "KAYIT BAÞARILI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        else
                        {
                            MessageBox.Show(client.paket.detay,"HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            return false;
                        }
                        break;
                    }
                    MessageBox.Show("wtf");
                    break;
                }
            }
            return false;
        }
    }
}