using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading.Tasks;

namespace DMPAplikaceProSŠ
{
    /// <summary>
    /// Vypočítání bodů paraboly
    /// </summary>
    class Parabola
    {
        private double m, n, p;
        private Path seskupeniBodu = new Path();
        private Line ridiciPrimka = new Line();
        private int index;
        private string k, l, mO, nO, pO;
        private bool obecnaRovnice = false;
        public Parabola(string m, string n, string p, int index, int indexBarvy, int tloustka, bool ridiciPrimky)
        {
            ridiciPrimka.ToolTip = "";
            PathGeometry bodyparaboly = new PathGeometry();
            PathFigureCollection kolekce2 = new PathFigureCollection();
            PathFigure pomocnyBod = new PathFigure();
            PathSegmentCollection kolekce = new PathSegmentCollection();
            QuadraticBezierSegment parabola = new QuadraticBezierSegment();
            if(m.Contains(';'))
            {
                string[] parametry = m.Split(';');
                m = parametry[0];
                k = parametry[1];
                l = parametry[2];
                mO = parametry[3];
                nO = parametry[4];
                pO = parametry[5];
                obecnaRovnice = true;
            }
            try
            {
                this.m = Convert.ToDouble(m);
                this.n = -Convert.ToDouble(n);
                this.p = Convert.ToDouble(p);
                this.index = index;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ŠPATNĚ ZADANÉ PARAMTERY!", "Chyba!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            switch (index)
            {
                case 0:
                    {
                        seskupeniBodu.ToolTip = "Vrcholová rovnice: (x - (" + m + "))² = 2*(" + p + ")*(y - (" + n + "))";
                        pomocnyBod.StartPoint = new System.Windows.Point((-Math.Sqrt(2 * this.p * (50 + this.n)) + this.m) * 16 + 800, 0);
                        parabola.Point1 = new System.Windows.Point(this.m * 16 + 800, (50 + 2 * this.n) * 16 + 800);
                        parabola.Point2 = new System.Windows.Point((Math.Sqrt(2 * this.p * (50 + this.n)) + this.m) * 16 + 800, 0);
                        ridiciPrimka.X1 = 50 * 16 + 800;
                        ridiciPrimka.Y1 = (this.n + this.p / 2) * 16 + 800;
                        ridiciPrimka.X2 = -50 * 16 + 800;
                        ridiciPrimka.Y2 = (this.n + this.p / 2) * 16 + 800;
                        break;
                    }
                case 1:
                    {
                        seskupeniBodu.ToolTip = "Vrcholová rovnice: (x - (" + m + "))² = -2*(" + p + ")*(y - (" + n + "))";
                        pomocnyBod.StartPoint = new System.Windows.Point((-Math.Sqrt(-2 * this.p * (-50 + this.n)) + this.m) * 16 + 800, 1600);
                        parabola.Point1 = new System.Windows.Point(this.m * 16 + 800, (-50 + 2 * this.n) * 16 + 800);
                        parabola.Point2 = new System.Windows.Point((Math.Sqrt(-2 * this.p * (-50 + this.n)) + this.m) * 16 + 800, 1600);
                        ridiciPrimka.X1 = 50 * 16 + 800;
                        ridiciPrimka.Y1 = (this.n - this.p / 2) * 16 + 800;
                        ridiciPrimka.X2 = -50 * 16 + 800;
                        ridiciPrimka.Y2 = (this.n - this.p / 2) * 16 + 800;

                        break;
                    }
                case 2:
                    {
                        seskupeniBodu.ToolTip = "Vrcholová rovnice: (y - (" + n + "))² = 2*(" + p + ")*(x - (" + m + "))";
                        pomocnyBod.StartPoint = new System.Windows.Point(1600, (Math.Sqrt(2 * this.p * (50 - this.m)) + this.n) * 16 + 800);
                        parabola.Point1 = new System.Windows.Point((-50 + 2 * this.m) * 16 + 800, this.n * 16 + 800);
                        parabola.Point2 = new System.Windows.Point(1600, (-Math.Sqrt(2 * this.p * (50 - this.m)) + this.n) * 16 + 800);
                        ridiciPrimka.X1 = (this.m - this.p / 2) * 16 + 800;
                        ridiciPrimka.Y1 = -50 * 16 + 800;
                        ridiciPrimka.X2 = (this.m - this.p / 2) * 16 + 800;
                        ridiciPrimka.Y2 = 50 * 16 + 800;
                        break;
                    }
                case 3:
                    {
                        seskupeniBodu.ToolTip = "Vrcholová rovnice: (y - (" + n + "))² = -2*(" + p + ")*(x - (" + m + "))";
                        pomocnyBod.StartPoint = new System.Windows.Point(0, (Math.Sqrt(-2 * this.p * (-50 - this.m)) + this.n) * 16 + 800);
                        parabola.Point1 = new System.Windows.Point((50 + 2 * this.m) * 16 + 800, this.n * 16 + 800);
                        parabola.Point2 = new System.Windows.Point(0, (-Math.Sqrt(-2 * this.p * (-50 - this.m)) + this.n) * 16 + 800);
                        ridiciPrimka.X1 = (this.m + this.p / 2) * 16 + 800;
                        ridiciPrimka.Y1 = 50 * 16 + 800;
                        ridiciPrimka.X2 = (this.m + this.p / 2) * 16 + 800;
                        ridiciPrimka.Y2 = -50 * 16 + 800;
                        break;
                    }
            }
            kolekce.Add(parabola);
            pomocnyBod.Segments = kolekce;
            kolekce2.Add(pomocnyBod);
            bodyparaboly.Figures = kolekce2;
            seskupeniBodu.Data = bodyparaboly;
            ColorPicker barva = new ColorPicker(indexBarvy);
            seskupeniBodu.Stroke = barva.Barva;
            ridiciPrimka.Stroke = barva.Barva;
            seskupeniBodu.StrokeThickness = tloustka + 1;
            ridiciPrimka.StrokeThickness = 1;
            ridiciPrimka.ToolTip = seskupeniBodu.ToolTip;
            if (!ridiciPrimky)
            {
                ridiciPrimka = new Line();
                ridiciPrimka.ToolTip = seskupeniBodu.ToolTip;
            }
            if (obecnaRovnice)
            {
                if (l == "0")
                {
                    seskupeniBodu.ToolTip = "Obecná rovnice: " + k + "x² + " + mO + "x + " + nO + "y + " + pO + " = 0";
                    ridiciPrimka.ToolTip = seskupeniBodu.ToolTip;
                }
                else
                {
                    seskupeniBodu.ToolTip = "Obecná rovnice: " + l + "y² + " + mO + "x + " + nO + "y + " + pO + " = 0";
                    ridiciPrimka.ToolTip = seskupeniBodu.ToolTip;
                }
            }
        }
        public Path VykresleniParaboly()
        {
            return seskupeniBodu;
        }
        public Line vykresleniRidiciPrimky()
        {
            return ridiciPrimka;
        }
    }
}