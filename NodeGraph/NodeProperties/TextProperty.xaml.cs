using System.Windows.Controls;

namespace Ogxd.NodeGraph {

    public partial class TextProperty : Grid, IProperty {

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
