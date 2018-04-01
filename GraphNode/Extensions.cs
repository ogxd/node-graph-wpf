using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2 {

    public static class Extensions {

        public static Brush GetBrush(int type) {
            int a = type % 10;
            switch (a) {
                case 0:
                    return new SolidColorBrush(Color.FromRgb(255, 148, 77));
                case 1:
                    return new SolidColorBrush(Color.FromRgb(77, 166, 255)); 
                case 2:
                    return Brushes.Green;
                case 3:
                    return Brushes.Red;
                case 4:
                    return Brushes.Red;
                case 5:
                    return Brushes.Red;
                case 6:
                    return Brushes.Red;
                case 7:
                    return Brushes.Red;
                case 8:
                    return Brushes.Red;
                case 9:
                    return Brushes.Red;
                default:
                    return Brushes.Black;
            }
        }
    }
}
