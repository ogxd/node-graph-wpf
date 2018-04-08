using System;
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

        public MainWindow() {
            InitializeComponent();

            NodeGraphContext context = new NodeGraphContext();

            NodeChest chest = new NodeChest(context);
            container.Children.Add(chest);
            Grid.SetRow(chest, 2);
            chest.addNode(new AdditionNode());
            chest.addNode(new IntNode());

            nodeGraph = new NodeGraph(context);
            container.Children.Add(nodeGraph);

            IntNode intNode1 = nodeGraph.addNode(new IntNode() { position = new Point(10, 10) });
            IntNode intNode2 = nodeGraph.addNode(new IntNode() { position = new Point(10, 230) });
            AdditionNode additionNode1 = nodeGraph.addNode(new AdditionNode() { position = new Point(350, 120) });
            AdditionNode additionNode2 = nodeGraph.addNode(new AdditionNode() { position = new Point(350, 220) });
            AdditionNode additionNode3 = nodeGraph.addNode(new AdditionNode() { position = new Point(500, 160) });
            IntToHexNode intToHexNode = nodeGraph.addNode(intToHexNode = new IntToHexNode() { position = new Point(700, 120) });
            ConsoleOutputNode consoleOutputNode = nodeGraph.addNode(consoleOutputNode = new ConsoleOutputNode() { position = new Point(1050, 120) });

            var a = new IntToHexNode() { position = new Point(700, 120) };
            Task.Run(() => {
                Thread.Sleep(10);
                Dispatcher.Invoke(() => {
                    nodeGraph.addNode(a);
                });
            });
            
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
    }
}
