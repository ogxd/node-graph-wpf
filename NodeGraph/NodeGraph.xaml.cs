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

namespace Ogxd.NodeGraph {
    /// <summary>
    /// Interaction logic for NodeGraph.xaml
    /// </summary>
    public partial class NodeGraph : Border {

        // #131a24 // 19 26 36
        // #28303a

        public NodeGraph() {
            InitializeComponent();

            this.MouseDown += NodeGraph_MouseDown;
        }

        private void NodeGraph_MouseDown(object sender, MouseButtonEventArgs e) {
            if (Pipe.EditingPipe != null && Mouse.DirectlyOver == this) {
                Pipe.EditingPipe.Dispose();
                Pipe.EditingPipe = null;
            }
        }

        public void addNode(Node node) {
            canvas.Children.Add(node);
        }

        public void tryAddNode(Node node) {
            if (!canvas.Children.Contains(node))
                canvas.Children.Add(node);
        }

        public void removeNode(Node node) {
            canvas.Children.Remove(node);
        }

        public void tryRemoveNode(Node node) {
            if (canvas.Children.Contains(node))
                canvas.Children.Remove(node);
        }
    }
}
