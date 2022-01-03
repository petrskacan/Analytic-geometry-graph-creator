using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DMPAplikaceProSŠ
{    /// <summary>
     /// Vypočítání bodů hyperboly
     /// </summary>
    class Hyperbola
    {
        private double m, n, a, b, k = 1, l = 1;
        Polyline hyperbola = new Polyline(), hyperbola2 = new Polyline(), hyperbola3 = new Polyline(), hyperbola4 = new Polyline();
        Polyline asymptota = new Polyline(), asymptota2 = new Polyline();
        private string mO, nO, p;
        private bool obecnaRovnice = false;
        public Hyperbola(string m, string n, string a, string b, double index, int indexBarvy, int tloustka, bool asymptoty)
        {
            PointCollection body = new PointCollection();
            PointCollection body2 = new PointCollection();
            PointCollection body3 = new PointCollection();
            PointCollection body4 = new PointCollection();
            PointCollection bodyAsymptoty = new PointCollection();
            PointCollection bodyAsymptoty2 = new PointCollection();
            if(b.Contains(';'))
            {
                string[] parametry = b.Split(';');
                b = parametry[0];
                k = Convert.ToDouble(parametry[1]);
                l = Convert.ToDouble(parametry[2]);
                mO = parametry[3];
                nO = parametry[4];
                p = parametry[5];
                obecnaRovnice = true;
            }
            try
            {
                this.m = Math.Round(Convert.ToDouble(m),1);
                this.n = Math.Round(Convert.ToDouble(n), 1);
                this.a = Math.Round(Convert.ToDouble(a), 1);
                this.b = Math.Round(Convert.ToDouble(b), 1);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ŠPATNĚ ZADANÉ PARAMTERY!", "Chyba!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }

            if (index == 0)
            {
                hyperbola.ToolTip = "Středová rovnice: ((x - (" + m + "))²/ " + a + "²) - ((y - (" + n + "))²/ " + b + "²) = 1";
                hyperbola2.ToolTip = hyperbola.ToolTip;
                hyperbola3.ToolTip = hyperbola.ToolTip;
                hyperbola4.ToolTip = hyperbola.ToolTip;
                double odmocnina;
                double odmocneno;
                for (double x = Math.Floor(this.m * 100); x <= 10000; x++)
                {
                    odmocnina = (-Math.Round(Math.Pow(this.a, 2), 2) * Math.Round(Math.Pow(this.b, 2), 2) + Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k) * Math.Round(Math.Pow(this.m, 2), 2) - 2 * Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k) * this.m * (x / 100) + Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k) * Math.Round(Math.Pow(x / 100, 2), 2)) / (Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l));
                    odmocneno = Math.Round(Math.Sqrt(odmocnina), 2);
                    if (odmocneno + this.n < 100)
                    {
                        body.Add(new System.Windows.Point(x / 100 * 16 + 800, Math.Round(-(odmocneno + this.n) * 16 + 800, 1)));
                        body2.Add(new System.Windows.Point(x / 100 * 16 + 800, Math.Round(-(this.n - odmocneno) * 16 + 800, 1)));
                    }
                }
                for (double x = Math.Floor(this.m * 100); x >= -10000; x--)
                {
                    odmocnina = (-Math.Round(Math.Pow(this.a, 2), 2) * Math.Round(Math.Pow(this.b, 2), 2) + Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k) * Math.Round(Math.Pow(this.m, 2), 2) - 2 * Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k) * this.m * (x / 100) + Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k) * Math.Round(Math.Pow(x / 100, 2), 2)) / (Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l));
                    odmocneno = Math.Round(Math.Sqrt(odmocnina), 2);
                    if (odmocneno + this.n < 100)
                    {
                        body3.Add(new System.Windows.Point(x / 100 * 16 + 800, Math.Round(-(odmocneno + this.n) * 16 + 800, 1)));
                        body4.Add(new System.Windows.Point(x / 100 * 16 + 800, Math.Round(-(this.n - odmocneno) * 16 + 800, 1)));
                    }
                }
            }
            else
            {
                hyperbola.ToolTip = "Středová rovnice: -((x - (" + m + "))²/ " + b + "²) + ((y - (" + n + "))²/ " + a + "²) = 1";
                hyperbola2.ToolTip = hyperbola.ToolTip;
                hyperbola3.ToolTip = hyperbola.ToolTip;
                hyperbola4.ToolTip = hyperbola.ToolTip;
                double odmocnina;
                double odmocneno;
                for (double y = Math.Floor(this.n * 100) - Math.Floor(this.n); y <= 10000; y++)
                {
                    odmocnina = (Math.Round(Math.Pow(this.a, 2), 2) * (-Math.Round(Math.Pow(this.b, 2), 2)) + Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l) * Math.Round(Math.Pow(this.n, 2), 2) - 2 * Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l) * this.n * (y / 100) + Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l) * Math.Round(Math.Pow(y / 100, 2), 2)) / (Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k));
                    odmocneno = Math.Round(Math.Sqrt(odmocnina), 2);
                    if (odmocneno + this.m <= 100)
                    {
                        body.Add(new System.Windows.Point((odmocneno + this.m) * 16 + 800, -y / 100 * 16 + 800));
                        body2.Add(new System.Windows.Point((this.m - odmocneno) * 16 + 800, -y / 100 * 16 + 800));
                    }
                }
                for (double y = -10000; y <= Math.Floor(this.n * 100) + Math.Floor(this.n); y++)
                {
                    odmocnina = (Math.Round(Math.Pow(this.a, 2), 2) * (-Math.Round(Math.Pow(this.b, 2), 2)) + Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l) * Math.Round(Math.Pow(this.n, 2), 2) - 2 * Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l) * this.n * (y / 100) + Math.Round(Math.Pow(this.a, 2), 2) * Math.Abs(l) * Math.Round(Math.Pow(y / 100, 2), 2)) / (Math.Round(Math.Pow(this.b, 2), 2) * Math.Abs(k));
                    odmocneno = Math.Round(Math.Sqrt(odmocnina), 2);
                    if (odmocneno + this.m <= 100)
                    {
                        body3.Add(new System.Windows.Point((odmocneno + this.m) * 16 + 800, -y / 100 * 16 + 800));
                        body4.Add(new System.Windows.Point((this.m - odmocneno) * 16 + 800, -y / 100 * 16 + 800));
                    }
                }
            }
            bodyAsymptoty.Add(new System.Windows.Point(50 * 16 + 800, -((50 - this.m) / this.a * this.b + this.n) * 16 + 800));
            bodyAsymptoty.Add(new System.Windows.Point(-50 * 16 + 800, -((-50 - this.m) / this.a * this.b + this.n) * 16 + 800));
            bodyAsymptoty2.Add(new System.Windows.Point(50 * 16 + 800,-((50 - this.m) / this.a * (-this.b) + this.n) * 16 + 800));
            bodyAsymptoty2.Add(new System.Windows.Point(-50 * 16 + 800, -((-50 - this.m) / this.a * (-this.b) + this.n) * 16 + 800));
            asymptota.ToolTip = "";
            asymptota2.ToolTip = "";
            hyperbola.Points = body;
            hyperbola2.Points = body2;
            hyperbola3.Points = body3;
            hyperbola4.Points = body4;
            asymptota.Points = bodyAsymptoty;
            asymptota2.Points = bodyAsymptoty2;
            ColorPicker barva = new ColorPicker(indexBarvy);
            hyperbola.Stroke = barva.Barva;
            hyperbola2.Stroke = barva.Barva;
            hyperbola3.Stroke = barva.Barva;
            hyperbola4.Stroke = barva.Barva;
            asymptota.Stroke = barva.Barva;
            asymptota2.Stroke = barva.Barva;
            hyperbola.StrokeThickness = tloustka + 1;
            hyperbola2.StrokeThickness = tloustka + 1;
            hyperbola3.StrokeThickness = tloustka + 1;
            hyperbola4.StrokeThickness = tloustka + 1;
            asymptota.StrokeThickness = 1;
            asymptota2.StrokeThickness = 1;
            if(obecnaRovnice)
            {
                hyperbola.ToolTip = "Obecná rovnice: " + k.ToString() + "x² + " + l.ToString() + "y² + " + mO + "x + " + nO + "y + " + p + " = 0";
                hyperbola2.ToolTip = hyperbola.ToolTip;
                hyperbola3.ToolTip = hyperbola.ToolTip;
                hyperbola4.ToolTip = hyperbola.ToolTip;
            }
            if(!asymptoty)
            {
                asymptota = new Polyline();
                asymptota2 = new Polyline();
                asymptota.ToolTip = "";
                asymptota2.ToolTip = "";
            }
        }
        public Polyline VykresleniHyperboly1()
        {
            return hyperbola;
        }
        public Polyline VykresleniHyperboly2()
        {
            return hyperbola2;
        }
        public Polyline VykresleniHyperboly3()
        {
            return hyperbola3;
        }
        public Polyline VykresleniHyperboly4()
        {
            return hyperbola4;
        }
        public Polyline VykresleniAsymptoty1()
        {
            return asymptota;
        }
        public Polyline VykresleniAsymptoty2()
        {
            return asymptota2;
        }
    }
}
