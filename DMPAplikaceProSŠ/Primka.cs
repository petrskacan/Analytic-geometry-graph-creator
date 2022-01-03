using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using System.Linq;

namespace DMPAplikaceProSŠ
{
    /// <summary>
    /// Vytvoření přímky
    /// </summary>
    class Primka
    {
        /*private double a, b, c;
        private double[] x = new double[] { -50, 50 };
        private double[] y = new double[2];*/
        private double a1, a2, u1, u2, t;
        private Line primka = new Line();
        private bool obecnaRovnice = false;
        /// <summary>
        /// Inicializace přímky
        public Primka(string a1, string a2, string u1, string u2, int index, int tloustka)
        {
            if(u2 == "Obecná")
            {
                obecnaRovnice = true;
                u2 = "1";
            }
            try
            {
                this.a1 = Convert.ToDouble(a1);
                this.a2 = Convert.ToDouble(a2);
                this.u1 = Convert.ToDouble(u1);
                this.u2 = Convert.ToDouble(u2);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ŠPATNĚ ZADANÉ PARAMTERY!", "Chyba!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            ColorPicker barva = new ColorPicker(index);
            if (!obecnaRovnice)
            {
                t = (50 - this.a1) / this.u1;
                primka.X1 = 50 * 16 + 800;
                primka.Y1 = -(this.a2 + this.u2 * t) * 16 + 800;
                t = (-50 - this.a1) / this.u1;
                primka.X2 = -50 * 16 + 800;
                primka.Y2 = -(this.a2 + this.u2 * t) * 16 + 800;
                primka.ToolTip = "Parametrická rovnice: [x;y] = [" + a1 + ";" + a2 + "] + t * (" + u1 + ";" + u2 + ")";
            }
            else
            {
                double k = (-this.a1 / this.a2);
                double q = (-this.u1 / this.a2);
                primka.X1 = 50 * 16 + 800;
                primka.Y1 = -(k * 50 + q) * 16 + 800;
                primka.X2 = -50 * 16 + 800;
                primka.Y2 = -(k * -50 + q) * 16 + 800;
                primka.ToolTip = "Obecná rovnice: " + a1 + "x +" + a2 + "y +" + u1 + " = 0";
            }
            primka.Stroke = barva.Barva;
            primka.StrokeThickness = tloustka + 1;
        }
        /// <summary>
        /// Vykreslení přímky
        /// </summary>
        public Line VykresleniPrimky()
        {
            return primka;
        }
    }
}
