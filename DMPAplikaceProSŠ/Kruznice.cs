using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace DMPAplikaceProSŠ
{
    /// <summary>
    /// Vytvoření kružnice
    /// </summary>
    class Kruznice
    {
        private double m, n, r;
        private Ellipse kruznice = new Ellipse();
        private string mO, nO, p;
        private bool obecnaRovnice = false;
        /// <summary>
        /// Inicializace objektu
        /// </summary>
        /// <param name="m">X-ová souřadnice středu</param>
        /// <param name="n">Y-ová souřadnice středu</param>
        /// <param name="r">poloměr kruřnice</param>
        public Kruznice(string m, string n, string r, int index, int tloustka)
        {
            if(m.Contains(';'))
            {
                string[] parametry = m.Split(';');
                m = parametry[0];
                mO = parametry[3];
                nO = parametry[4];
                p = parametry[5];
                obecnaRovnice = true;
            }
            try
            {
                this.m = Convert.ToDouble(m) * 16 + 800;
                this.n = -Convert.ToDouble(n) * 16 + 800;
                this.r = Convert.ToDouble(r) * 16;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ŠPATNĚ ZADANÉ PARAMTERY!", "Chyba!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            ColorPicker barva = new ColorPicker(index);
            kruznice.Stroke = barva.Barva;
            kruznice.StrokeThickness = tloustka + 1;
            kruznice.ToolTip = "Středová rovnice: (x - (" + m + "))² + (y - (" + n + "))² = " + r + "²";
            if(obecnaRovnice)
            {
                kruznice.ToolTip = "Obecná rovnice: x² + y² + " + mO + "x + " + nO + "y + " + p + " = 0";
            }
        }
        /// <summary>
        /// Vykreslení kružnice
        /// </summary>
        public Ellipse VykresleniKruznice()
        {
            kruznice.Width = r * 2;
            kruznice.Height = r * 2;
            Canvas.SetLeft(kruznice, m - r);
            Canvas.SetTop(kruznice, n - r);
            return kruznice;
        }
    }
}
