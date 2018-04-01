using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ogxd.NodeGraph {

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
            //InitializeComponent();
            this.LoadViewFromUri("/Ogxd.NodeGraph;component/node.xaml");

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

        public Dock addInput(int type) {
            Dock dock = new Dock(this, type, DockSide.IN);
            stackInputs.Children.Add(dock);
            //RenderSize = MeasureOverride(RenderSize);
            //RenderSize = ArrangeOverride(new Size(double.MaxValue, double.MaxValue));
            //Measure(new Size(double.MaxValue, double.MaxValue));
            return dock;
        }

        public void removeInput(Dock dock) {
            stackInputs.Children.Remove(dock);
        }

        public Dock addOutput(int type) {
            Dock dock = new Dock(this, type, DockSide.OUT);
            stackOutputs.Children.Add(dock);
            Console.WriteLine("Added to stack");
            return dock;
        }

        public void removeOutput(Dock dock) {
            stackOutputs.Children.Remove(dock);
        }

        public abstract void process();
    }
}
