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
    /// Vytvoření elipsy
    /// </summary>
    class Elipsa
    {
        private double m, n, a, b;
        private Ellipse elipsa = new Ellipse();
        private string k, l, mO, nO, p;
        private bool obecnaRovnice = false;
        /// <summary>
        /// Inicializace Elipsy
        /// </summary>
        /// <param name="m">X-ová souřadnice středu elipsy</param>
        /// <param name="n">Y-ová souřadnice středu elipsy</param>
        /// <param name="a">Vodorovná poloosa elipsy</param>
        /// <param name="b">Svislá poloosa elipsy</param>
        public Elipsa(string m, string n, string a, string b, int index, int tloustka)
        {
            if(m.Contains(';'))
            {
                string[] parametry = m.Split(';');
                m = parametry[0];
                k = parametry[1];
                l = parametry[2];
                mO = parametry[3];
                nO = parametry[4];
                p = parametry[5];
                obecnaRovnice = true;
            }
            try
            {
                this.m = Convert.ToDouble(m) * 16 + 800;
                this.n = -Convert.ToDouble(n) * 16 + 800;
                this.a = Convert.ToDouble(a) * 16;
                this.b = Convert.ToDouble(b) * 16;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ŠPATNĚ ZADANÉ PARAMTERY!", "Chyba!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            ColorPicker barva = new ColorPicker(index);
            elipsa.Stroke = barva.Barva;
            elipsa.StrokeThickness = tloustka + 1;
            elipsa.ToolTip = "Středová rovnice: ((x - (" + m + "))²/" + a + "²) + ((x - (" + n + "))²/" + b + "²) = 1";
            if(obecnaRovnice)
            {
                elipsa.ToolTip = "Obecná rovnice: " + k + "x² + " + l + "y² + " + mO + "x + " + nO + "y + " + p + " = 0";
            }
        }
        /// <summary>
        /// Vykreslení elipsy
        /// </summary>
        public Ellipse VykresleniElipsy()
        {
            elipsa.Width = a*2;
            elipsa.Height = b*2;
            Canvas.SetLeft(elipsa, m - a);
            Canvas.SetTop(elipsa, n - b);
            return elipsa;
        }
    }
}
