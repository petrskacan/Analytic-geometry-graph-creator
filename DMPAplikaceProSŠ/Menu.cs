using System;
using System.Windows.Forms;

namespace DMPAplikaceProSŠ
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }
        private void Menu_Load(object sender, EventArgs e)
        {
            panelBarva.BackColor = BackColor;
        }

        private void panelBarva_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            BackColor = colorDialog1.Color;
            panelBarva.BackColor = BackColor;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "Možnosti")
            {
                button2.Text = "Zpět";
                panelBarva.Visible = true;
                lbBarvaPozadi.Visible = true;
                lbSSystem.Visible = false;
            }
            else
            {

                button2.Text = "Možnosti";
                panelBarva.Visible = false;
                lbBarvaPozadi.Visible = false;
                lbSSystem.Visible = true;
            }
        }

        private void lbSSystem_Click(object sender, EventArgs e)
        {
            System.Windows.Media.Color bgColor = System.Windows.Media.Color.FromArgb(BackColor.A, BackColor.R, BackColor.G, BackColor.B);
            Souradnice souradnice = new Souradnice(bgColor);
            Hide();
            souradnice.ShowDialog();
            Close();
        }
    }
}
