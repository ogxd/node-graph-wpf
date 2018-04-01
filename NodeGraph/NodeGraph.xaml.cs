using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public double pipeStiffness { get; set; } = 50;

        public delegate Brush TypeToBrushHandler(int type);

        public TypeToBrushHandler typeToBrushHandler { get; set; } = (type) => {
            return new SolidColorBrush(Extensions.GetUniqueColor(type));
        };

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
            node.setConnections();
        }

        public void tryAddNode(Node node) {
            if (!canvas.Children.Contains(node)) {
                canvas.Children.Add(node);
                node.setConnections();
            }
        }

        public void removeNode(Node node) {
            canvas.Children.Remove(node);
        }

        public void tryRemoveNode(Node node) {
            if (canvas.Children.Contains(node)) {
                canvas.Children.Remove(node);
            }
        }

        public Brush getPipeColor(int type) {
            return typeToBrushHandler.Invoke(type);
        }

        public void process() {
            var nodes = canvas.Children.OfType<Node>();
            foreach (Node node in nodes) {
                foreach (Dock dock in node.outputs) {
                    if (dock.pipe != null) {
                        dock.pipe.result = null;
                    }
                }
            }
            foreach (Node node in nodes.Where(x => x.inputs.Length == 0)) {
                Thread thread = new Thread(new ThreadStart(() => {
                    node.queryProcess();
                }));
                thread.Start();
            }
        }
    }
}
