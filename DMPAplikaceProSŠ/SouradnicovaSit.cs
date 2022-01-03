using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DMPAplikaceProSŠ
{
    /// <summary>
    /// Souřadný systém
    /// </summary>
    class SouradnicovaSit
    {
        private TextBlock[] popisky = new TextBlock[202];
        /// <summary>
        /// Inicializace Souřadnicové sítě
        /// </summary>
        private Path[] sSystem = new Path[3];
        public SouradnicovaSit()
        {
            GeometryGroup ridiciOsy = new GeometryGroup();
            ridiciOsy.Children.Add(new LineGeometry(new Point(1600 / 2, 0), new Point(1600 / 2, 1600)));
            ridiciOsy.Children.Add(new LineGeometry(new Point(0, 1600 / 2), new Point(1600, 1600 / 2)));
            sSystem[1] = DataSouradnice(10, 1, Brushes.Black);
            sSystem[2] = DataSouradnice(100, 1, Brushes.LightGray);
            sSystem[0] = new Path();
            sSystem[0].StrokeThickness = 2;
            sSystem[0].Stroke = Brushes.Black;
            sSystem[0].Data = ridiciOsy;
        }
        /// <summary>
        /// Vykreslení souřadnicové sítě
        /// </summary>
        public Path[] SSystem()
        {
            return sSystem;
        }
        /// <summary>
        /// Vrací popisky k řídícím osám
        /// </summary>
        public TextBlock[] Popisky()
        {
            double hodnota = -50;
            double posunX = 0, posunY = 0;
            for (int i = 0; i <= 100; i++)
            {
                popisky[i] = new TextBlock();
                popisky[101 + i] = new TextBlock();
                popisky[i].TextAlignment = TextAlignment.Center;
                popisky[i].FontSize = 6;
                popisky[101 + i].FontSize = 6;
                popisky[i].Foreground = Brushes.Black;
                popisky[101 + i].Foreground = Brushes.Black;
                popisky[i].Text = (-hodnota).ToString();
                popisky[101 + i].Text = hodnota.ToString();
                Canvas.SetTop(popisky[i], posunY);
                Canvas.SetLeft(popisky[i], 1600 / 2 - 10);
                Canvas.SetLeft(popisky[101 + i], posunX - 3);
                Canvas.SetTop(popisky[101 + i], 1600 / 2 + 3);
                hodnota++;
                if (hodnota == 0)
                {
                    hodnota++;
                    posunX += 1600 / 100;
                    posunY += 1600 / 100;
                }
                posunX += 1600 / 100;
                posunY += 1600 / 100;
            }
            popisky[201] = new TextBlock();
            popisky[201].TextAlignment = TextAlignment.Center;
            popisky[201].FontSize = 7;
            popisky[201].Text = "0";
            Canvas.SetLeft(popisky[201], 1600 / 2 - 6);
            Canvas.SetTop(popisky[201], 1600 / 2 + 3);
            return popisky;
        }
        /// <summary>
        /// Vytvoření souřadného systému
        /// </summary>
        /// <param name="pocetPrvku">Počet prvků sítě</param>
        /// <param name="tloustka">Tloušťka čáry</param>
        /// <param name="barva">Barva čar</param>
        private Path DataSouradnice(double pocetPrvku, int tloustka, Brush barva)
        {
            double posunX = 0;
            double posunY = 0;
            GeometryGroup data = new GeometryGroup();
            for (int i = 0; i <= pocetPrvku; i++)
            {
                data.Children.Add(new LineGeometry(new Point(0, posunY), new Point(1600, posunY)));
                data.Children.Add(new LineGeometry(new Point(posunX, 0), new Point(posunX, 1600)));
                posunX += 1600 / pocetPrvku;
                posunY += 1600 / pocetPrvku;
            }
            Path vystup = new Path();
            vystup.StrokeThickness = tloustka;
            vystup.Stroke = barva;
            vystup.Data = data;
            return vystup;
        }
    }
}
