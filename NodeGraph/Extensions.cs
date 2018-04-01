using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Ogxd.NodeGraph {

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

        public static void LoadViewFromUri(this UIElement userControl, string baseUri) {
            try {
                var resourceLocater = new Uri(baseUri, UriKind.Relative);
                var exprCa = (PackagePart)typeof(Application).GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { resourceLocater });
                var stream = exprCa.GetStream();
                var uri = new Uri((Uri)typeof(BaseUriHelper).GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null, null), resourceLocater);
                var parserContext = new ParserContext {
                    BaseUri = uri
                };
                typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { stream, parserContext, userControl, true });
            }
            catch (Exception) {
                //log
            }
        }
    }
}
