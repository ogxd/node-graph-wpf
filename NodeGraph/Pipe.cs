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

        public NodeGraph graph => (dockA != null) ? dockA.node.graph : dockB.node.graph;

        public object result = null;

        private Dock _dockA;
        public Dock dockA {
            get { return _dockA; }
            set {
                if (_dockA == value || _dockB == value)
                    return;

                EditingPipe = null;
                _dockA = value;
                _dockA.node.moved += NodeA_moved;
                _dockA.pipe = this;
                setAnchorPointA(dockA.getPositionInCanvas());
            }
        }

        private Dock _dockB;
        public Dock dockB {
            get { return _dockB; }
            set {
                if (_dockA == value || _dockB == value)
                    return;

                EditingPipe = null;
                _dockB = value;
                _dockB.node.moved += NodeB_moved;
                _dockB.pipe = this;
                setAnchorPointB(dockB.getPositionInCanvas());
            }
        }

        public Pipe(Dock dockA, Dock dockB) {

            bezier = new BezierSegment() {
                IsStroked = true
            };

            // Set up the Path to insert the segments
            PathGeometry pathGeometry = new PathGeometry();

            pathFigure = new PathFigure();
            pathFigure.IsClosed = false;
            pathGeometry.Figures.Add(pathFigure);

            this.dockA = dockA;
            this.dockB = dockB;
            if (dockA == null || dockB == null)
                EditingPipe = this;

            pathFigure.Segments.Add(bezier);
            path = new Path();
            path.IsHitTestVisible = false;
            path.Stroke = graph.getPipeColor((dockA != null)? dockA.type : dockB.type);
            path.StrokeThickness = 2;
            path.Data = pathGeometry;

            graph.canvas.Children.Add(path);
            ((UIElement)graph.canvas.Parent).MouseMove += Canvas_MouseMove;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e) {
            if (EditingPipe == null)
                ((UIElement)graph.canvas.Parent).MouseMove -= Canvas_MouseMove;

            if (dockA == null) {
                setAnchorPointA(Mouse.GetPosition(graph.canvas));
            }

            if (dockB == null) {
                setAnchorPointB(Mouse.GetPosition(graph.canvas));
            }
        }

        private void setAnchorPointB(Point point) {
            Point D = point;
            Point C = new Point(D.X - graph.pipeStiffness, D.Y);
            bezier.Point2 = C;
            bezier.Point3 = D;
        }

        private void setAnchorPointA(Point point) {
            Point A = point;
            Point B = new Point(A.X + graph.pipeStiffness, A.Y);
            pathFigure.StartPoint = A;
            bezier.Point1 = B;
        }

        private void NodeA_moved(Node node) {
            setAnchorPointA(dockA.getPositionInCanvas());
        }

        private void NodeB_moved(Node node) {
            setAnchorPointB(dockB.getPositionInCanvas());
        }

        public void Dispose() {
            graph.canvas.Children.Remove(path);
            ((UIElement)graph.canvas.Parent).MouseMove -= Canvas_MouseMove;
        }
    }
}
