using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Ogxd.NodeGraph
{

    /// <summary>
    /// Interaction logic for Node.xaml
    /// </summary>
    public abstract partial class Node : Border {

        public delegate void NodeEventHandler(Node node);
        public event NodeEventHandler moved;

        public NodeGraph graph { get; private set; }
        private Nullable<Point> dragStart = null;

        public Dock[] inputs => Dispatcher.Invoke(() => stackInputs.Children.OfType<Dock>().ToArray());
        public Dock[] outputs => Dispatcher.Invoke(() => stackOutputs.Children.OfType<Dock>().ToArray());
        public Dictionary<string, object> parameters => Dispatcher.Invoke(() => stackParameters.Children.OfType<IProperty>().ToDictionary(x => x.label, y => y.value));

        public Node() {
            this.LoadViewFromUri("/Ogxd.NodeGraph;component/node.xaml");

            MouseDown += mouseDown;
            MouseMove += mouseMove;
            MouseUp += mouseUp;
        }

        public abstract void setConnections();

        protected override void OnVisualParentChanged(DependencyObject oldParent) {
            base.OnVisualParentChanged(oldParent);
            this.graph = (Parent as Canvas)?.Parent as NodeGraph;
        }

        private void mouseMove(object sender, MouseEventArgs args) {
            if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                var element = (UIElement)sender;
                var p2 = args.GetPosition(graph.canvas);
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
            //stackInputs.UpdateLayout();
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

        public void clearOutputs() {
            foreach (Dock output in outputs) {
                if (output.pipe != null) {
                    output.pipe.Dispose();
                }
            }
            stackOutputs.Children.Clear();
        }

        public IProperty addProperty<IProperty>(IProperty property) {
            stackParameters.Children.Add(property as UIElement);
            return property;
        }

        public void queryProcess() {

            // Makes sure all inputs are ready for processing. Otherwise, cancel.
            Dock[] ins = inputs;
            foreach (Dock input in ins)
                if (input == null)
                    return;

            // Process the data (Overriding class implementation)
            Dispatcher.Invoke(() => { BorderThickness = new Thickness(2); });
            Thread.Sleep(100);
            object[] results = process(ins.Select(x => x.pipe.result).ToArray(), parameters);
            Dispatcher.Invoke(() => { BorderThickness = new Thickness(0); });

            // Tranfers results to next nodes
            Dock[] outs = outputs;
            for (int i = 0; i < outs.Length; i++) {
                if (outs[i].pipe != null) {
                    outs[i].pipe.result = results[i];
                    outs[i].pipe.dockB.node.queryProcess();
                }
            }
        }

        public abstract object[] process(object[] ins, Dictionary<string, object> parameters);
    }
}
