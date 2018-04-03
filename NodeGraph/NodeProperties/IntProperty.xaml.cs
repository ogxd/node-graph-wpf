using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ogxd.NodeGraph
{

    /// <summary>
    /// Logique d'interaction pour TextProperty.xaml
    /// </summary>
    public partial class IntProperty : Grid, IProperty
    {
        public event VoidHandler valueChanged;

        public IntProperty() {
            InitializeComponent();
        }

        // Cached label for thread safe get
        private string _label;
        public string label {
            get { return _label; }
            set { _label = labelUI.Text = value; }
        }

        // Cached value for thread safe get
        private object _value;
        public object value {
            get { return _value; }
            set { fieldUI.Text = value.ToString();
                _value = int.Parse(fieldUI.Text, CultureInfo.InvariantCulture);
            }
        }

        private void fieldUI_TextChanged(object sender, TextChangedEventArgs e) {
            _value = int.Parse(fieldUI.Text, CultureInfo.InvariantCulture);
            valueChanged?.Invoke();
        }

        private void fieldUI_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text) {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }
    }
}
