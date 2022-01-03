using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Drawing.Printing;

namespace DMPAplikaceProSŠ
{
    public partial class Souradnice : Window
    {
        public Souradnice()
        {
            Hide();
            Menu menu = new Menu();
            menu.ShowDialog();
            Close();
            InitializeComponent();
        }
        public Souradnice(Color bgcolor)
        {
            InitializeComponent();
            SolidColorBrush brush = new SolidColorBrush(bgcolor);
            Background = brush;
        }
        bool asymptoty = true, ridiciPrimky = true;
        Matrix zacatecniPozice;
        //Prvotní načtení okna
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //pozice středu canvasu
            zacatecniPozice = souradnySystem.RenderTransform.Value;
            //Vykreslení souřadného systému
            SouradnicovaSit sstm = new SouradnicovaSit();
            Path[] data = sstm.SSystem();
            TextBlock[] popisky = sstm.Popisky();
            souradnySystem.Children.Add(data[2]);
            souradnySystem.Children.Add(data[1]);
            souradnySystem.Children.Add(data[0]);
            for (int i = 0; i < popisky.Length; i++)
            {
                souradnySystem.Children.Add(popisky[i]);
            }
            //
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            
        }
        double scale = 1;
        //Přibližování a oddalování canvasu
        private void souradnySystem_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (scale > 2.5 || scale < 0.4)
            {
                if (e.Delta < 0 && scale > 2.5 || e.Delta > 0 && scale < 0.4) { }
                else
                {
                    return;
                }

            }
            bool scaling = true;
            Point poziceMysi = e.MouseDevice.GetPosition(souradnySystem);
            Matrix m = souradnySystem.RenderTransform.Value;
            if (e.Delta > 0)
            {
                scale += 0.1;
                m.ScaleAtPrepend(1.1, 1.1, poziceMysi.X, poziceMysi.Y);
            }
            else
            {
                scale -= 0.1;
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, poziceMysi.X, poziceMysi.Y);
            }
            if (scale > 3 && scaling)
            {
                scaling = false;
            }
            else
            {
                scaling = true;
            }
            souradnySystem.RenderTransform = new MatrixTransform(m);
        }
        //
        Point start, aktualni;
        bool jeZmacknut = false;
        //Posouvání
        private void souradnySystem_MouseMove(object sender, MouseEventArgs e)
        {
            double[] docasneParametry, parametry;
            if (jeZmacknut && cbPohyb.IsChecked == false)
            {
                aktualni = e.GetPosition(souradnySystem);
                Matrix pozice = souradnySystem.RenderTransform.Value;
                pozice.OffsetX = pozice.OffsetX + (aktualni.X - start.X);
                pozice.OffsetY = pozice.OffsetY + (aktualni.Y - start.Y);
                souradnySystem.RenderTransform = new MatrixTransform(pozice);
            }
            else if(jeZmacknut && cbPohyb.IsChecked == true)
            {
                if (rbPar.IsChecked == true)
                {
                    if (typ == "Hyperbola")
                    {
                        foreach (Polyline hyperbola in vybranaHyperbola)
                        {
                            aktualni = e.GetPosition(hyperbola);
                            Matrix pozice = hyperbola.RenderTransform.Value;
                            pozice.OffsetX = pozice.OffsetX + (aktualni.X - start.X);
                            pozice.OffsetY = pozice.OffsetY + (aktualni.Y - start.Y);
                            hyperbola.RenderTransform = new MatrixTransform(pozice);
                        }
                    }
                    else
                    {
                        aktualni = e.GetPosition(pohybujiciSeObjekt);
                        Matrix pozice = pohybujiciSeObjekt.RenderTransform.Value;
                        pozice.OffsetX = pozice.OffsetX + (aktualni.X - start.X);
                        pozice.OffsetY = pozice.OffsetY + (aktualni.Y - start.Y);
                        pohybujiciSeObjekt.RenderTransform = new MatrixTransform(pozice);
                    }
                    Point bod = new Point(Convert.ToDouble(tbX.Text) * 16 + 800, Convert.ToDouble(tbY.Text) * 16 + 800);
                    bod.X += aktualni.X - start.X;
                    bod.Y -= aktualni.Y - start.Y;
                    tbX.Text = Math.Round((bod.X - 800) / 16, 3).ToString();
                    tbY.Text = Math.Round((bod.Y - 800) / 16, 3).ToString();
                }
                else
                {
                    return;
                    //Nedokončená část pohybu s objektu tvořené podle obecných rovnic
                    if(rbParabola.IsChecked == true)
                    {
                        if(lbX.Content.ToString().Contains('L'))
                        {
                            docasneParametry = doplneniNaCtevrec("0", tbX.Text, tbY.Text, tbC.Text, tbB.Text);
                            Point bod = new Point(docasneParametry[0] * 16 + 800, docasneParametry[1] * 16 + 800);
                            bod.X += aktualni.X - start.X;
                            bod.Y -= aktualni.Y - start.Y;
                            parametry = reversniDoplneniNaCtverec(bod.X.ToString(), bod.Y.ToString(), docasneParametry[2].ToString(), docasneParametry[3].ToString());
                            tbC.Text = parametry[3].ToString();
                            tbB.Text = parametry[4].ToString();
                        }
                        else
                        {
                            docasneParametry = doplneniNaCtevrec(tbX.Text, "0", tbY.Text, tbC.Text, tbB.Text);
                            Point bod = new Point(docasneParametry[0] * 16 + 800, docasneParametry[1] * 16 + 800);
                            bod.X += aktualni.X - start.X;
                            bod.Y -= aktualni.Y - start.Y;
                            parametry = reversniDoplneniNaCtverec(bod.X.ToString(), bod.Y.ToString(), docasneParametry[2].ToString(), docasneParametry[3].ToString());
                            tbC.Text = parametry[2].ToString();
                            tbB.Text = parametry[4].ToString();
                        }
                    }
                    else if(rbPrimka.IsChecked == true)
                    {
                        parametry = reversniDoplneniNaCtverec(tbX.Text, tbY.Text, tbC.Text, "0");
                        Point bod = new Point(parametry[0] * 16 + 800, parametry[1] * 16 + 800);
                        bod.X += aktualni.X - start.X;
                        bod.Y -= aktualni.Y - start.Y;
                    }
                    else if(rbKruznice.IsChecked == true)
                    {
                        docasneParametry = doplneniNaCtevrec("1", "1", tbX.Text, tbY.Text, tbC.Text);
                        Point bod = new Point(docasneParametry[0] * 16 + 800, docasneParametry[1] * 16 + 800);
                        bod.X += aktualni.X - start.X;
                        bod.Y -= aktualni.Y - start.Y;
                        parametry = reversniDoplneniNaCtverec(bod.X.ToString(), bod.Y.ToString(), docasneParametry[2].ToString(), docasneParametry[3].ToString());
                        tbX.Text = parametry[2].ToString();
                        tbY.Text = parametry[3].ToString();
                        tbC.Text = parametry[4].ToString();
                    }
                    else
                    {
                        docasneParametry = doplneniNaCtevrec(tbX.Text, tbY.Text, tbC.Text, tbB.Text, tbP.Text);
                        Point bod = new Point(docasneParametry[0] * 16 + 800, docasneParametry[1] * 16 + 800);
                        bod.X += aktualni.X - start.X;
                        bod.Y -= aktualni.Y - start.Y;
                        parametry = reversniDoplneniNaCtverec(bod.X.ToString(), bod.Y.ToString(), docasneParametry[2].ToString(), docasneParametry[3].ToString());
                        tbX.Text = parametry[2].ToString();
                        tbY.Text = parametry[3].ToString();
                        tbC.Text = parametry[4].ToString();
                    }
                }

            }
        }
        //
        //vybrané objekty - použití při úpravách
        Path vybranyBod = new Path();
        Ellipse vybranaKruznice = new Ellipse();
        Line vybranaPrimka = new Line();
        Path stredKruznice = new Path();
        Ellipse vybranaElipsa = new Ellipse();
        Path ohniskoE = new Path();
        Path ohniskoF = new Path();
        Path vybranaParabola = new Path();
        List<Polyline> vybranaHyperbola = new List<Polyline>();
        List<object> vsechnyTvary = new List<object>();
        Line ridiciPrimka = new Line();
        Shape pohybujiciSeObjekt;
        string typ = "";
        //
        //Úpravy objektů
        private void souradnySystem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(cbPohyb.IsChecked == true)
            {
                jeZmacknut = true;
                if (typ == "Hyperbola")
                {
                    start = e.GetPosition(vybranaHyperbola[0]);
                }
                else
                {
                    start = e.GetPosition(pohybujiciSeObjekt);
                }
                return;
            }
            ColorPicker barva = new ColorPicker(cbBarva.SelectedIndex);

            if (btVloz.Content.ToString() == "Aplikovat úpravy")
            {
                vybranaParabola.Stroke = barva.Barva;
                vybranaPrimka.Stroke = barva.Barva;
                vybranyBod.Stroke = barva.Barva;
                vybranaKruznice.Stroke = barva.Barva;
                vybranaElipsa.Stroke = barva.Barva;
                ridiciPrimka.Stroke = barva.Barva;
                foreach (Polyline hyperbola in vybranaHyperbola)
                {
                    hyperbola.Stroke = barva.Barva;
                }
                vybranaHyperbola.Clear();
                btVloz.Content = "Vložit";
                btVloz.Visibility = Visibility.Hidden;
                tbX.Visibility = Visibility.Hidden;
                tbY.Visibility = Visibility.Hidden;
                lbX.Visibility = Visibility.Hidden;
                lbY.Visibility = Visibility.Hidden;
                lbC.Visibility = Visibility.Hidden;
                tbC.Visibility = Visibility.Hidden;
                cbPohyb.Visibility = Visibility.Hidden;
                btDelete.Visibility = Visibility.Hidden;
                btVloz.Margin = new Thickness(217, 175, 0, 0);
                tbC.Text = "";
                tbX.Text = "";
                tbY.Text = "";
                rbBod.IsChecked = false;
                rbPrimka.IsChecked = false;
                rbKruznice.IsChecked = false;
                rbElipsa.IsChecked = false;
                rbHyperbola.IsChecked = false;
                rbParabola.IsChecked = false;
                zapnouttRadioButtony();
            }
            start = e.GetPosition(souradnySystem);
            jeZmacknut = true;
            try
            {
                if (((Shape)e.OriginalSource).Stroke == Brushes.LightGray || ((Shape)e.OriginalSource).Stroke == Brushes.Black)
                {
                    return;
                }
            }
            catch
            {
                return;
            }
            cbTloustka.SelectedIndex = (int)(e.OriginalSource as Shape).StrokeThickness - 1;
            btDelete.Visibility = Visibility.Visible;
            if (e.OriginalSource is Path)
            {
                vybranyBod = (Path)e.OriginalSource;
                pohybujiciSeObjekt = vybranyBod;
                cbPohyb.Visibility = Visibility.Visible;
                start = e.GetPosition(vybranyBod);
                barva = new ColorPicker(vybranyBod.Stroke.ToString());
                cbBarva.SelectedIndex = barva.Index;
                vypnoutRadioButtony();
                if (vybranyBod.ToolTip.ToString()[0] == '[')
                {
                    vybranyBod.Stroke = Brushes.Orange;
                    string souradnice = vybranyBod.ToolTip.ToString();
                    tbX.Visibility = Visibility.Visible;
                    tbY.Visibility = Visibility.Visible;
                    lbX.Visibility = Visibility.Visible;
                    lbY.Visibility = Visibility.Visible;
                    btVloz.Visibility = Visibility.Visible;
                    btVloz.Content = "Aplikovat úpravy";
                    string[] text = souradnice.Split(';');
                    tbX.Text = text[0].Replace('[', ' ').Trim();
                    tbY.Text = text[1].Replace(']', ' ').Trim();
                    typ = "Bod";
                    
                    rbBod.IsChecked = true;
                }
                else
                {

                    vybranyBod = new Path();
                    vybranaParabola = (Path)e.OriginalSource;
                    pohybujiciSeObjekt = vybranaParabola;
                    start = e.GetPosition(vybranaParabola);
                    cbPohyb.Visibility = Visibility.Visible;
                    foreach (UIElement objekt in souradnySystem.Children)
                    {
                        if (objekt is Line && (objekt as Line).ToolTip.ToString() == vybranaParabola.ToolTip.ToString())
                        {
                            ridiciPrimka = objekt as Line;
                        }
                    }
                    vybranaParabola.Stroke = Brushes.Orange;
                    ridiciPrimka.Stroke = Brushes.Orange;
                    string souradnice = vybranaParabola.ToolTip.ToString();
                    string[] split = souradnice.Split(':');
                    souradnice = split[1].Trim();
                    if (souradnice.Last() == '0')
                    {
                        rbObe.IsChecked = true;
                        rbPar.IsChecked = false;
                        rbParabola.IsChecked = true;
                        rbParabola_Checked(true, null);
                        string[] paramtery = souradnice.Split('+');
                        if(paramtery[0].Contains('x'))
                        {
                            cbVzorec.SelectedIndex = 0;
                            string[] x = paramtery[0].Split('x');
                            tbX.Text = x[0].Trim();
                        }
                        else
                        {
                            cbVzorec.SelectedIndex = 1;
                            string[] y = paramtery[0].Split('y');
                            tbX.Text = y[0].Trim();
                        }
                        string[] m = paramtery[1].Split('x');
                        tbY.Text = m[0].Trim();
                        string[] n = paramtery[2].Split('y');
                        tbC.Text = n[0].Trim();
                        string[] p = paramtery[3].Split('=');
                        tbB.Text = p[0].Trim();
                    }
                    else
                    {
                        rbObe.IsChecked = false;
                        rbPar.IsChecked = true;
                        rbParabola.IsChecked = true;
                        rbParabola_Checked(true, null);

                        if (souradnice[1] == 'x')
                        {
                            string[] data = souradnice.Split('=');
                            if (data[1][1] == '-')
                            {
                                cbVzorec.SelectedIndex = 1;
                            }
                            string[] dataM = data[0].Split(')');
                            dataM = dataM[0].Split('(');
                            tbX.Text = dataM[2].Trim();
                            dataM = data[1].Split(')');
                            data = dataM[1].Split('(');
                            tbY.Text = data[2];
                            data = dataM[0].Split('(');
                            tbC.Text = data[1];
                        }
                        else
                        {
                            string[] data = souradnice.Split('=');
                            if (data[1][1] == '-')
                            {
                                cbVzorec.SelectedIndex = 3;
                            }
                            else
                            {
                                cbVzorec.SelectedIndex = 2;
                            }
                            string[] dataM = data[0].Split(')');
                            dataM = dataM[0].Split('(');
                            tbY.Text = dataM[2].Trim();
                            dataM = data[1].Split(')');
                            data = dataM[1].Split('(');
                            tbX.Text = data[2];
                            data = dataM[0].Split('(');
                            tbC.Text = data[1];
                        }
                    }
                    btVloz.Content = "Aplikovat úpravy";
                    typ = "Parabola";
                    rbParabola.IsChecked = true;
                }
            }
            else if(e.OriginalSource is Line)
            {
                barva = new ColorPicker(((Line)e.OriginalSource).Stroke.ToString());
                cbBarva.SelectedIndex = barva.Index;
                if ((e.OriginalSource as Line).ToolTip.ToString()[0] != 'V' || !(e.OriginalSource as Line).ToolTip.ToString().Contains('²'))
                {
                    vypnoutRadioButtony();
                    vybranaPrimka = (Line)e.OriginalSource;
                    pohybujiciSeObjekt = vybranaPrimka;
                    start = e.GetPosition(vybranaPrimka);
                    cbPohyb.Visibility = Visibility.Visible;
                    vybranaPrimka.Stroke = Brushes.Orange;
                    string souradnice = vybranaPrimka.ToolTip.ToString();
                    if (souradnice.Last() == '0')
                    {
                        rbObe.IsChecked = true;
                        rbPar.IsChecked = false;
                        rbPrimka.IsChecked = true;
                        rbPrimka_Checked(true, null);
                        string[] split = souradnice.Split(':');
                        souradnice = split[1].Trim();
                        string[] paramtery = souradnice.Split('+');
                        string[] m = paramtery[0].Split('x');
                        tbX.Text = m[0].Trim();
                        string[] n = paramtery[1].Split('y');
                        tbY.Text = n[0].Trim();
                        string[] p = paramtery[2].Split('=');
                        tbC.Text = p[0].Trim();
                    }
                    else
                    {
                        rbObe.IsChecked = false;
                        rbPar.IsChecked = true;
                        rbPrimka.IsChecked = true;
                        rbPrimka_Checked(true, null);
                        string[] text = souradnice.Split('=');
                        string[] parametry = text[1].Replace('+', ' ').Replace('t', ' ').Split('*');
                        string[] a = parametry[0].Replace('[', ' ').Replace(']', ' ').Trim().Split(';');
                        tbX.Text = a[0];
                        tbY.Text = a[1];
                        string[] u = parametry[1].Replace('(', ' ').Replace(')', ' ').Trim().Split(';');
                        tbC.Text = u[0];
                        tbB.Text = u[1];
                    }
                    btVloz.Content = "Aplikovat úpravy";
                    typ = "Přímka";
                    rbPrimka.IsChecked = true;
                }
                else
                {
                    btDelete.Visibility = Visibility.Hidden;
                    cbPohyb.Visibility = Visibility.Hidden;
                }
            }
            else if(e.OriginalSource is Ellipse)
            {
                barva = new ColorPicker(((Ellipse)e.OriginalSource).Stroke.ToString());
                cbBarva.SelectedIndex = barva.Index;
                vypnoutRadioButtony();
                vybranaKruznice = (Ellipse)e.OriginalSource;
                vybranaKruznice.Stroke = Brushes.Orange;
                if (vybranaKruznice.ToolTip.ToString().Last() == '²' || vybranaKruznice.ToolTip.ToString()[16] == 'x')
                {
                    pohybujiciSeObjekt = vybranaKruznice;
                    start = e.GetPosition(vybranaKruznice);
                    cbPohyb.Visibility = Visibility.Visible;
                    string souradnice = vybranaKruznice.ToolTip.ToString();
                    if (souradnice.Last() == '0')
                    {
                        rbObe.IsChecked = true;
                        rbPar.IsChecked = false;
                        rbKruznice.IsChecked = true;
                        rbKruznice_Checked(true, null);
                        string[] split = souradnice.Split(':');
                        souradnice = split[1].Trim();
                        string[] paramtery = souradnice.Split('+');
                        string[] m = paramtery[2].Split('x');
                        tbX.Text = m[0].Trim();
                        string[] n = paramtery[3].Split('y');
                        tbY.Text = n[0].Trim();
                        string[] p = paramtery[4].Split('=');
                        tbC.Text = p[0].Trim();
                    }
                    else
                    {
                        rbObe.IsChecked = false;
                        rbPar.IsChecked = true;
                        rbKruznice.IsChecked = true;
                        rbKruznice_Checked(true, null);
                        string[] parametry = souradnice.Split('²');
                        string[] souradniceM = parametry[0].Split('(');
                        tbX.Text = souradniceM[2].Replace(')', ' ').Trim();
                        souradniceM = parametry[1].Split('(');
                        tbY.Text = souradniceM[2].Replace(')', ' ').Trim();
                        tbC.Text = parametry[2].Replace('=', ' ').Trim();
                    }
                    btVloz.Content = "Aplikovat úpravy";
                    typ = "Kružnice";
                    rbKruznice.IsChecked = true;
                }
                else
                {
                    vybranaElipsa = vybranaKruznice;
                    vybranaKruznice = new Ellipse();
                    pohybujiciSeObjekt = vybranaElipsa;
                    start = e.GetPosition(vybranaElipsa);
                    cbPohyb.Visibility = Visibility.Visible;
                    string souradnice = vybranaElipsa.ToolTip.ToString();
                    string[] text = souradnice.Split('²');
                    if (souradnice.Last() == '0')
                    {
                        string[] split = souradnice.Split(':');
                        souradnice = split[1].Trim();
                        rbObe.IsChecked = true;
                        rbPar.IsChecked = false;
                        rbElipsa.IsChecked = true;
                        rbElipsa_Checked(true, null);
                        string[] paramtery = souradnice.Split('+');
                        string[] x = paramtery[0].Split('x');
                        tbX.Text = x[0].Trim();
                        string[] y = paramtery[1].Split('y');
                        tbY.Text = y[0].Trim();
                        string[] m = paramtery[2].Split('x');
                        tbC.Text = m[0].Trim();
                        string[] n = paramtery[3].Split('y');
                        tbB.Text = n[0].Trim();
                        string[] p = paramtery[4].Split('=');
                        tbP.Text = p[0].Trim();

                    }
                    else
                    {
                        rbObe.IsChecked = false;
                        rbPar.IsChecked = true;
                        rbElipsa.IsChecked = true;
                        rbElipsa_Checked(true, null);
                        string[] m = text[0].Split('(');
                        tbX.Text = m[3].Replace(')', ' ').Trim();
                        tbC.Text = text[1].Replace('/', ' ').Trim();
                        m = text[2].Split('(');
                        tbY.Text = m[3].Replace(')', ' ').Trim();
                        tbB.Text = text[3].Replace('/', ' ').Trim();
                    }
                    btVloz.Content = "Aplikovat úpravy";
                    typ = "Elipsa";
                    rbElipsa.IsChecked = true;
                }
            }
            else if(e.OriginalSource is Polyline)
            {
                barva = new ColorPicker(((Polyline)e.OriginalSource).Stroke.ToString());
                cbBarva.SelectedIndex = barva.Index;
                cbPohyb.Visibility = Visibility.Visible;
                if ((e.OriginalSource as Polyline).ToolTip.ToString()=="")
                {
                    return;
                }
                vybranaHyperbola.Add((Polyline)e.OriginalSource);
                string souradnice = vybranaHyperbola[0].ToolTip.ToString();
                foreach(UIElement objekty in souradnySystem.Children)
                {
                    Polyline castHyperboly;
                    if (objekty is Polyline)
                    {
                        castHyperboly = objekty as Polyline;
                        if (castHyperboly.ToolTip.ToString() =="")
                        {
                            vybranaHyperbola.Add(castHyperboly);
                        }
                        else if (souradnice == castHyperboly.ToolTip.ToString() && vybranaHyperbola[0].Points != castHyperboly.Points)
                        {
                            vybranaHyperbola.Add(castHyperboly);
                        }
                        
                    }
                }
                start = e.GetPosition(vybranaHyperbola[0]);

                if (souradnice.Last() == '0')
                {
                    string[] split = souradnice.Split(':');
                    souradnice = split[1].Trim();
                    rbObe.IsChecked = true;
                    rbPar.IsChecked = false;
                    rbHyperbola.IsChecked = true;
                    rbHyperbola_Checked(true, null);
                    string[] paramtery = souradnice.Split('+');
                    string[] x = paramtery[0].Split('x');
                    tbX.Text = x[0].Trim();
                    string[] y = paramtery[1].Split('y');
                    tbY.Text = y[0].Trim();
                    string[] m = paramtery[2].Split('x');
                    tbC.Text = m[0].Trim();
                    string[] n = paramtery[3].Split('y');
                    tbB.Text = n[0].Trim();
                    string[] p = paramtery[4].Split('=');
                    tbP.Text = p[0].Trim();

                }
                else
                {
                    rbObe.IsChecked = false;
                    rbPar.IsChecked = true;
                    rbHyperbola.IsChecked = true;
                    rbHyperbola_Checked(true, null);
                    if (souradnice[6] == '-')
                    {
                        cbVzorec.SelectedIndex = 1;
                    }
                    else
                    {
                        cbVzorec.SelectedIndex = 0;
                    }
                    string[] text = souradnice.Split('²');
                    string[] m = text[0].Split('(');
                    tbX.Text = m[3].Replace(')', ' ').Trim();
                    tbC.Text = text[1].Replace('/', ' ').Trim();
                    m = text[2].Split('(');
                    tbY.Text = m[3].Replace(')', ' ').Trim();
                    tbB.Text = text[3].Replace('/', ' ').Trim();
                }
                vybranaHyperbola[0].Stroke = Brushes.Orange;
                vybranaHyperbola[1].Stroke = Brushes.Orange;
                vybranaHyperbola[2].Stroke = Brushes.Orange;
                vybranaHyperbola[3].Stroke = Brushes.Orange;
                vybranaHyperbola[4].Stroke = Brushes.Orange;
                vybranaHyperbola[5].Stroke = Brushes.Orange;
                btVloz.Content = "Aplikovat úpravy";
                typ = "Hyperbola";
                rbHyperbola.IsChecked = true;
            }
        }
        //

        private void souradnySystem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            jeZmacknut = false;
        }
        private void btResetTvar_Click(object sender, RoutedEventArgs e)
        {
            rbBod.IsChecked = false;
            rbPrimka.IsChecked = false;
            rbKruznice.IsChecked = false;
            rbElipsa.IsChecked = false;
            rbHyperbola.IsChecked = false;
            rbParabola.IsChecked = false;
        }
        
        //Vložení a úprava objektu
        private void btVloz_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btVloz.Content == "Vložit")
            {
                if (rbBod.IsChecked == true)
                {
                    Bod bod = new Bod(tbX.Text, tbY.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                    souradnySystem.Children.Add(bod.VykresleniBodu());
                    vsechnyTvary.Add(bod.VykresleniBodu());
                }
                if (rbPrimka.IsChecked == true)
                {
                    if (rbObe.IsChecked == true)
                    {
                        Primka primka = new Primka(tbX.Text, tbY.Text, tbC.Text, "Obecná", cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                        souradnySystem.Children.Add(primka.VykresleniPrimky());
                        vsechnyTvary.Add(primka.VykresleniPrimky());
                    }
                    else
                    {
                        Primka primka = new Primka(tbX.Text, tbY.Text, tbC.Text, tbB.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                        souradnySystem.Children.Add(primka.VykresleniPrimky());
                        vsechnyTvary.Add(primka.VykresleniPrimky());
                    }

                }
                if (rbKruznice.IsChecked == true)
                {
                    if (rbObe.IsChecked == true)
                    {
                        if ((Math.Pow(Convert.ToDouble(tbX.Text), 2) + Math.Pow(Convert.ToDouble(tbY.Text), 2)) < 4 * Convert.ToDouble(tbC.Text))
                        {
                            MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            double[] parametry = doplneniNaCtevrec("1", "1", tbX.Text, tbY.Text, tbC.Text);
                            Kruznice kruznice = new Kruznice(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), parametry[4].ToString(), cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                            souradnySystem.Children.Add(kruznice.VykresleniKruznice());
                            vsechnyTvary.Add(kruznice.VykresleniKruznice());
                        }
                    }
                    else
                    {
                        Kruznice kruznice = new Kruznice(tbX.Text, tbY.Text, tbC.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                        souradnySystem.Children.Add(kruznice.VykresleniKruznice());
                        vsechnyTvary.Add(kruznice.VykresleniKruznice());
                    }
                }
                if (rbElipsa.IsChecked == true)
                {
                    if (rbObe.IsChecked == true)
                    {
                        if (tbX.Text == tbY.Text || Convert.ToDouble(tbX.Text) < 0 || Convert.ToDouble(tbY.Text) < 0 || (Math.Pow(Convert.ToDouble(tbC.Text), 2) / (4 * Convert.ToDouble(tbX.Text)) + Math.Pow(Convert.ToDouble(tbB.Text), 2) / (4 * Convert.ToDouble(tbY.Text)) - Convert.ToDouble(tbP.Text)) < 0)
                        {
                            MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            double[] parametry = doplneniNaCtevrec(tbX.Text, tbY.Text, tbC.Text, tbB.Text, tbP.Text);
                            Elipsa elipsa = new Elipsa(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), Math.Sqrt(parametry[2]).ToString(), Math.Sqrt(parametry[3]).ToString(), cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                            souradnySystem.Children.Add(elipsa.VykresleniElipsy());
                            vsechnyTvary.Add(elipsa.VykresleniElipsy());
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(tbC.Text) > Convert.ToDouble(tbB.Text) && cbVzorec.SelectedIndex == 1 || Convert.ToDouble(tbB.Text) > Convert.ToDouble(tbC.Text) && cbVzorec.SelectedIndex == 0)
                        {
                            MessageBox.Show("Vyberte správný vzorec pro tyto parametry!", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            Elipsa elipsa = new Elipsa(tbX.Text, tbY.Text, tbC.Text, tbB.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                            souradnySystem.Children.Add(elipsa.VykresleniElipsy());
                            vsechnyTvary.Add(elipsa.VykresleniElipsy());

                        }
                    }
                }
                if (rbParabola.IsChecked == true)
                {
                    if (rbObe.IsChecked == true)
                    {
                        if (cbVzorec.SelectedIndex == 0)
                        {
                            if (Convert.ToDouble(tbX.Text) * Convert.ToDouble(tbY.Text) != 0)
                            {
                                double[] parametry = doplneniNaCtevrec(tbX.Text, "0", tbY.Text, tbC.Text, tbB.Text);
                                Parabola parabola = new Parabola(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), Math.Abs(parametry[2]).ToString(), (int)parametry[3], cbBarva.SelectedIndex, cbTloustka.SelectedIndex, ridiciPrimky);
                                souradnySystem.Children.Add(parabola.VykresleniParaboly());
                                vsechnyTvary.Add(parabola.VykresleniParaboly());
                                souradnySystem.Children.Add(parabola.vykresleniRidiciPrimky());
                                vsechnyTvary.Add(parabola.vykresleniRidiciPrimky());
                            }
                        }
                        else if (cbVzorec.SelectedIndex == 1)
                        {
                            if (Convert.ToDouble(tbX.Text) * Convert.ToDouble(tbY.Text) != 0)
                            {
                                double[] parametry = doplneniNaCtevrec("0", tbX.Text, tbY.Text, tbC.Text, tbB.Text);
                                Parabola parabola = new Parabola(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), Math.Abs(parametry[2]).ToString(), (int)parametry[3], cbBarva.SelectedIndex, cbTloustka.SelectedIndex, ridiciPrimky);
                                souradnySystem.Children.Add(parabola.VykresleniParaboly());
                                vsechnyTvary.Add(parabola.VykresleniParaboly());
                                souradnySystem.Children.Add(parabola.vykresleniRidiciPrimky());
                                vsechnyTvary.Add(parabola.vykresleniRidiciPrimky());
                            }
                        }
                    }
                    else
                    {
                        Parabola parabola = new Parabola(tbX.Text, tbY.Text, tbC.Text, cbVzorec.SelectedIndex, cbBarva.SelectedIndex, cbTloustka.SelectedIndex, ridiciPrimky);
                        souradnySystem.Children.Add(parabola.VykresleniParaboly());
                        vsechnyTvary.Add(parabola.VykresleniParaboly());
                        souradnySystem.Children.Add(parabola.vykresleniRidiciPrimky());
                        vsechnyTvary.Add(parabola.vykresleniRidiciPrimky());
                    }
                }
                if (rbHyperbola.IsChecked == true)
                {
                    if (rbObe.IsChecked == true)
                    {
                        if (Convert.ToDouble(tbX.Text) * Convert.ToDouble(tbY.Text) >= 0 || (Math.Pow(Convert.ToDouble(tbC.Text), 2) / (4 * Convert.ToDouble(tbX.Text)) + Math.Pow(Convert.ToDouble(tbB.Text), 2) / (4 * Convert.ToDouble(tbY.Text)) - Convert.ToDouble(tbP.Text)) == 0)
                        {
                            MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            double[] parametry = doplneniNaCtevrec(tbX.Text, tbY.Text, tbC.Text, tbB.Text, tbP.Text);
                            Hyperbola hyperbola = new Hyperbola(parametry[0].ToString(), parametry[1].ToString(), parametry[2].ToString(), parametry[2].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[3], cbBarva.SelectedIndex, cbTloustka.SelectedIndex, asymptoty);
                            souradnySystem.Children.Add(hyperbola.VykresleniHyperboly1());
                            vsechnyTvary.Add(hyperbola.VykresleniHyperboly1());
                            souradnySystem.Children.Add(hyperbola.VykresleniHyperboly2());
                            vsechnyTvary.Add(hyperbola.VykresleniHyperboly2());
                            souradnySystem.Children.Add(hyperbola.VykresleniHyperboly3());
                            vsechnyTvary.Add(hyperbola.VykresleniHyperboly3());
                            souradnySystem.Children.Add(hyperbola.VykresleniHyperboly4());
                            vsechnyTvary.Add(hyperbola.VykresleniHyperboly4());
                            souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty1());
                            vsechnyTvary.Add(hyperbola.VykresleniAsymptoty1());
                            souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty2());
                            vsechnyTvary.Add(hyperbola.VykresleniAsymptoty2());
                        }
                    }
                    else
                    {
                        Hyperbola hyperbola = new Hyperbola(tbX.Text, tbY.Text, tbC.Text, tbB.Text, cbVzorec.SelectedIndex, cbBarva.SelectedIndex, cbTloustka.SelectedIndex, asymptoty);
                        souradnySystem.Children.Add(hyperbola.VykresleniHyperboly1());
                        vsechnyTvary.Add(hyperbola.VykresleniHyperboly1());
                        souradnySystem.Children.Add(hyperbola.VykresleniHyperboly2());
                        vsechnyTvary.Add(hyperbola.VykresleniHyperboly2());
                        souradnySystem.Children.Add(hyperbola.VykresleniHyperboly3());
                        vsechnyTvary.Add(hyperbola.VykresleniHyperboly3());
                        souradnySystem.Children.Add(hyperbola.VykresleniHyperboly4());
                        vsechnyTvary.Add(hyperbola.VykresleniHyperboly4());
                        souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty1());
                        vsechnyTvary.Add(hyperbola.VykresleniAsymptoty1());
                        souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty2());
                        vsechnyTvary.Add(hyperbola.VykresleniAsymptoty2());
                    }
                }
                tbB.Text = "";
                tbC.Text = "";
                tbX.Text = "";
                tbY.Text = "";
                tbP.Text = "";

            }
            if((string)btVloz.Content == "Aplikovat úpravy")
            {
                cbPohyb.Visibility = Visibility.Hidden;
                cbPohyb.IsChecked = false;
                if (typ == "Bod")
                {
                    Bod bod = new Bod(tbX.Text, tbY.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                    souradnySystem.Children.Remove(vybranyBod);
                    vsechnyTvary.Remove(vybranyBod);
                    vybranyBod = bod.VykresleniBodu();
                    souradnySystem.Children.Add(vybranyBod);
                    vsechnyTvary.Add(vybranyBod);
                }
                if(typ == "Přímka")
                {
                    Primka primka;
                    if (rbObe.IsChecked == true)
                    {
                        primka = new Primka(tbX.Text, tbY.Text, tbC.Text, "Obecná", cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                    }
                    else
                    { 
                        primka = new Primka(tbX.Text, tbY.Text, tbC.Text, tbB.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                    }
                    souradnySystem.Children.Remove(vybranaPrimka);
                    vsechnyTvary.Remove(vybranaPrimka);
                    vybranaPrimka = primka.VykresleniPrimky();
                    souradnySystem.Children.Add(vybranaPrimka);
                    vsechnyTvary.Add(vybranaPrimka);
                }
                if(typ == "Kružnice")
                {
                    Kruznice kruznice;
                    if (rbObe.IsChecked == true)
                    {
                        if ((Math.Pow(Convert.ToDouble(tbX.Text), 2) + Math.Pow(Convert.ToDouble(tbY.Text), 2)) < 4 * Convert.ToDouble(tbC.Text))
                        {
                            MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            double[] parametry = doplneniNaCtevrec("1", "1", tbX.Text, tbY.Text, tbC.Text);
                            kruznice = new Kruznice(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), parametry[4].ToString(), cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                        }
                    }
                    else
                    {
                        kruznice = new Kruznice(tbX.Text, tbY.Text, tbC.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                    }
                    souradnySystem.Children.Remove(vybranaKruznice);
                    vsechnyTvary.Remove(vybranaKruznice);
                    vybranaKruznice = kruznice.VykresleniKruznice();
                    souradnySystem.Children.Add(vybranaKruznice);
                    souradnySystem.Children.Remove(stredKruznice);
                    vsechnyTvary.Add(vybranaKruznice);
                }
                if(typ == "Elipsa")
                {
                    Elipsa elipsa;
                    if (rbObe.IsChecked == true)
                    {
                        if (tbX.Text == tbY.Text || Convert.ToDouble(tbX.Text) < 0 || Convert.ToDouble(tbY.Text) < 0 || (Math.Pow(Convert.ToDouble(tbC.Text), 2) / (4 * Convert.ToDouble(tbX.Text)) + Math.Pow(Convert.ToDouble(tbB.Text), 2) / (4 * Convert.ToDouble(tbY.Text)) - Convert.ToDouble(tbP.Text)) < 0)
                        {
                            MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            double[] parametry = doplneniNaCtevrec(tbX.Text, tbY.Text, tbC.Text, tbB.Text, tbP.Text);
                            elipsa = new Elipsa(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), Math.Sqrt(parametry[2]).ToString(), Math.Sqrt(parametry[3]).ToString(), cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                        }
                    }
                    else
                    {
                        elipsa = new Elipsa(tbX.Text, tbY.Text, tbC.Text, tbB.Text, cbBarva.SelectedIndex, cbTloustka.SelectedIndex);
                    }
                    souradnySystem.Children.Remove(vybranaElipsa);
                    vsechnyTvary.Remove(vybranaElipsa);
                    vybranaElipsa = elipsa.VykresleniElipsy();
                    souradnySystem.Children.Add(vybranaElipsa);
                    vsechnyTvary.Add(vybranaElipsa);
                }
                if(typ == "Parabola")
                {
                    Parabola parabola;
                    if (rbObe.IsChecked == true)
                    {
                        if (cbVzorec.SelectedIndex == 0)
                        {
                            if (Convert.ToDouble(tbX.Text) * Convert.ToDouble(tbY.Text) != 0)
                            {
                                double[] parametry = doplneniNaCtevrec(tbX.Text, "0", tbY.Text, tbC.Text, tbB.Text);
                                parabola = new Parabola(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), Math.Abs(parametry[2]).ToString(), (int)parametry[3], cbBarva.SelectedIndex, cbTloustka.SelectedIndex, ridiciPrimky);
                            }
                            else
                            {
                                MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        else if (cbVzorec.SelectedIndex == 1)
                        {
                            if (Convert.ToDouble(tbX.Text) * Convert.ToDouble(tbY.Text) != 0)
                            {
                                double[] parametry = doplneniNaCtevrec("0", tbX.Text, tbY.Text, tbC.Text, tbB.Text);
                                parabola = new Parabola(parametry[0].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[1].ToString(), Math.Abs(parametry[2]).ToString(), (int)parametry[3], cbBarva.SelectedIndex, cbTloustka.SelectedIndex, ridiciPrimky);
                            }
                            else
                            {
                                MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        parabola = new Parabola(tbX.Text, tbY.Text, tbC.Text, cbVzorec.SelectedIndex, cbBarva.SelectedIndex, cbTloustka.SelectedIndex, ridiciPrimky);
                    }
                    souradnySystem.Children.Remove(vybranaParabola);
                    vsechnyTvary.Remove(vybranaParabola);
                    vsechnyTvary.Remove(ridiciPrimka);
                    souradnySystem.Children.Remove(ridiciPrimka);
                    vybranaParabola = parabola.VykresleniParaboly();
                    vsechnyTvary.Add(vybranaParabola);
                    souradnySystem.Children.Add(vybranaParabola);
                    souradnySystem.Children.Add(parabola.vykresleniRidiciPrimky());
                    vsechnyTvary.Add(parabola.vykresleniRidiciPrimky());
                }
                if (typ == "Hyperbola")
                {
                    Hyperbola hyperbola;
                    if (rbObe.IsChecked == true)
                    {
                        if (Convert.ToDouble(tbX.Text) * Convert.ToDouble(tbY.Text) >= 0 || (Math.Pow(Convert.ToDouble(tbC.Text), 2) / (4 * Convert.ToDouble(tbX.Text)) + Math.Pow(Convert.ToDouble(tbB.Text), 2) / (4 * Convert.ToDouble(tbY.Text)) - Convert.ToDouble(tbP.Text)) == 0)
                        {
                            MessageBox.Show("Chybně zadané parametry", "Upozornění!", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        else
                        {
                            double[] parametry = doplneniNaCtevrec(tbX.Text, tbY.Text, tbC.Text, tbB.Text, tbP.Text);
                            hyperbola = new Hyperbola(parametry[0].ToString(), parametry[1].ToString(), parametry[2].ToString(), parametry[2].ToString() + ";" + parametry[6].ToString() + ";" + parametry[7].ToString() + ";" + parametry[8].ToString() + ";" + parametry[9].ToString() + ";" + parametry[10].ToString(), parametry[3], cbBarva.SelectedIndex, cbTloustka.SelectedIndex, asymptoty);
                        }
                    }
                    else
                    {
                        hyperbola = new Hyperbola(tbX.Text, tbY.Text, tbC.Text, tbB.Text, cbVzorec.SelectedIndex, cbBarva.SelectedIndex, cbTloustka.SelectedIndex, asymptoty);
                    }
                    souradnySystem.Children.Remove(vybranaHyperbola[0]);
                    souradnySystem.Children.Remove(vybranaHyperbola[1]);
                    souradnySystem.Children.Remove(vybranaHyperbola[2]);
                    souradnySystem.Children.Remove(vybranaHyperbola[3]);
                    souradnySystem.Children.Remove(vybranaHyperbola[4]);
                    souradnySystem.Children.Remove(vybranaHyperbola[5]);
                    vsechnyTvary.Remove(vybranaHyperbola[0]);
                    vsechnyTvary.Remove(vybranaHyperbola[1]);
                    vsechnyTvary.Remove(vybranaHyperbola[2]);
                    vsechnyTvary.Remove(vybranaHyperbola[3]);
                    vsechnyTvary.Remove(vybranaHyperbola[4]);
                    vsechnyTvary.Remove(vybranaHyperbola[5]);
                    souradnySystem.Children.Add(hyperbola.VykresleniHyperboly1());
                    vsechnyTvary.Add(hyperbola.VykresleniHyperboly1());
                    souradnySystem.Children.Add(hyperbola.VykresleniHyperboly2());
                    vsechnyTvary.Add(hyperbola.VykresleniHyperboly2());
                    souradnySystem.Children.Add(hyperbola.VykresleniHyperboly3());
                    vsechnyTvary.Add(hyperbola.VykresleniHyperboly3());
                    souradnySystem.Children.Add(hyperbola.VykresleniHyperboly4());
                    vsechnyTvary.Add(hyperbola.VykresleniHyperboly4());
                    souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty1());
                    vsechnyTvary.Add(hyperbola.VykresleniAsymptoty1());
                    souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty2());
                    vsechnyTvary.Add(hyperbola.VykresleniAsymptoty2());
                    vybranaHyperbola.Clear();
                }

                btVloz.Content = "Vložit";
                btVloz.Margin = new Thickness(217, 175, 0, 0);
                btVloz.Visibility = Visibility.Hidden;
                tbX.Visibility = Visibility.Hidden;
                tbY.Visibility = Visibility.Hidden;
                lbX.Visibility = Visibility.Hidden;
                lbY.Visibility = Visibility.Hidden;
                lbC.Visibility = Visibility.Hidden;
                tbC.Visibility = Visibility.Hidden;
                lbB.Visibility = Visibility.Hidden;
                tbB.Visibility = Visibility.Hidden;
                vybranaElipsa = new Ellipse();
                vybranaHyperbola = new List<Polyline>();
                vybranaKruznice = new Ellipse();
                vybranaParabola = new Path();
                vybranaPrimka = new Line();
                vybranyBod = new Path();
                ridiciPrimka = new Line();
                btDelete.Visibility = Visibility.Hidden;
                tbC.Text = "";
                tbX.Text = "";
                tbY.Text = "";
                tbB.Text = "";
                tbP.Text = "";
                cbVzorec.Items.Clear();
                zapnouttRadioButtony();
            }
        }
        //
        //resetování pohledu
        private void btReset_Click(object sender, RoutedEventArgs e)
        {
            souradnySystem.RenderTransform = new MatrixTransform(zacatecniPozice);
            foreach(object tvar in vsechnyTvary)
            {
                souradnySystem.Children.Remove(tvar as UIElement);
            }
            vsechnyTvary.Clear();
            scale = 1;
        }
        //
        //Zobrazeení potřebných datových položek na vyplnění podle vybraného radioButtonu
        private void rbBod_Checked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Visible;
            tbY.Visibility = Visibility.Visible;
            lbX.Visibility = Visibility.Visible;
            lbY.Visibility = Visibility.Visible;
            cbVzorec.Items.Clear();
            btVloz.Visibility = Visibility.Visible;
        }

        private void rbBod_Unchecked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
        }

        private void rbPrimka_Checked(object sender, RoutedEventArgs e)
        {
            if(rbObe.IsChecked == true)
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Obecná rovnice: ax + by + c = 0");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "a: ";
                lbY.Content = "b: ";
                lbC.Content = "c: ";
                btVloz.Margin = new Thickness(217, 203, 0, 0);
            }
            else
            { 
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                lbB.Visibility = Visibility.Visible;
                tbB.Visibility = Visibility.Visible;
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Parametrická rovnice: [x;y] = [a₁;a₂] + t * (u₁;u₂)");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "a₁: ";
                lbY.Content = "a₂: ";
                lbC.Content = "u₁: ";
                lbB.Content = "u₂: ";
                btVloz.Margin = new Thickness(217, 231, 0, 0); }
        }

        private void rbPrimka_Unchecked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            
            tbB.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            btVloz.Margin = new Thickness(217, 175, 0, 0);
            lbX.Content = "x: ";
            lbY.Content = "y: ";
            lbC.Content = "c: ";
            lbB.Content = "b: ";
        }

        private void rbKruznice_Checked(object sender, RoutedEventArgs e)
        {
            if (rbObe.IsChecked == true)
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Obecná rovnice: x² + y² + Mx + Ny + P = 0");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "M: ";
                lbY.Content = "N: ";
                lbC.Content = "P: ";
                btVloz.Margin = new Thickness(217, 203, 0, 0);
            }
            else
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                tbP.Visibility = Visibility.Hidden;
                lbP.Visibility = Visibility.Hidden;
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Středová rovnice: (x - m)² + (y - n)² = r²");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "m: ";
                lbY.Content = "n: ";
                lbC.Content = "r: ";
                btVloz.Margin = new Thickness(217, 203, 0, 0);
            }
        }

        private void rbElipsa_Checked(object sender, RoutedEventArgs e)
        {
            if (rbObe.IsChecked == true)
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbP.Visibility = Visibility.Visible;
                lbP.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbX.Content = "K:";
                lbY.Content = "L:";
                lbB.Content = "N:";
                lbC.Content = "M:";
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Obecná rovnice: Kx² + Ly² + Mx + Ny + P = 0");
                cbVzorec.SelectedIndex = 0;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                
                lbB.Visibility = Visibility.Visible;
                tbB.Visibility = Visibility.Visible;
                tbP.Visibility = Visibility.Visible;
                btVloz.Margin = new Thickness(217, 259, 0, 0);
            }
            else
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                
                lbB.Visibility = Visibility.Visible;
                tbB.Visibility = Visibility.Visible;
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Středová rovnice: ((x - m)²/a²) + ((y - n)²/ b²) = 1");
                cbVzorec.Items.Add("Středová rovnice: ((x - m)²/b²) + ((y - n)²/a²) = 1");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "m: ";
                lbY.Content = "n: ";
                lbC.Content = "a: ";
                btVloz.Margin = new Thickness(217, 231, 0, 0);
            }
        }

        private void rbElipsa_Unchecked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            tbP.Visibility = Visibility.Hidden;
            lbP.Visibility = Visibility.Hidden;
            tbB.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            btVloz.Margin = new Thickness(217, 175, 0, 0);
            lbX.Content = "x: ";
            lbY.Content = "y: ";
            lbC.Content = "c: ";
        }

        private void rbParabola_Checked(object sender, RoutedEventArgs e)
        {
            if (rbObe.IsChecked == true)
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                lbB.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                tbB.Visibility = Visibility.Visible;
                
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Obecná rovnice: Kx² + Mx + Ny + P = 0");
                cbVzorec.Items.Add("Obecná rovnice: Ly² + Mx + Ny + P = 0");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "K: ";
                lbY.Content = "M: ";
                lbC.Content = "N: ";
                lbB.Content = "P: ";
                btVloz.Margin = new Thickness(217, 231, 0, 0);
            }
            else
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Vrcholová rovnice: (x - m)² = 2p(y - n)");
                cbVzorec.Items.Add("Vrcholová rovnice: (x - m)² = -2p(y - n)");
                cbVzorec.Items.Add("Vrcholová rovnice: (y - n)² = 2p(x - m)");
                cbVzorec.Items.Add("Vrcholová rovnice: (y - n)² = -2p(x - m)");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "m: ";
                lbY.Content = "n: ";
                lbC.Content = "p: ";
                btVloz.Margin = new Thickness(217, 203, 0, 0);
            }
        }

        private void rbParabola_Unchecked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            tbP.Visibility = Visibility.Hidden;
            lbP.Visibility = Visibility.Hidden;
            tbB.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            btVloz.Margin = new Thickness(217, 175, 0, 0);
            lbX.Content = "x: ";
            lbY.Content = "y: ";
            lbC.Content = "c: ";
        }

        private void rbKruznice_Unchecked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            tbP.Visibility = Visibility.Hidden;
            lbP.Visibility = Visibility.Hidden;
            btVloz.Margin = new Thickness(217, 175, 0, 0);
            lbX.Content = "x: ";
            lbY.Content = "y: ";
            lbC.Content = "c: ";
        }
        void vypnoutRadioButtony()
        {
            rbBod.IsEnabled = false;
            rbElipsa.IsEnabled = false;
            rbHyperbola.IsEnabled = false;
            rbKruznice.IsEnabled = false;
            rbParabola.IsEnabled = false;
            rbPrimka.IsEnabled = false;
            btResetTvar.IsEnabled = false;
            btReset.IsEnabled = false;
        }

        private void rbHyperbola_Checked(object sender, RoutedEventArgs e)
        {
            if (rbObe.IsChecked == true)
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbP.Visibility = Visibility.Visible;
                lbP.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbX.Content = "K:";
                lbY.Content = "L:";
                lbB.Content = "N:";
                lbC.Content = "M:";
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Obecná rovnice: Kx² + Ly² + Mx + Ny + P = 0");
                cbVzorec.SelectedIndex = 0;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                
                lbB.Visibility = Visibility.Visible;
                tbP.Visibility = Visibility.Visible;
                tbB.Visibility = Visibility.Visible;
                btVloz.Margin = new Thickness(217, 259, 0, 0);
            }
            else
            {
                tbX.Visibility = Visibility.Visible;
                tbY.Visibility = Visibility.Visible;
                lbX.Visibility = Visibility.Visible;
                lbY.Visibility = Visibility.Visible;
                btVloz.Visibility = Visibility.Visible;
                lbC.Visibility = Visibility.Visible;
                tbC.Visibility = Visibility.Visible;
                
                lbB.Visibility = Visibility.Visible;
                tbB.Visibility = Visibility.Visible;
                cbVzorec.Items.Clear();
                cbVzorec.Items.Add("Středová rovnice: (x - m)²/a² - (y - n)²/b² = 1");
                cbVzorec.Items.Add("Středová rovnice: -(x - m)²/b² + (y - n)²/a² = 1");
                cbVzorec.SelectedIndex = 0;
                lbX.Content = "m: ";
                lbY.Content = "n: ";
                lbC.Content = "a: ";
                btVloz.Margin = new Thickness(217, 231, 0, 0);
            }
        }

        private void rbHyperbola_Unchecked(object sender, RoutedEventArgs e)
        {
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            btVloz.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            
            tbB.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            tbP.Visibility = Visibility.Hidden;
            lbP.Visibility = Visibility.Hidden;
            btVloz.Margin = new Thickness(217, 175, 0, 0);
            lbX.Content = "x: ";
            lbY.Content = "y: ";
            lbC.Content = "c: ";
            lbB.Content= "b: ";
        }

        private void zapnouttRadioButtony()
        {
            rbBod.IsEnabled = true;
            rbElipsa.IsEnabled = true;
            rbHyperbola.IsEnabled = true;
            rbKruznice.IsEnabled = true;
            rbParabola.IsEnabled = true;
            rbPrimka.IsEnabled = true;
            btResetTvar.IsEnabled = true;
            btReset.IsEnabled = true;
        }
        string cesta = "";
        //Ukládání do souborů
        void SaveFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog ulozit = new SaveFileDialog();
            bool? result = true;
            if (cesta == "")
            {
                ulozit.Filter = "Projekt formátu dmp (.pdmp)|*.pdmp";
                result = ulozit.ShowDialog();
            }
            else
            {
                result = true;
                ulozit.FileName = cesta;
            }
            if(result == true)
            {
                cesta = ulozit.FileName;
                System.IO.StreamWriter zapis = new System.IO.StreamWriter(cesta);
                string toolTip = "0";
                foreach (object objekt in vsechnyTvary)
                {
                    string vystup = "";
                    ColorPicker barva = new ColorPicker((objekt as Shape).Stroke.ToString());
                    string indexBarvy = barva.Index.ToString() + ";";
                    string tloustka = ((objekt as Shape).StrokeThickness - 1).ToString() + ";";
                    if (objekt is Ellipse)
                    {
                        Ellipse elipsa = objekt as Ellipse;
                        if (elipsa.ToolTip.ToString().Last() == '²' || elipsa.ToolTip.ToString()[16] == 'x')
                        {
                            string souradnice = elipsa.ToolTip.ToString();
                            if (souradnice.Last() == '0')
                            {
                                string[] split = souradnice.Split(':');
                                souradnice = split[1].Trim();
                                string[] paramtery = souradnice.Split('+');
                                string[] m = paramtery[2].Split('x');
                                vystup  += m[0].Trim() + ";";
                                string[] n = paramtery[3].Split('y');
                                vystup += n[0].Trim() + ";";
                                string[] p = paramtery[4].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Kružnice" + ";OBE");
                            }
                            else
                            {
                                string[] parametry = souradnice.Split('²');
                                string[] souradniceM = parametry[0].Split('(');
                                vystup += souradniceM[2].Replace(')', ' ').Trim() + ";";
                                souradniceM = parametry[1].Split('(');
                                vystup += souradniceM[2].Replace(')', ' ').Trim() + ";";
                                vystup += parametry[2].Replace('=', ' ').Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Kružnice" + ";PAR");
                            }
                        }
                        else
                        {
                            string souradnice = elipsa.ToolTip.ToString();
                            if (souradnice.Last() == '0')
                            {
                                string[] split = souradnice.Split(':');
                                souradnice = split[1].Trim();
                                rbObe.IsChecked = true;
                                rbPar.IsChecked = false;
                                rbElipsa.IsChecked = true;
                                rbElipsa_Checked(true, null);
                                string[] paramtery = souradnice.Split('+');
                                string[] x = paramtery[0].Split('x');
                                vystup += x[0].Trim() + ";";
                                string[] y = paramtery[1].Split('y');
                                vystup += y[0].Trim() + ";";
                                string[] m = paramtery[2].Split('x');
                                vystup += m[0].Trim() + ";";
                                string[] n = paramtery[3].Split('y');
                                vystup += n[0].Trim() + ";";
                                string[] p = paramtery[4].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Elipsa" + ";OBE");
                            }
                            else
                            {
                                string[] text = souradnice.Split('²');
                                string[] m = text[0].Split('(');
                                vystup += m[3].Replace(')', ' ').Trim() + ";";
                                m = text[2].Split('(');
                                vystup += m[3].Replace(')', ' ').Trim() + ";";
                                vystup += text[1].Replace('/', ' ').Trim() + ";";
                                vystup += text[3].Replace('/', ' ').Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Elipsa" + ";PAR");
                            }
                            
                        }
                    }
                    else if (objekt is Line)
                    {
                        Line primka = objekt as Line;
                        if (primka.ToolTip.ToString()[0] == 'V' || primka.ToolTip.ToString()[0] == 'O' || primka.ToolTip.ToString().Contains('²'))
                        {                          
                        }
                        else
                        {
                            string souradnice = primka.ToolTip.ToString();
                            if (souradnice.Last() == '0')
                            {
                                string[] split = souradnice.Split(':');
                                souradnice = split[1].Trim();
                                string[] paramtery = souradnice.Split('+');
                                string[] m = paramtery[0].Split('x');
                                vystup += m[0].Trim() + ";";
                                string[] n = paramtery[1].Split('y');
                                vystup += n[0].Trim() + ";";
                                string[] p = paramtery[2].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Přímka;OBE");
                            }
                            else
                            {
                                string[] text = souradnice.Split('=');
                                string[] parametry = text[1].Replace('+', ' ').Replace('t', ' ').Split('*');
                                string a = parametry[0].Replace('[', ' ').Replace(']', ' ').Trim() + ";";
                                string u = parametry[1].Replace('(', ' ').Replace(')', ' ').Trim() + ";";
                                vystup = a + u;
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Přímka;PAR");
                            }
                        }
                    }
                    else if (objekt is Path)
                    {
                        Path bod = objekt as Path;
                        string souradnice = bod.ToolTip.ToString();
                        string index = "0;";
                        if (souradnice[0] == '[')
                        {
                            string[] text = souradnice.Split(';');
                            vystup += text[0].Replace('[', ' ').Trim() + ";";
                            vystup += text[1].Replace(']', ' ').Trim() + ";";
                            zapis.WriteLine(vystup + indexBarvy + tloustka + "Bod");
                        }
                        else if (souradnice.Last() == '0')
                        {
                            string[] split = souradnice.Split(':');
                            souradnice = split[1].Trim();
                            string[] paramtery = souradnice.Split('+');
                            if (paramtery[0].Contains('x'))
                            {
                                index = "0;";
                                string[] x = paramtery[0].Split('x');
                                vystup+= x[0].Trim() + ";";
                            }
                            else
                            {
                                index = "1;";
                                string[] y = paramtery[0].Split('y');
                                vystup+= y[0].Trim() + ";";
                            }
                            string[] m = paramtery[1].Split('x');
                            vystup += m[0].Trim() + ";";
                            string[] n = paramtery[2].Split('y');
                            vystup += n[0].Trim() + ";";
                            string[] p = paramtery[3].Split('=');
                            vystup += p[0].Trim() + ";";
                            zapis.WriteLine(vystup + index + indexBarvy + tloustka + "Parabola;OBE");
                        }
                        else
                        {
                            string m, n, p;
                            if (souradnice[1] == 'x')
                            {
                                string[] data = souradnice.Split('=');
                                if (data[1][1] == '-')
                                {
                                    index = "1;";
                                }
                                string[] dataM = data[0].Split(')');
                                dataM = dataM[0].Split('(');
                                m = dataM[2].Trim();
                                dataM = data[1].Split(')');
                                data = dataM[1].Split('(');
                                n = data[2];
                                data = dataM[0].Split('(');
                                p = data[1];
                            }
                            else
                            {
                                string[] data = souradnice.Split('=');
                                if (data[1][1] == '-')
                                {
                                    index = "3;";
                                }
                                else
                                {
                                    index = "2;";
                                }
                                string[] dataM = data[0].Split(')');
                                dataM = dataM[0].Split('(');
                                n = dataM[2].Trim();
                                dataM = data[1].Split(')');
                                data = dataM[1].Split('(');
                                m = data[2];
                                data = dataM[0].Split('(');
                                p = data[1];
                            }
                            vystup = m + ";" + n + ";" + p + ";";
                            zapis.WriteLine(vystup + index + indexBarvy + tloustka + "Parabola;PAR");
                        }
                    }
                    else if (objekt is Polyline)
                    {
                        Polyline hyperbola = objekt as Polyline;
                        if (toolTip != hyperbola.ToolTip.ToString() && hyperbola.ToolTip.ToString() != "")
                        {
                            toolTip = hyperbola.ToolTip.ToString();
                            string index = "0;";
                            if (toolTip.Last() == '0')
                            {
                                string[] split = toolTip.Split(':');
                                toolTip = split[1].Trim();
                                string[] paramtery = toolTip.Split('+');
                                string[] x = paramtery[0].Split('x');
                                vystup += x[0].Trim() + ";";
                                string[] y = paramtery[1].Split('y');
                                vystup += y[0].Trim() + ";";
                                string[] mO = paramtery[2].Split('x');
                                vystup += mO[0].Trim() + ";";
                                string[] nO = paramtery[3].Split('y');
                                vystup += nO[0].Trim() + ";";
                                string[] p = paramtery[4].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + index + indexBarvy + tloustka + "Hyperbola;OBE");
                            }
                            else
                            {
                                if (toolTip[0] == '-')
                                {
                                    index = "1;";
                                }
                                string[] text = toolTip.Split('²');
                                string[] data = text[0].Split('(');
                                string m = data[3].Replace(')', ' ').Trim();
                                string a = text[1].Replace('/', ' ').Trim();
                                data = text[2].Split('(');
                                string n = data[3].Replace(')', ' ').Trim();
                                string b = text[3].Replace('/', ' ').Trim();
                                zapis.WriteLine(m + ";" + n + ";" + a + ";" + b + ";" + index + indexBarvy + tloustka + "Hyperbola;PAR");
                            }
                        }
                    }
                }
                zapis.Dispose();
            }
        }
        //Ukládání do souborů (Uložit jako)
        private void SaveAsFile(object sender, RoutedEventArgs e)
        {
            SaveFileDialog ulozit = new SaveFileDialog();
            ulozit.Filter = "Projekt formátu dmp (.pdmp)|*.pdmp";
            var result = ulozit.ShowDialog();
            if (result == true)
            {
                cesta = ulozit.FileName;
                System.IO.StreamWriter zapis = new System.IO.StreamWriter(cesta);
                string toolTip = " ";
                foreach (object objekt in vsechnyTvary)
                {
                    string vystup = "";
                    ColorPicker barva = new ColorPicker((objekt as Shape).Stroke.ToString());
                    string indexBarvy = barva.Index.ToString() + ";";
                    string tloustka = ((objekt as Shape).StrokeThickness - 1).ToString() + ";";
                    if (objekt is Ellipse)
                    {
                        Ellipse elipsa = objekt as Ellipse;
                        if (elipsa.ToolTip.ToString().Last() == '²' || elipsa.ToolTip.ToString()[16] == 'x')
                        {
                            string souradnice = elipsa.ToolTip.ToString();
                            if (souradnice.Last() == '0')
                            {
                                string[] split = souradnice.Split(':');
                                souradnice = split[1].Trim();
                                string[] paramtery = souradnice.Split('+');
                                string[] m = paramtery[2].Split('x');
                                vystup += m[0].Trim() + ";";
                                string[] n = paramtery[3].Split('y');
                                vystup += n[0].Trim() + ";";
                                string[] p = paramtery[4].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Kružnice" + ";OBE");
                            }
                            else
                            {
                                string[] parametry = souradnice.Split('²');
                                string[] souradniceM = parametry[0].Split('(');
                                vystup += souradniceM[2].Replace(')', ' ').Trim() + ";";
                                souradniceM = parametry[1].Split('(');
                                vystup += souradniceM[2].Replace(')', ' ').Trim() + ";";
                                vystup += parametry[2].Replace('=', ' ').Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Kružnice" + ";PAR");
                            }
                        }
                        else
                        {
                            string souradnice = elipsa.ToolTip.ToString();
                            if (souradnice.Last() == '0')
                            {
                                string[] split = souradnice.Split(':');
                                souradnice = split[1].Trim();
                                rbObe.IsChecked = true;
                                rbPar.IsChecked = false;
                                rbElipsa.IsChecked = true;
                                rbElipsa_Checked(true, null);
                                string[] paramtery = souradnice.Split('+');
                                string[] x = paramtery[0].Split('x');
                                vystup += x[0].Trim() + ";";
                                string[] y = paramtery[1].Split('y');
                                vystup += y[0].Trim() + ";";
                                string[] m = paramtery[2].Split('x');
                                vystup += m[0].Trim() + ";";
                                string[] n = paramtery[3].Split('y');
                                vystup += n[0].Trim() + ";";
                                string[] p = paramtery[4].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Elipsa" + ";OBE");
                            }
                            else
                            {
                                string[] text = souradnice.Split('²');
                                string[] m = text[0].Split('(');
                                vystup += m[3].Replace(')', ' ').Trim() + ";";
                                m = text[2].Split('(');
                                vystup += m[3].Replace(')', ' ').Trim() + ";";
                                vystup += text[1].Replace('/', ' ').Trim() + ";";
                                vystup += text[3].Replace('/', ' ').Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Elipsa" + ";PAR");
                            }

                        }
                    }
                    else if (objekt is Line)
                    {
                        Line primka = objekt as Line;
                        if (primka.ToolTip.ToString()[0] == 'V' || primka.ToolTip.ToString()[0] == 'O' || primka.ToolTip.ToString().Contains('²'))
                        {
                        }
                        else
                        {
                            string souradnice = primka.ToolTip.ToString();
                            if (souradnice.Last() == '0')
                            {
                                string[] split = souradnice.Split(':');
                                souradnice = split[1].Trim();
                                string[] paramtery = souradnice.Split('+');
                                string[] m = paramtery[0].Split('x');
                                vystup += m[0].Trim() + ";";
                                string[] n = paramtery[1].Split('y');
                                vystup += n[0].Trim() + ";";
                                string[] p = paramtery[2].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Přímka;OBE");
                            }
                            else
                            {
                                string[] text = souradnice.Split('=');
                                string[] parametry = text[1].Replace('+', ' ').Replace('t', ' ').Split('*');
                                string a = parametry[0].Replace('[', ' ').Replace(']', ' ').Trim() + ";";
                                string u = parametry[1].Replace('(', ' ').Replace(')', ' ').Trim() + ";";
                                vystup = a + u;
                                zapis.WriteLine(vystup + indexBarvy + tloustka + "Přímka;PAR");
                            }
                        }
                    }
                    else if (objekt is Path)
                    {
                        Path bod = objekt as Path;
                        string souradnice = bod.ToolTip.ToString();
                        string index = "0;";
                        if (souradnice[0] == '[')
                        {
                            string[] text = souradnice.Split(';');
                            vystup += text[0].Replace('[', ' ').Trim() + ";";
                            vystup += text[1].Replace(']', ' ').Trim() + ";";
                            zapis.WriteLine(vystup + indexBarvy + tloustka + "Bod");
                        }
                        else if (souradnice.Last() == '0')
                        {
                            string[] split = souradnice.Split(':');
                            souradnice = split[1].Trim();
                            string[] paramtery = souradnice.Split('+');
                            if (paramtery[0].Contains('x'))
                            {
                                index = "0;";
                                string[] x = paramtery[0].Split('x');
                                vystup += x[0].Trim() + ";";
                            }
                            else
                            {
                                index = "1;";
                                string[] y = paramtery[0].Split('y');
                                vystup += y[0].Trim() + ";";
                            }
                            string[] m = paramtery[1].Split('x');
                            vystup += m[0].Trim() + ";";
                            string[] n = paramtery[2].Split('y');
                            vystup += n[0].Trim() + ";";
                            string[] p = paramtery[3].Split('=');
                            vystup += p[0].Trim() + ";";
                            zapis.WriteLine(vystup + index + indexBarvy + tloustka + "Parabola;OBE");
                        }
                        else
                        {
                            string m, n, p;
                            if (souradnice[1] == 'x')
                            {
                                string[] data = souradnice.Split('=');
                                if (data[1][1] == '-')
                                {
                                    index = "1;";
                                }
                                string[] dataM = data[0].Split(')');
                                dataM = dataM[0].Split('(');
                                m = dataM[2].Trim();
                                dataM = data[1].Split(')');
                                data = dataM[1].Split('(');
                                n = data[2];
                                data = dataM[0].Split('(');
                                p = data[1];
                            }
                            else
                            {
                                string[] data = souradnice.Split('=');
                                if (data[1][1] == '-')
                                {
                                    index = "3;";
                                }
                                else
                                {
                                    index = "2;";
                                }
                                string[] dataM = data[0].Split(')');
                                dataM = dataM[0].Split('(');
                                n = dataM[2].Trim();
                                dataM = data[1].Split(')');
                                data = dataM[1].Split('(');
                                m = data[2];
                                data = dataM[0].Split('(');
                                p = data[1];
                            }
                            vystup = m + ";" + n + ";" + p + ";";
                            zapis.WriteLine(vystup + index + indexBarvy + tloustka + "Parabola;PAR");
                        }
                    }
                    else if (objekt is Polyline)
                    {
                        Polyline hyperbola = objekt as Polyline;
                        if (toolTip != hyperbola.ToolTip.ToString() && hyperbola.ToolTip.ToString() != "")
                        {
                            toolTip = hyperbola.ToolTip.ToString();
                            string index = "0;";
                            if (toolTip.Last() == '0')
                            {
                                string[] split = toolTip.Split(':');
                                toolTip = split[1].Trim();
                                string[] paramtery = toolTip.Split('+');
                                string[] x = paramtery[0].Split('x');
                                vystup += x[0].Trim() + ";";
                                string[] y = paramtery[1].Split('y');
                                vystup += y[0].Trim() + ";";
                                string[] mO = paramtery[2].Split('x');
                                vystup += mO[0].Trim() + ";";
                                string[] nO = paramtery[3].Split('y');
                                vystup += nO[0].Trim() + ";";
                                string[] p = paramtery[4].Split('=');
                                vystup += p[0].Trim() + ";";
                                zapis.WriteLine(vystup + index + indexBarvy + tloustka + "Hyperbola;OBE");
                            }
                            else
                            {
                                if (toolTip[0] == '-')
                                {
                                    index = "1;";
                                }
                                string[] text = toolTip.Split('²');
                                string[] data = text[0].Split('(');
                                string m = data[3].Replace(')', ' ').Trim();
                                string a = text[1].Replace('/', ' ').Trim();
                                data = text[2].Split('(');
                                string n = data[3].Replace(')', ' ').Trim();
                                string b = text[3].Replace('/', ' ').Trim();
                                zapis.WriteLine(m + ";" + n + ";" + a + ";" + b + ";" + index + indexBarvy + tloustka + "Hyperbola;PAR");
                            }
                        }
                    }
                }
                zapis.Dispose();
            }
        }
        //Otevírání souborů
        private void OpenFile(object sender, RoutedEventArgs e)
        {
            if(vsechnyTvary.Count != 0)
            {
                MessageBoxResult ulozitAnoNe = MessageBox.Show("Přejete si uložit změny?", "Uložit projekt", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if(ulozitAnoNe == MessageBoxResult.Yes)
                {
                    SaveFile(true, new RoutedEventArgs());
                }
                else if(ulozitAnoNe == MessageBoxResult.No)
                {
                    btReset_Click(true, null);
                }
            }
            foreach(UIElement objekt in vsechnyTvary)
            {
                souradnySystem.Children.Remove(objekt);
            }
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Projekt formátu dmp (.pdmp)|*.pdmp";
            var result = openFile.ShowDialog();
            if(result == true)
            {
                System.IO.StreamReader nacist = new System.IO.StreamReader(openFile.FileName);
                string radek;
                while((radek = nacist.ReadLine()) != null)
                {
                    string[] parametry = radek.Split(';');
                    switch (parametry[parametry.Length - 2])
                    {
                        case "Bod":
                            {
                                Bod bod = new Bod(parametry[0], parametry[1], Convert.ToInt32(parametry[2]), Convert.ToInt32(parametry[3]));
                                souradnySystem.Children.Add(bod.VykresleniBodu());
                                vsechnyTvary.Add(bod.VykresleniBodu());
                                break;
                            }
                        case "Přímka":
                            {
                                Primka primka;
                                if (parametry[parametry.Length - 1] == "OBE")
                                {
                                    primka = new Primka(parametry[0], parametry[1], parametry[2], "Obecná", Convert.ToInt32(parametry[3]), Convert.ToInt32(parametry[4]));
                                }
                                else
                                {
                                    primka = new Primka(parametry[0], parametry[1], parametry[2], parametry[3], Convert.ToInt32(parametry[4]), Convert.ToInt32(parametry[5]));
                                }
                                souradnySystem.Children.Add(primka.VykresleniPrimky());
                                vsechnyTvary.Add(primka.VykresleniPrimky());
                                break;
                            }
                        case "Kružnice":
                            {
                                Kruznice kruznice;
                                if (parametry[parametry.Length - 1] == "OBE")
                                {
                                    double[] parametryK = doplneniNaCtevrec("1", "1", parametry[0], parametry[1], parametry[2]);
                                    kruznice = new Kruznice(parametryK[0].ToString() + ";" + parametryK[6].ToString() + ";" + parametryK[7].ToString() + ";" + parametryK[8].ToString() + ";" + parametryK[9].ToString() + ";" + parametryK[10].ToString(), parametryK[1].ToString(), parametryK[4].ToString(), Convert.ToInt32(parametry[3]), Convert.ToInt32(parametry[4]));
                                }
                                else
                                {
                                    kruznice = new Kruznice(parametry[0], parametry[1], parametry[2], Convert.ToInt32(parametry[3]), Convert.ToInt32(parametry[4]));
                                }
                                souradnySystem.Children.Add(kruznice.VykresleniKruznice());
                                vsechnyTvary.Add(kruznice.VykresleniKruznice());
                                break;
                            }
                        case "Elipsa":
                            {
                                Elipsa elipsa;
                                if (parametry[parametry.Length - 1] == "OBE")
                                {
                                    double[] parametryE = doplneniNaCtevrec(parametry[0], parametry[1], parametry[2], parametry[3], parametry[4]);
                                    elipsa = new Elipsa(parametryE[0].ToString() + ";" + parametryE[6].ToString() + ";" + parametryE[7].ToString() + ";" + parametryE[8].ToString() + ";" + parametryE[9].ToString() + ";" + parametryE[10].ToString(), parametryE[1].ToString(), Math.Sqrt(parametryE[2]).ToString(), Math.Sqrt(parametryE[3]).ToString(), Convert.ToInt32(parametry[4]), Convert.ToInt32(parametry[5]));
                                }
                                else
                                {
                                    elipsa = new Elipsa(parametry[0], parametry[1], parametry[2], parametry[3], Convert.ToInt32(parametry[4]), Convert.ToInt32(parametry[5]));
                                }
                                souradnySystem.Children.Add(elipsa.VykresleniElipsy());
                                vsechnyTvary.Add(elipsa.VykresleniElipsy());
                                break;
                            }
                        case "Parabola":
                            {
                                Parabola parabola;
                                if (parametry[parametry.Length - 1] == "OBE")
                                {
                                    double[] parametryP;
                                    if (parametry[4] == "1")
                                    {
                                        parametryP = doplneniNaCtevrec("0", parametry[0], parametry[1], parametry[2], parametry[3]);
                                    }
                                    else
                                    {
                                        parametryP = doplneniNaCtevrec(parametry[0], "0",parametry[1], parametry[2], parametry[3]);
                                    }
                                    parabola = new Parabola(parametryP[0].ToString() + ";" + parametryP[6].ToString() + ";" + parametryP[7].ToString() + ";" + parametryP[8].ToString() + ";" + parametryP[9].ToString() + ";" + parametryP[10].ToString(), parametryP[1].ToString(), Math.Abs(parametryP[2]).ToString(), (int)parametryP[3], Convert.ToInt32(parametry[4]), Convert.ToInt32(parametry[5]), ridiciPrimky);
                                }
                                else
                                {
                                    parabola = new Parabola(parametry[0], parametry[1], parametry[2], Convert.ToInt32(parametry[3]), Convert.ToInt32(parametry[4]), Convert.ToInt32(parametry[5]), ridiciPrimky);
                                }
                                souradnySystem.Children.Add(parabola.VykresleniParaboly());
                                vsechnyTvary.Add(parabola.VykresleniParaboly());
                                souradnySystem.Children.Add(parabola.vykresleniRidiciPrimky());
                                vsechnyTvary.Add(parabola.vykresleniRidiciPrimky());
                                break;
                            }
                        case "Hyperbola":
                            {
                                Hyperbola hyperbola;
                                if (parametry[parametry.Length - 1] == "OBE")
                                {
                                    double[] parametryH = doplneniNaCtevrec(parametry[0], parametry[1], parametry[2], parametry[3], parametry[4]);
                                    hyperbola = new Hyperbola(parametryH[0].ToString(), parametryH[1].ToString(), parametryH[2].ToString(), parametryH[2].ToString() + ";" + parametryH[6].ToString() + ";" + parametryH[7].ToString() + ";" + parametryH[8].ToString() + ";" + parametryH[9].ToString() + ";" + parametryH[10].ToString(), parametryH[3], Convert.ToInt32(parametry[5]), Convert.ToInt32(parametry[6]), asymptoty);

                                }
                                else
                                {
                                    hyperbola = new Hyperbola(parametry[0], parametry[1], parametry[2], parametry[3], Convert.ToInt32(parametry[4]), Convert.ToInt32(parametry[5]), Convert.ToInt32(parametry[6]), asymptoty);
                                }
                                souradnySystem.Children.Add(hyperbola.VykresleniHyperboly1());
                                vsechnyTvary.Add(hyperbola.VykresleniHyperboly1());
                                souradnySystem.Children.Add(hyperbola.VykresleniHyperboly2());
                                vsechnyTvary.Add(hyperbola.VykresleniHyperboly2());
                                souradnySystem.Children.Add(hyperbola.VykresleniHyperboly3());
                                vsechnyTvary.Add(hyperbola.VykresleniHyperboly3());
                                souradnySystem.Children.Add(hyperbola.VykresleniHyperboly4());
                                vsechnyTvary.Add(hyperbola.VykresleniHyperboly4());
                                souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty1());
                                vsechnyTvary.Add(hyperbola.VykresleniAsymptoty1());
                                souradnySystem.Children.Add(hyperbola.VykresleniAsymptoty2());
                                vsechnyTvary.Add(hyperbola.VykresleniAsymptoty2());
                                break;
                            }
                    }
                }
                nacist.Dispose();
                GC.Collect();
            }
        }

        private void BgColor_Changed(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            var result = colorDialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush barvaPozadi = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                this.Background = barvaPozadi;
            }
        }
        private void ridiciPrimka_Checked(object sender, RoutedEventArgs e)
        {
            ridiciPrimky = true;
        }
        private void ridiciPrimka_Unchecked(object sender, RoutedEventArgs e)
        {
            ridiciPrimky = false;
        }
        private void asymptoty_Checked(object sender, RoutedEventArgs e)
        {
            asymptoty = true;
        }

        private void asymptoty_Unchecked(object sender, RoutedEventArgs e)
        {
            asymptoty = false;
        }
        private void mItemParametricke_Click(object sender, RoutedEventArgs e)
        {
            rbPar.IsChecked = true;
            rbObe.IsChecked = false;
            tbX.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            tbB.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            tbP.Visibility = Visibility.Hidden;
            lbP.Visibility = Visibility.Hidden;
            if (rbPrimka.IsChecked == true)
            {
                rbPrimka_Checked(true, null);
            }
            else if (rbKruznice.IsChecked == true)
            {
                rbKruznice_Checked(true, null);
            }
            else if (rbElipsa.IsChecked == true)
            {
                rbElipsa_Checked(true, null);
            }
            else if (rbParabola.IsChecked == true)
            {
                rbParabola_Checked(true, null);
            }
            else if (rbHyperbola.IsChecked == true)
            {
                rbHyperbola_Checked(true, null);
            }
        }

        private void mItemObecne_Click(object sender, RoutedEventArgs e)
        {
            rbObe.IsChecked = true;
            rbPar.IsChecked = false;
            if (rbPrimka.IsChecked == true)
            {
                rbPrimka_Checked(true, null);
            }
            else if (rbKruznice.IsChecked == true)
            {
                rbKruznice_Checked(true, null);
            }
            else if (rbElipsa.IsChecked == true)
            {
                rbElipsa_Checked(true, null);
            }
            else if (rbParabola.IsChecked == true)
            {
                rbParabola_Checked(true, null);
            }
            else if (rbHyperbola.IsChecked == true)
            {
                rbHyperbola_Checked(true, null);
            }
        }

        private void cbVzorec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(rbParabola.IsChecked == true)
            {
                if(cbVzorec.SelectedIndex == 0)
                {
                    lbX.Content = "K: ";
                }
                else
                {
                    lbX.Content = "L: ";
                }
            }
        }
        //Tlačítko mazání
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if (rbElipsa.IsChecked == true)
            {
                souradnySystem.Children.Remove(vybranaElipsa);
                vsechnyTvary.Remove(vybranaElipsa);
            }
            else if (rbKruznice.IsChecked == true)
            {
                souradnySystem.Children.Remove(vybranaKruznice);
                vsechnyTvary.Remove(vybranaKruznice);
            }
            else if (rbBod.IsChecked == true)
            {
                souradnySystem.Children.Remove(vybranyBod);
                vsechnyTvary.Remove(vybranyBod);
            }
            else if (rbPrimka.IsChecked == true)
            {
                souradnySystem.Children.Remove(vybranaPrimka);
                vsechnyTvary.Remove(vybranaPrimka);
            }
            else if (rbHyperbola.IsChecked == true)
            {
                foreach (Polyline hyperbola in vybranaHyperbola)
                {
                    souradnySystem.Children.Remove(hyperbola);
                    vsechnyTvary.Remove(hyperbola);
                }
            }
            else if (rbParabola.IsChecked == true)
            {
                souradnySystem.Children.Remove(vybranaParabola);
                vsechnyTvary.Remove(vybranaParabola);
                souradnySystem.Children.Remove(ridiciPrimka);
                vsechnyTvary.Remove(ridiciPrimka);
            }
            btVloz.Content = "Vložit";
            btVloz.Margin = new Thickness(217, 175, 0, 0);
            btVloz.Visibility = Visibility.Hidden;
            tbX.Visibility = Visibility.Hidden;
            tbY.Visibility = Visibility.Hidden;
            lbX.Visibility = Visibility.Hidden;
            lbY.Visibility = Visibility.Hidden;
            lbC.Visibility = Visibility.Hidden;
            tbC.Visibility = Visibility.Hidden;
            lbB.Visibility = Visibility.Hidden;
            tbB.Visibility = Visibility.Hidden;
            vybranaElipsa = new Ellipse();
            vybranaHyperbola = new List<Polyline>();
            vybranaKruznice = new Ellipse();
            vybranaParabola = new Path();
            vybranaPrimka = new Line();
            vybranyBod = new Path();
            ridiciPrimka = new Line();
            btDelete.Visibility = Visibility.Hidden;
            tbC.Text = "";
            tbX.Text = "";
            tbY.Text = "";
            tbB.Text = "";
            tbP.Text = "";
            cbVzorec.Items.Clear();
            zapnouttRadioButtony();
        }
        //Export Canvasu
        private void ExportImage(object sender, RoutedEventArgs e)
        {
            Canvas printCanvas = new Canvas();
            foreach(UIElement objekt in souradnySystem.Children)
            {
                var xaml = System.Windows.Markup.XamlWriter.Save(objekt);
                var deepCopy = System.Windows.Markup.XamlReader.Parse(xaml) as UIElement;
                printCanvas.Children.Add(deepCopy);
            }
            TextBlock data = new TextBlock();
            string tTip = "";
            foreach (UIElement objekt in vsechnyTvary)
            {
                if (tTip != (objekt as Shape).ToolTip.ToString() && (objekt as Shape).ToolTip.ToString() != " ")
                {

                    try
                    {
                        System.Windows.Documents.Run radek = new System.Windows.Documents.Run((objekt as Shape).ToolTip.ToString());
                        tTip = (objekt as Shape).ToolTip.ToString();
                        radek.Foreground = (objekt as Shape).Stroke;
                        if(radek.Foreground.ToString() == "")
                        {
                            
                        }
                        else
                        {
                            data.Inlines.Add(radek);
                            data.Inlines.Add(new System.Windows.Documents.LineBreak());
                        }
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            var pageSize = new Size(1600, 1600);
            var document = new System.Windows.Documents.FixedDocument();
            document.DocumentPaginator.PageSize = pageSize;
            souradnySystem.RenderTransform = new MatrixTransform(zacatecniPozice);
            var fixedPage = new System.Windows.Documents.FixedPage();
            fixedPage.Width = pageSize.Width;
            fixedPage.Height = pageSize.Height;
            fixedPage.Children.Add(printCanvas);
            fixedPage.Measure(pageSize);
            fixedPage.Arrange(new Rect(new Point(), pageSize));
            fixedPage.UpdateLayout();
            var pageContent = new System.Windows.Documents.PageContent();
            ((System.Windows.Markup.IAddChild)pageContent).AddChild(fixedPage);
            document.Pages.Add(pageContent);
            pageSize = new Size(8.26 * 96, 11.69 * 96);
            fixedPage = new System.Windows.Documents.FixedPage();
            fixedPage.Width = pageSize.Width;
            fixedPage.Height = pageSize.Height;
            fixedPage.Children.Add(data);
            fixedPage.Measure(pageSize);
            fixedPage.Arrange(new Rect(new Point(), pageSize));
            fixedPage.UpdateLayout();
            var pageContent1 = new System.Windows.Documents.PageContent();
            ((System.Windows.Markup.IAddChild)pageContent1).AddChild(fixedPage);
            document.Pages.Add(pageContent1);
            var pd = new PrintDialog();
            try
            {
                pd.PrintDocument(document.DocumentPaginator, "Souřadný systém");
            }
            catch
            {
                MessageBox.Show("Export se nezdařil!", "Neúspěšně!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MessageBox.Show("Projekt úspěšně vyexportován!", "Provedeno!", MessageBoxButton.OK, MessageBoxImage.Information);
            printCanvas.Children.Clear();
        }
        //Metoda dopnění na čtverec, převede obecnou rovnici na parametrickou
        private double[] doplneniNaCtevrec(string k, string l, string m, string n, string p)
        {
            double[] parametry = new double[11];
            if (rbParabola.IsChecked == true)
            {
                if (Convert.ToDouble(l) == 0)
                {
                    parametry[0] = -(Convert.ToDouble(m) / Convert.ToDouble(k) / 2);
                    parametry[2] = Convert.ToDouble(n) / Convert.ToDouble(k) / 2;
                    parametry[1] = -((Convert.ToDouble(p) / Convert.ToDouble(k) - Math.Pow(parametry[0], 2)) / (2 * parametry[2]));
                    if ((Convert.ToDouble(k) * Convert.ToDouble(n)) > 0)
                    {
                        parametry[3] = 1;
                    }
                    else
                    {
                        parametry[3] = 0;
                    }
                }
                else
                {
                    parametry[1] = -(Convert.ToDouble(n) / Convert.ToDouble(l) / 2);
                    parametry[2] = Convert.ToDouble(m) / Convert.ToDouble(l) / 2;
                    parametry[0] = -((Convert.ToDouble(p) / Convert.ToDouble(l) - Math.Pow(parametry[1], 2)) / (2 * parametry[2]));
                    if((Convert.ToDouble(l) * Convert.ToDouble(m)) > 0)
                    {
                        parametry[3] = 3;
                    }
                    else
                    {
                        parametry[3] = 2;
                    }
                }
                
            }
            else if(rbHyperbola.IsChecked == true)
            {
                parametry[0] = -(Convert.ToDouble(m) / Convert.ToDouble(k) / 2);
                parametry[1] = -(Convert.ToDouble(n) / Convert.ToDouble(l) / 2);
                if (Convert.ToDouble(k) < 0)
                {
                    if (double.IsNaN(Math.Sqrt(-Convert.ToDouble(p) + Math.Pow(Convert.ToDouble(parametry[0]), 2) * Convert.ToDouble(k) + Math.Pow(Convert.ToDouble(parametry[1]), 2) * Convert.ToDouble(l))))
                    {
                        parametry[2] = Math.Sqrt(Math.Abs(-Convert.ToDouble(p) + Math.Pow(Convert.ToDouble(parametry[0]), 2) * Convert.ToDouble(k) + Math.Pow(Convert.ToDouble(parametry[1]), 2) * Convert.ToDouble(l)));
                        parametry[3] = 0;
                    }
                    else
                    {
                        parametry[2] = Math.Sqrt(-Convert.ToDouble(p) + Math.Pow(Convert.ToDouble(parametry[0]), 2) * Convert.ToDouble(k) + Math.Pow(Convert.ToDouble(parametry[1]), 2) * Convert.ToDouble(l));
                        parametry[3] = 1;
                    }
                }
                else
                {
                    if (double.IsNaN(Math.Sqrt(-Convert.ToDouble(p) + Math.Pow(Convert.ToDouble(parametry[0]), 2) * Convert.ToDouble(k) + Math.Pow(Convert.ToDouble(parametry[1]), 2) * Convert.ToDouble(l))))
                    {
                        parametry[2] = Math.Sqrt(Math.Abs(-Convert.ToDouble(p) + Math.Pow(Convert.ToDouble(parametry[0]), 2) * Convert.ToDouble(k) + Math.Pow(Convert.ToDouble(parametry[1]), 2) * Convert.ToDouble(l)));
                        parametry[3] = 1;
                    }
                    else
                    {
                        parametry[2] = Math.Sqrt(-Convert.ToDouble(p) + Math.Pow(Convert.ToDouble(parametry[0]), 2) * Convert.ToDouble(k) + Math.Pow(Convert.ToDouble(parametry[1]), 2) * Convert.ToDouble(l));
                        parametry[3] = 0;
                    }
                }
                parametry[4] = Math.Abs(Convert.ToDouble(k));
                parametry[5] = Math.Abs(Convert.ToDouble(l));
            }
            else
            {
                parametry[0] = -(Convert.ToDouble(m) / Convert.ToDouble(k) / 2);
                parametry[1] = -(Convert.ToDouble(n) / Convert.ToDouble(l) / 2);
                parametry[2] = (-Convert.ToDouble(p) + Math.Abs(Math.Pow(parametry[0], 2) * Convert.ToDouble(k)) + Math.Abs(Math.Pow(parametry[1], 2) * Convert.ToDouble(l))) / Convert.ToDouble(k);
                parametry[3] = (-Convert.ToDouble(p) + Math.Abs(Math.Pow(parametry[0], 2) * Convert.ToDouble(k)) + Math.Abs(Math.Pow(parametry[1], 2) * Convert.ToDouble(l))) / Convert.ToDouble(l);
                parametry[4] = 0.5 * Math.Sqrt(Math.Pow(Convert.ToDouble(m), 2) + Math.Pow(Convert.ToDouble(n), 2) - 4 * Convert.ToDouble(p));
            }
            parametry[6] = Convert.ToDouble(k);
            parametry[7] = Convert.ToDouble(l);
            parametry[8] = Convert.ToDouble(m);
            parametry[9] = Convert.ToDouble(n);
            parametry[10] = Convert.ToDouble(p);
            return parametry;
        }
        //Nedokončená metoda pro reversní metodu k předochí, mělo být použito k pohybu rovnic vykreslené přes obecnou rovnici
        private double[] reversniDoplneniNaCtverec(string m, string n, string a, string b)
        {
            double[] parametry = new double[5];
            parametry[0] = Convert.ToDouble(a);
            parametry[1] = Convert.ToDouble(b);
            parametry[2] = parametry[0] * 2 * Convert.ToDouble(m);
            parametry[3] = parametry[1] * 2 * Convert.ToDouble(n);
            parametry[4] = (parametry[0] * Math.Pow(Convert.ToDouble(m), 2)) - (parametry[1] * Math.Pow(Convert.ToDouble(n), 2)) - (parametry[0] * parametry[1]);
            if(rbParabola.IsChecked == true)
            {
                if(parametry[0] == 0)
                {
                    parametry[4] = Math.Abs(Convert.ToDouble(parametry[2]) * Convert.ToDouble(m)) + (Convert.ToDouble(parametry[1]) * Math.Pow(Convert.ToDouble(n),2)); 
                }
                else
                {
                    parametry[4] = Math.Abs(Convert.ToDouble(parametry[3]) * Convert.ToDouble(n)) + (Convert.ToDouble(parametry[0]) * Math.Pow(Convert.ToDouble(m), 2));
                }
            }
            if(rbPrimka.IsChecked == true)
            {
                parametry[0] = 0;
                parametry[1] = -(Convert.ToDouble(n) / Convert.ToDouble(a));
            }
            return parametry;
        }
    }
}

