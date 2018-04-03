using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public delegate void VoidHandler();

    public interface IProperty
    {
        string label { get; set; }
        object value { get; set; }
        event VoidHandler valueChanged;
    }

    /// <summary>
    /// Logique d'interaction pour TextProperty.xaml
    /// </summary>
    public partial class TextProperty : Grid, IProperty
    {
        public event VoidHandler valueChanged;

        public TextProperty() {
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
            set { _value = fieldUI.Text = value.ToString(); }
        }

        private void fieldUI_TextChanged(object sender, TextChangedEventArgs e) {
            _value = fieldUI.Text;
            valueChanged?.Invoke();
        }
    }
}
