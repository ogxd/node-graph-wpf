using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ogxd.NodeGraph {

    public partial class FloatingChest : Border {

        private Nullable<Point> dragStart = null;
        private Point startPoint;
        private readonly NodeChest chest;
        private readonly NodeGraph graph;

        public FloatingChest(NodeChest chest, NodeGraph graph) {
            InitializeComponent();

            this.chest = chest;
            this.graph = graph;

            Child = chest;

            PreviewMouseLeftButtonDown += Node_PreviewMouseLeftButtonDown;
            MouseDown += mouseDown;
            MouseMove += mouseMove;
            MouseUp += mouseUp;
        }

        private void Node_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            startPoint = e.GetPosition(null);
        }

        private void mouseMove(object sender, MouseEventArgs args) {

            if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                UIElement element = (UIElement)sender;
                Point p2 = args.GetPosition(graph.canvas);
                position = new Point(p2.X - dragStart.Value.X, p2.Y - dragStart.Value.Y);
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
            set {
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
            }
        }
    }
}
