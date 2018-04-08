using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Ogxd.NodeGraph {

    public abstract class Node : Border, IDisposable {

        public delegate void NodeEventHandler(Node node);
        public event NodeEventHandler moved;

        public NodeGraph graph { get; private set; }
        public NodeChest chest { get; private set; }
        private Nullable<Point> dragStart = null;

        public NodeGraphContext context => (graph != null) ? graph.context : chest.context;

        public InputDock[] getInputs() => Dispatcher.Invoke(() => stackInputs.Children.OfType<InputDock>().ToArray());
        public OutputDock[] getOutputs() => Dispatcher.Invoke(() => stackOutputs.Children.OfType<OutputDock>().ToArray());
        public Dictionary<string, object> parameters => Dispatcher.Invoke(() => stackParameters.Children.OfType<IProperty>().ToDictionary(x => x.label, y => y.value));

        public bool isTemplate => chest != null;

        public string title {
            get {
                return titleUI.Text;
            }
            set {
                titleUI.Text = value;
            }
        }

        public Node() {
            initialize();
        }

        private TextBlock titleUI;
        private StackPanel stackInputs;
        private StackPanel stackParameters;
        private StackPanel stackOutputs;

        private void initialize() {
            Width = 250;
            MinHeight = 50;
            CornerRadius = new CornerRadius(5);
            var converter = new System.Windows.Media.BrushConverter();
            Background = (Brush)converter.ConvertFromString("#262626");
            BorderBrush = Brushes.White;
            BorderThickness = new Thickness(0);

            Grid grid = new Grid {};
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(20) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            titleUI = new TextBlock {
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            stackInputs = new StackPanel { Margin = new Thickness(-8, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };
            stackParameters = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            stackOutputs = new StackPanel { Margin = new Thickness(0, 0, -8, 0), VerticalAlignment = VerticalAlignment.Center };

            Child = grid;

            grid.Children.Add(titleUI);
            grid.Children.Add(stackInputs);
            grid.Children.Add(stackParameters);
            grid.Children.Add(stackOutputs);

            Grid.SetColumn(titleUI, 1);
            Grid.SetColumn(stackInputs, 0);
            Grid.SetColumn(stackParameters, 1);
            Grid.SetColumn(stackOutputs, 2);

            Grid.SetRow(stackInputs, 1);
            Grid.SetRow(stackParameters, 1);
            Grid.SetRow(stackOutputs, 1);

            PreviewMouseLeftButtonDown += Node_PreviewMouseLeftButtonDown;
            MouseDown += mouseDown;
            MouseMove += mouseMove;
            MouseUp += mouseUp;
        }

        Point startPoint;

        private void Node_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            startPoint = e.GetPosition(null);
        }

        public abstract void setConnections();

        protected override void OnVisualParentChanged(DependencyObject oldParent) {
            base.OnVisualParentChanged(oldParent);
            this.graph = (Parent as Canvas)?.Parent as NodeGraph;
            this.chest = (Parent as WrapPanel)?.Parent as NodeChest;
        }

        private void mouseMove(object sender, MouseEventArgs args) {

            if (isTemplate) {
                // Get the current mouse position
                Point mousePos = args.GetPosition(null);
                Vector diff = startPoint - mousePos;

                if (args.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)) {

                    // Initialize the drag & drop operation
                    DataObject dragData = new DataObject("node", this);
                    DragDrop.DoDragDrop(this, dragData, DragDropEffects.Move);
                }
            }
            else {
                if (dragStart != null && args.LeftButton == MouseButtonState.Pressed) {
                    UIElement element = (UIElement)sender;
                    Point p2 = args.GetPosition(graph.canvas);
                    position = new Point(p2.X - dragStart.Value.X, p2.Y - dragStart.Value.Y);
                    isAboutToBeRemoved = !isPointWithin(p2);
                }
            }
        }

        private bool isPointWithin(Point point) {
            if (point.X < 0 || point.Y < 0 || point.X > graph.ActualWidth || point.Y > graph.ActualHeight)
                return false;
            return true;
        }

        private void mouseUp(object sender, MouseEventArgs args) {
            var element = (UIElement)sender;
            dragStart = null;
            element.ReleaseMouseCapture();
            if (isAboutToBeRemoved)
                graph.removeNode(this);
        }

        private void mouseDown(object sender, MouseEventArgs args) {
            var element = (UIElement)sender;
            dragStart = args.GetPosition(element);
            element.CaptureMouse();
        }

        private bool _isAboutToBeRemoved = false;
        public bool isAboutToBeRemoved {
            get { return _isAboutToBeRemoved; }
            private set {
                if (_isAboutToBeRemoved == value)
                    return;

                _isAboutToBeRemoved = value;
                if (_isAboutToBeRemoved) {
                    BorderBrush = Brushes.Red;
                    BorderThickness = new Thickness(2);
                } else {
                    BorderBrush = Brushes.White;
                    BorderThickness = new Thickness(0);
                }
            }
        }

        public Point position {
            get { return new Point(Canvas.GetLeft(this), Canvas.GetTop(this)); }
            set {
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
                moved?.Invoke(this);
            }
        }

        public Dock addInput(int type) {
            InputDock dock = new InputDock(this, type);
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

        public OutputDock addOutput(int type) {
            OutputDock dock = new OutputDock(this, type);
            stackOutputs.Children.Add(dock);
            return dock;
        }

        public void removeOutput(Dock dock) {
            stackOutputs.Children.Remove(dock);
        }

        public void clearOutputs() {
            foreach (OutputDock output in getOutputs()) {
                foreach (Pipe pipe in output.pipes) {
                    pipe.Dispose();
                }
            }
            stackOutputs.Children.Clear();
        }

        public IProperty addProperty<IProperty>(IProperty property) {
            stackParameters.Children.Add(property as UIElement);
            return property;
        }

        public int getMaximumDepth() {
            InputDock[] inputs = getInputs();
            int maxDepth = 0;
            foreach (InputDock input in inputs) {
                Node previousNode = input.pipe?.outputDock?.node;
                if (previousNode == null)
                    continue;
                int previousMaxDepth = previousNode.getMaximumDepth() + 1;
                if (previousMaxDepth > maxDepth)
                    maxDepth = previousMaxDepth;
            }
            return maxDepth;
        }

        public void queryProcess() {

            // Makes sure all inputs are ready for processing. Otherwise, cancel.
            InputDock[] ins = getInputs();
            foreach (InputDock input in ins)
                if (input.pipe.result == null)
                    return;

            // Process the data (Overriding class implementation)
            Dispatcher.Invoke(() => { BorderThickness = new Thickness(2); });
            Thread.Sleep(100);
            object[] results = process(ins.Select(x => x.pipe.result).ToArray(), parameters);
            Dispatcher.Invoke(() => { BorderThickness = new Thickness(0); });

            // Tranfers results to next nodes
            OutputDock[] outs = getOutputs();
            for (int i = 0; i < outs.Length; i++) {
                foreach (Pipe pipe in outs[i].pipes) {
                    pipe.result = results[i];
                    pipe.inputDock.node.queryProcess();
                }
            }
        }

        public abstract object[] process(object[] ins, Dictionary<string, object> parameters);

        public void Dispose() {
            Visibility = Visibility.Collapsed;
            foreach (InputDock input in getInputs()) {
                if (input.pipe != null)
                    input.pipe.Dispose();
            }
            foreach (OutputDock output in getOutputs()) {
                foreach (Pipe pipe in output.pipes.ToArray()) {
                    pipe.Dispose();
                }
            }
        }
    }
}
