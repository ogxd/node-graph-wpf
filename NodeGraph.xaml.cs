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

namespace WpfApp2 {
    /// <summary>
    /// Interaction logic for NodeGraph.xaml
    /// </summary>
    public partial class NodeGraph : Border {

        // #131a24 // 19 26 36
        // #28303a

        public NodeGraph() {
            InitializeComponent();

            this.MouseDown += NodeGraph_MouseDown;

            canvas.Children.Add(new ReaderNode(canvas) { position = new Point(10, 10)});
            canvas.Children.Add(new NormalsNode(canvas) { position = new Point(210, 110) });
            canvas.Children.Add(new WriterNode(canvas) { position = new Point(410, 210) });
        }

        private void NodeGraph_MouseDown(object sender, MouseButtonEventArgs e) {
            if (Pipe.EditingPipe != null && Mouse.DirectlyOver == this) {
                Pipe.EditingPipe.Dispose();
                Pipe.EditingPipe = null;
            }
        }
    }
}
