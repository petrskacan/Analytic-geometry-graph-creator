using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
namespace DMPAplikaceProSŠ
{
    /// <summary>
    /// Vytvoření bodu
    /// </summary>
    class Bod
    {
        private double x, y;
        private Path point = new Path();
        private GeometryGroup krizek = new GeometryGroup();
        /// <summary>
        /// Inicializace bodu pomocí typu string
        /// </summary>
        /// <param name="x">X-ová souřadnice bodu</param>
        /// <param name="y">Y-ová souřadnice bodu</param>
        public Bod(string x, string y, int index, int tloustka)
        {
            try
            {
                this.x = Convert.ToDouble(x);
                this.y = Convert.ToDouble(y);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("ŠPATNĚ ZADANÉ PARAMTERY!", "Chyba!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            this.x = 16 * this.x + 800;
            this.y = 16 * (-this.y) + 800;
            krizek.Children.Add(new LineGeometry(new Point(this.x - 5, this.y), new Point(this.x + 5, this.y)));
            krizek.Children.Add(new LineGeometry(new Point(this.x, this.y - 5), new Point(this.x, this.y + 5)));
            point.Data = krizek;
            point.ToolTip = "[" + x + "; " + y + "]";
            ColorPicker barva = new ColorPicker(index);
            point.Stroke = barva.Barva;
            point.StrokeThickness = tloustka + 1;

        }
        /// <summary>
        /// Vykreslení bodu
        /// </summary>
        public Path VykresleniBodu()
        {
            return point;
        }
    }
}
