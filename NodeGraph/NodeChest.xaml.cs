using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Ogxd.NodeGraph {

    public partial class NodeChest : Border {

        public readonly NodeGraphContext context;

        public NodeChest(NodeGraphContext context) {
            InitializeComponent();
            context.propertyChanged += Context_propertyChanged;
            this.context = context;
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

        public Node[] getNodes() {
            return wrapPanel.Children.OfType<Node>().ToArray();
        }

        public void addNode(Node node) {
            node.Margin = new Thickness(15);
            wrapPanel.Children.Add(node);
            node.setConnections();
        }

        public void removeNode(Node node) {
            if (wrapPanel.Children.Contains(node)) {
                wrapPanel.Children.Remove(node);
            }
        }
    }
}
