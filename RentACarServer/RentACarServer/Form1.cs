using ORNEKPANEL;

namespace RentACarServer
{
    public partial class Form1 : Form
    {
        public static Server server;
        AdminPanel panel = new AdminPanel();
        public Form1()
        {
            InitializeComponent();
            server = new Server();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.startListen(2525);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}