using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ogxd.NodeGraph {

    public partial class NodeGraph : Border {

        public readonly NodeGraphContext context;

        private double _pipeStiffness = 50;
        public double pipeStiffness {
            get {
                return _pipeStiffness;
            }
            set {
                if (_pipeStiffness == value)
                    return;
            }
        }

        public NodeGraph(NodeGraphContext context) {
            InitializeComponent();

            context.propertyChanged += Context_propertyChanged;
            this.context = context;

            this.MouseDown += NodeGraph_MouseDown;
            this.DragEnter += Canvas_DragEnter;
            this.Drop += NodeGraph_Drop;
            this.AllowDrop = true;
        }

        private void Context_propertyChanged(string propertyName) {
            switch (propertyName) {
                case "orientation":
                    foreach (Node node in getNodes()) {
                        node.updateOrientation();
                    }
                    break;
            }
        }

        private void NodeGraph_Drop(object sender, DragEventArgs e) {
            Node node = e.Data.GetData("node") as Node;
            if (node == null)
                return;
            Node copy = Activator.CreateInstance(node.GetType()) as Node;
            addNode(copy);
            copy.position = e.GetPosition(canvas);
        }

        private void Canvas_DragEnter(object sender, DragEventArgs e) {
            Node node = e.Data.GetData("node") as Node;
            if (node == null || sender == e.Source) {
                e.Effects = DragDropEffects.None;
            } else {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void NodeGraph_MouseDown(object sender, MouseButtonEventArgs e) {
            if (Pipe.EditingPipe != null && (Mouse.DirectlyOver == this || Mouse.DirectlyOver == canvas)) {
                Pipe.EditingPipe.Dispose();
                Pipe.EditingPipe = null;
            }
        }

        public T addNode<T>(T node) where T : Node {
            canvas.Children.Add(node);
            node.setConnections();
            return node;
        }

        public bool tryAddNode(Node node) {
            if (!canvas.Children.Contains(node)) {
                canvas.Children.Add(node);
                node.setConnections();
                return true;
            }
            return false;
        }

        public void removeNode(Node node) {
            canvas.Children.Remove(node);
            node.Dispose();
        }

        public bool tryRemoveNode(Node node) {
            if (canvas.Children.Contains(node)) {
                canvas.Children.Remove(node);
                node.Dispose();
                return true;
            }
            return false;
        }

        public void autoArrange() {

            Node[] nodes = getNodes();
            Dictionary<int, int> columns = new Dictionary<int, int>();
            int maxMaxDepth = -1;

            for (int i = 0; i < nodes.Length; i++) {
                int maxDepth = nodes[i].getMaximumDepth();
                if (maxDepth > maxMaxDepth)
                    maxMaxDepth = maxDepth;
            }

            for (int i = 0; i < nodes.Length; i++) {
                int maxDepth = nodes[i].getMaximumDepth();

                if (columns.ContainsKey(maxDepth)) {
                    columns[maxDepth]++;
                }
                else {
                    columns.Add(maxDepth, 1);
                }

                switch (context.orientation) {
                    case NodeGraphOrientation.LeftToRight:
                        nodes[i].position = new Point(maxDepth * 350, columns[maxDepth] * 100);
                        break;
                    case NodeGraphOrientation.RightToLeft:
                        nodes[i].position = new Point((maxMaxDepth - maxDepth) * 350, columns[maxDepth] * 100);
                        break;
                    case NodeGraphOrientation.UpToBottom:
                        nodes[i].position = new Point(columns[maxDepth] * 270, maxDepth * 150);
                        break;
                    case NodeGraphOrientation.BottomToUp:
                        nodes[i].position = new Point(columns[maxDepth] * 270, (maxMaxDepth - maxDepth) * 150);
                        break;
                }
            }
        }

        public Node[] getNodes() {
            return canvas.Children.OfType<Node>().ToArray();
        }

        public void process() {
            var nodes = getNodes();
            // Clears all the previous run results
            foreach (Node node in nodes) {
                foreach (OutputDock dock in node.getOutputs()) {
                    foreach (Pipe pipe in dock.pipes) {
                        pipe.result = null;
                    }
                }
            }
            // Runs !
            foreach (Node node in nodes.Where(x => x.getInputs().Length == 0)) {
                Thread thread = new Thread(new ThreadStart(() => {
                    node.queryProcess();
                }));
                thread.Start();
            }
        }
    }
}
