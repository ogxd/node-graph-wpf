using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Ogxd.NodeGraph {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        NodeGraph nodeGraph;
        NodeChest nodeChest;
        NodeGraphContext context;

        public MainWindow() {
            InitializeComponent();

            context = new NodeGraphContext();

            nodeChest = new NodeChest(context);
            //container.Children.Add(nodeChest);
            //Grid.SetRow(nodeChest, 2);

            nodeChest.addNode(new AdditionNode());
            nodeChest.addNode(new IntNode());
            nodeChest.addNode(new IntToHexNode());
            nodeChest.addNode(new ConsoleOutputNode());
            nodeChest.addNode(new DuplicationNode());

            nodeGraph = new NodeGraph(context);
            container.Children.Add(nodeGraph);
            FloatingChest fchest = new FloatingChest(nodeChest, nodeGraph);
            nodeGraph.canvas.Children.Add(fchest);

            IntNode intNode1 = nodeGraph.addNode(new IntNode() { position = new Point(10, 10) });
            IntNode intNode2 = nodeGraph.addNode(new IntNode() { position = new Point(10, 230) });
            AdditionNode additionNode1 = nodeGraph.addNode(new AdditionNode() { position = new Point(350, 120) });
            AdditionNode additionNode2 = nodeGraph.addNode(new AdditionNode() { position = new Point(350, 220) });
            AdditionNode additionNode3 = nodeGraph.addNode(new AdditionNode() { position = new Point(500, 160) });
            IntToHexNode intToHexNode = nodeGraph.addNode(intToHexNode = new IntToHexNode() { position = new Point(700, 120) });
            ConsoleOutputNode consoleOutputNode = nodeGraph.addNode(consoleOutputNode = new ConsoleOutputNode() { position = new Point(1050, 120) });
            
            new Pipe(intNode1.getOutputs()[0], additionNode1.getInputs()[0]);
            new Pipe(intNode1.getOutputs()[0], additionNode2.getInputs()[0]);
            new Pipe(intNode2.getOutputs()[0], additionNode1.getInputs()[1]);

            new Pipe(intNode2.getOutputs()[0], additionNode2.getInputs()[1]);

            new Pipe(additionNode1.getOutputs()[0], additionNode3.getInputs()[0]);
            new Pipe(additionNode2.getOutputs()[0], additionNode3.getInputs()[1]);

            new Pipe(additionNode3.getOutputs()[0], intToHexNode.getInputs()[0]);

            new Pipe(intToHexNode.getOutputs()[0], consoleOutputNode.getInputs()[0]);
        }

        private void runClick(object sender, RoutedEventArgs e) {
            nodeGraph.process();
        }

        private void addClick(object sender, RoutedEventArgs e) {
            nodeGraph.addNode(new IntNode() { position = new Point(10, 10) });
        }

        private void rearrange(object sender, RoutedEventArgs e) {
            nodeGraph.autoArrange();
        }

        private void rotate(object sender, RoutedEventArgs e) {
            context.orientation = GetNextEnumValueOf(context.orientation);
        }

        public static NodeGraphOrientation GetNextEnumValueOf(NodeGraphOrientation value) {
            var values = (NodeGraphOrientation[])Enum.GetValues(typeof(NodeGraphOrientation));
            var nextValues = values.Where(x => (int)x > (int)value);
            if (nextValues.Count() == 0) {
                return values.First();
            } else {
                return nextValues.First();
            }
        }
    }
}
