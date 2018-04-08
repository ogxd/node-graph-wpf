using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Ogxd.NodeGraph {

    public class Pipe : IDisposable {

        public static Pipe EditingPipe;

        public readonly PathFigure pathFigure;
        public readonly BezierSegment bezier;
        public readonly Path path;

        public NodeGraph graph => (inputDock != null) ? inputDock.node.graph : outputDock.node.graph;

        public object result = null;

        private InputDock _inputDock;
        public InputDock inputDock {
            get { return _inputDock; }
            set {
                if (_inputDock == value)
                    return;

                EditingPipe = null;
                _inputDock = value;
                _inputDock.node.moved += inputNodeMoved;
                _inputDock.pipe = this;

                if (_inputDock != null) {
                    if (_inputDock.IsLoaded)
                        setAnchorPointB(inputDock.getPositionInCanvas());
                    else
                        _inputDock.Loaded += _inputDock_Loaded;
                }
            }
        }

        private OutputDock _outputDock;
        public OutputDock outputDock {
            get { return _outputDock; }
            set {
                if (_outputDock == value)
                    return;

                EditingPipe = null;
                _outputDock = value;
                _outputDock.node.moved += outputNodeMoved;
                _outputDock.pipes.Add(this);
                
                if (_outputDock != null) {
                    if (_outputDock.IsLoaded)
                        setAnchorPointA(outputDock.getPositionInCanvas());
                    else
                        _outputDock.Loaded += _outputDock_Loaded;
                }
            }
        }

        private void _outputDock_Loaded(object sender, RoutedEventArgs e) {
            _outputDock.Loaded -= _outputDock_Loaded;
            setAnchorPointA(outputDock.getPositionInCanvas());
        }

        private void _inputDock_Loaded(object sender, RoutedEventArgs e) {
            _inputDock.Loaded -= _inputDock_Loaded;
            setAnchorPointB(inputDock.getPositionInCanvas());
        }

        public Pipe(OutputDock output, InputDock input) {

            bezier = new BezierSegment() {
                IsStroked = true
            };

            // Set up the Path to insert the segments
            PathGeometry pathGeometry = new PathGeometry();

            pathFigure = new PathFigure();
            pathFigure.IsClosed = false;
            pathGeometry.Figures.Add(pathFigure);

            this.inputDock = input;
            this.outputDock = output;
            if (input == null || output == null)
                EditingPipe = this;

            pathFigure.Segments.Add(bezier);
            path = new Path();
            path.IsHitTestVisible = false;
            path.Stroke = graph.context.getPipeColor((input != null)? input.type : output.type);
            path.StrokeThickness = 2;
            path.Data = pathGeometry;

            graph.canvas.Children.Add(path);
            ((UIElement)graph.canvas.Parent).MouseMove += Canvas_MouseMove;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            if (EditingPipe == null)
                ((UIElement)graph.canvas.Parent).MouseMove -= Canvas_MouseMove;

            if (outputDock == null) {
                setAnchorPointA(Mouse.GetPosition(graph.canvas));
            } else if (inputDock == null) {
                setAnchorPointB(Mouse.GetPosition(graph.canvas));
            }
        }

        private void setAnchorPointB(Point point) {
            bezier.Point3 = point;
            switch (graph.context.orientation) {
                case NodeGraphOrientation.LeftToRight:
                    bezier.Point2 = new Point(bezier.Point3.X - graph.pipeStiffness, bezier.Point3.Y);
                    break;
                case NodeGraphOrientation.RightToLeft:
                    bezier.Point2 = new Point(bezier.Point3.X + graph.pipeStiffness, bezier.Point3.Y);
                    break;
                case NodeGraphOrientation.UpToBottom:
                    bezier.Point2 = new Point(bezier.Point3.X, bezier.Point3.Y - graph.pipeStiffness);
                    break;
                case NodeGraphOrientation.BottomToUp:
                    bezier.Point2 = new Point(bezier.Point3.X, bezier.Point3.Y + graph.pipeStiffness);
                    break;
            }
        }

        private void setAnchorPointA(Point point) {
            pathFigure.StartPoint = point;
            switch (graph.context.orientation) {
                case NodeGraphOrientation.LeftToRight:
                    bezier.Point1 = new Point(pathFigure.StartPoint.X + graph.pipeStiffness, pathFigure.StartPoint.Y);
                    break;
                case NodeGraphOrientation.RightToLeft:
                    bezier.Point1 = new Point(pathFigure.StartPoint.X - graph.pipeStiffness, pathFigure.StartPoint.Y);
                    break;
                case NodeGraphOrientation.UpToBottom:
                    bezier.Point1 = new Point(pathFigure.StartPoint.X, pathFigure.StartPoint.Y + graph.pipeStiffness);
                    break;
                case NodeGraphOrientation.BottomToUp:
                    bezier.Point1 = new Point(pathFigure.StartPoint.X, pathFigure.StartPoint.Y - graph.pipeStiffness);
                    break;
            }
        }

        private void outputNodeMoved(Node node) {
            setAnchorPointA(outputDock.getPositionInCanvas());
        }

        private void inputNodeMoved(Node node) {
            setAnchorPointB(inputDock.getPositionInCanvas());
        }

        public void Dispose() {
            if (inputDock != null)
                inputDock.pipe = null;
            if (outputDock != null)
                outputDock.pipes.Remove(this);
            graph.canvas.Children.Remove(path);
            ((UIElement)graph.canvas.Parent).MouseMove -= Canvas_MouseMove;
        }
    }
}
