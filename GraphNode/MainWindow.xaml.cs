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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            NodeGraph nodeGraph = new NodeGraph();
            Content = nodeGraph;

            nodeGraph.addNode(new ReaderNode() { position = new Point(10, 10) });
            nodeGraph.addNode(new NormalsNode() { position = new Point(210, 110) });
            nodeGraph.addNode(new WriterNode() { position = new Point(410, 210) });
        }
    }
}
