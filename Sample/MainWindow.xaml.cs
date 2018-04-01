using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Ogxd.NodeGraph {

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        NodeGraph nodeGraph;

        public MainWindow() {
            InitializeComponent();

            nodeGraph = new NodeGraph();
            container.Children.Add(nodeGraph);

            IntNode intNode1;
            IntNode intNode2;
            AdditionNode additionNode;
            IntToHexNode intToHexNode;
            ConsoleOutputNode consoleOutputNode;

            nodeGraph.addNode(intNode1 = new IntNode() { position = new Point(10, 10) });
            nodeGraph.addNode(intNode2 = new IntNode() { position = new Point(10, 230) });
            nodeGraph.addNode(additionNode = new AdditionNode() { position = new Point(350, 120) });
            nodeGraph.addNode(intToHexNode = new IntToHexNode() { position = new Point(700, 120) });
            nodeGraph.addNode(consoleOutputNode = new ConsoleOutputNode() { position = new Point(1050, 120) });



            Task.Run(() => {
                Thread.Sleep(1);
                Dispatcher.Invoke(() => {
                    new Pipe(intNode1.outputs[0], additionNode.inputs[0]);
                    new Pipe(intNode2.outputs[0], additionNode.inputs[1]);
                    new Pipe(intToHexNode.outputs[0], consoleOutputNode.inputs[0]);
                });
            });
        }

        private void runClick(object sender, RoutedEventArgs e) {
            nodeGraph.process();
        }
    }
}
