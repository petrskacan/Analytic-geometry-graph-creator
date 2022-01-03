using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DMPAplikaceProSŠ
{
    class ColorPicker
    {
        private SolidColorBrush barva = new SolidColorBrush();
        private int index;
        public ColorPicker(int index)
        {
            switch (index)
            {
                case 0:
                    {
                        barva = Brushes.Red;
                        break;
                    }
                case 1:
                    {
                        barva = Brushes.DodgerBlue;
                        break;
                    }
                case 2:
                    {
                        barva = Brushes.LimeGreen;
                        break;
                    }
                case 3:
                    {
                        barva = Brushes.MediumVioletRed;
                        break;
                    }
                case 4:
                    {
                        barva = Brushes.Sienna;
                        break;
                    }
            }
        }
        public ColorPicker(string barva)
        {
            switch (barva)
            {
                case "#FFFF0000":
                    {
                        index = 0;
                        break;
                    }
                case "#FF1E90FF":
                    {
                        index = 1;
                        break;
                    }
                case "#FF32CD32":
                    {
                        index = 2;
                        break;
                    }
                case "#FFC71585":
                    {
                        index = 3;
                        break;
                    }
                case "#FFA0522D":
                    {
                        index = 4;
                        break;
                    }
                default:
                    {
                        index = 0;
                        break;
                    }
            }
        }
        public SolidColorBrush Barva
        {
            get { return barva; }
        }
        public int Index
        {
            get { return index; }
        }
    }
}
