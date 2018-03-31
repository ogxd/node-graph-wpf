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
    /// Interaction logic for Node.xaml
    /// </summary>
    public partial class Node : Border {

        public delegate void NodeEventHandler(Node node);
        public event NodeEventHandler moved;

        public readonly Canvas canvas;
        private Nullable<Point> dragStart = null;

        public Node(Canvas canvas) {
            InitializeComponent();

            this.canvas = canvas;

            MouseDown += mouseDown;
            MouseMove += mouseMove;
            MouseUp += mouseUp;
        }

        private void mouseMove(object sender, MouseEventArgs args) {
            if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                var element = (UIElement)sender;
                var p2 = args.GetPosition(canvas);
                Canvas.SetLeft(element, p2.X - dragStart.Value.X);
                Canvas.SetTop(element, p2.Y - dragStart.Value.Y);
                moved?.Invoke(this);
            }
        }

        private void mouseUp(object sender, MouseEventArgs args) {
            var element = (UIElement)sender;
            dragStart = null;
            element.ReleaseMouseCapture();
        }

        private void mouseDown(object sender, MouseEventArgs args) {
            var element = (UIElement)sender;
            dragStart = args.GetPosition(element);
            element.CaptureMouse();
        }

        public Point position {
            get { return new Point(Canvas.GetLeft(this), Canvas.GetTop(this)); }
            set { Canvas.SetLeft(this, value.X); Canvas.SetTop(this, value.Y); }
        }
    }

    public class ReaderNode : Node {

        public ReaderNode(Canvas canvas) : base (canvas) {
            stackOutputs.Children.Add(new Dock(this, 0, DockSide.OUT));
        }
    }

    public class NormalsNode : Node {

        public NormalsNode(Canvas canvas) : base(canvas) {
            stackInputs.Children.Add(new Dock(this, 0, DockSide.IN));
            stackOutputs.Children.Add(new Dock(this, 1, DockSide.OUT));
        }
    }

    public class WriterNode : Node {

        public WriterNode(Canvas canvas) : base(canvas) {
            stackInputs.Children.Add(new Dock(this, 1, DockSide.IN));
        }
    }
}
