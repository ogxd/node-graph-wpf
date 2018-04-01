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
using System.Linq;

namespace WpfApp2 {

    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public abstract partial class Node : Border {

        public delegate void NodeEventHandler(Node node);
        public event NodeEventHandler moved;

        public Canvas canvas { get; private set; }
        private Nullable<Point> dragStart = null;

        public Dock[] inputs => stackInputs.Children.OfType<Dock>().ToArray();
        public Dock[] outputs => stackOutputs.Children.OfType<Dock>().ToArray();

        public Node() {
            InitializeComponent();

            MouseDown += mouseDown;
            MouseMove += mouseMove;
            MouseUp += mouseUp;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent) {
            base.OnVisualParentChanged(oldParent);
            this.canvas = Parent as Canvas;
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

        public void addInput(Dock dock) {
            stackInputs.Children.Add(dock);
        }

        public void removeInput(Dock dock) {
            stackInputs.Children.Remove(dock);
        }

        public void addOutput(Dock dock) {
            stackOutputs.Children.Add(dock);
        }

        public void removeOutput(Dock dock) {
            stackOutputs.Children.Remove(dock);
        }

        public abstract void process();
    }

    public class ReaderNode : Node {

        public ReaderNode() : base () {
            addInput(new Dock(this, 0, DockSide.OUT));
        }

        public override void process() {
        }
    }

    public class NormalsNode : Node {

        public NormalsNode() : base() {
            addInput(new Dock(this, 0, DockSide.IN));
            addOutput(new Dock(this, 1, DockSide.OUT));
        }

        public override void process() {
        }
    }

    public class WriterNode : Node {

        public WriterNode() : base() {
            addInput(new Dock(this, 1, DockSide.IN));
        }

        public override void process() {
        }
    }
}
