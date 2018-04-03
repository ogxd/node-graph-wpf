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
using System.Windows.Threading;

namespace Ogxd.NodeGraph {

    public enum DockSide { IN, OUT }

    /// <summary>
    /// Interaction logic for Dock.xaml
    /// </summary>
    public partial class Dock : Button {

        public Pipe pipe;

        public readonly Node node;
        public readonly int type;
        public readonly DockSide side;

        public Dock(Node node, int type, DockSide side) {
            InitializeComponent();

            this.type = type;
            this.side = side;
            this.node = node;

            Click += Dock_Click;
            MouseEnter += Border_MouseEnter;
            MouseLeave += Border_MouseLeave;

            IsHitTestVisible = true;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent) {
            Background = node.graph.getPipeColor(type);
        }

        public Point getPositionInCanvas() {
            return this.TransformToVisual(node).Transform(new Point(Canvas.GetLeft(node) + 8, Canvas.GetTop(node) + 8));
        }

        private void Dock_Click(object sender, RoutedEventArgs e) {
            if (pipe != null) {
                pipe.Dispose();
                //Pipe.EditingPipe = null;
            }
            if (Pipe.EditingPipe == null) {
                if (side == DockSide.IN)
                    new Pipe(null, this);
                else
                    new Pipe(this, null);
            } else {
                if (Pipe.EditingPipe.dockA != null) {
                    if (Pipe.EditingPipe.dockA.type != type || Pipe.EditingPipe.dockA.side == side)
                        return;
                    Pipe.EditingPipe.dockB = this;
                } else {
                    if (Pipe.EditingPipe.dockB.type != type || Pipe.EditingPipe.dockB.side == side)
                        return;
                    Pipe.EditingPipe.dockA = this;
                }
            }
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e) {
            if (Pipe.EditingPipe == null)
                return;

            if (Pipe.EditingPipe.dockA != null) {
                if (Pipe.EditingPipe.dockA == this || Pipe.EditingPipe.dockA.type != type || Pipe.EditingPipe.dockA.side == side)
                    return;
                BorderBrush = Brushes.White;
            } else {
                if (Pipe.EditingPipe.dockB == this || Pipe.EditingPipe.dockB.type != type || Pipe.EditingPipe.dockB.side == side)
                    return;
                BorderBrush = Brushes.White;
            }
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e) {
            BorderBrush = Brushes.Black;
        }
    }
}
