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

        public static Color GetUniqueColor(int integer) {

            Random rnd = new Random(integer);
            int hash = rnd.Next();

            double iterator = Math.Abs((double)hash / (double)int.MaxValue);

            byte high = 235;
            byte low = 100;

            if (iterator < 1.0 / 6.0) {
                return Color.FromRgb(high, (byte)((iterator * 6.0) * high), low);
            } else if (iterator < 2.0 / 6.0) {
                return Color.FromRgb((byte)((2.0 - iterator * 6.0) * high), high, low);
            } else if (iterator < 3.0 / 6.0) {
                return Color.FromRgb(low, high, (byte)((iterator * 2.0) * high));
            } else if (iterator < 4.0 / 6.0) {
                return Color.FromRgb(low, (byte)((4.0 - iterator * 6.0) * high), high);
            } else if (iterator < 5.0 / 6.0) {
                return Color.FromRgb((byte)((iterator * 6.0 / 5.0) * high), low, high);
            } else {
                return Color.FromRgb(high, low, (byte)((6.0 - iterator * 6.0) * high));
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
            catch (Exception exception) {
                Console.WriteLine("Initialize Component Error : " + exception);
            }
        }
    }
}
