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

    /// <summary>
    /// Interaction logic for Dock.xaml
    /// </summary>
    public abstract class Dock : Border {

        public readonly Node node;
        public readonly int type;

        public Dock(Node node, int type) {
            Width = 16;
            Height = 16;
            Background = Brushes.White;
            BorderBrush = Brushes.Black;
            BorderThickness = new Thickness(2);

            this.type = type;
            this.node = node;

            MouseLeftButtonDown += onDockClick;
            MouseEnter += onDockMouseEnter;
            MouseLeave += onDockMouseLeave;
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent) {
            Background = node.context.getPipeColor(type);
        }

        public Point getPositionInCanvas() {
            return this.TransformToVisual(node).Transform(new Point(Canvas.GetLeft(node) + 8, Canvas.GetTop(node) + 8));
        }

        internal abstract void onDockClick(object sender, RoutedEventArgs e);

        internal abstract void onDockMouseEnter(object sender, MouseEventArgs e);

        internal abstract void onDockMouseLeave(object sender, MouseEventArgs e);

        /// <summary>
        /// Will return false if :
        /// - Same node
        /// - Different type
        /// - Not editing
        /// </summary>
        /// <returns></returns>
        public bool isCompatibleWithEditingPipe() {
            if (Pipe.EditingPipe == null)
                return false;
            // If input is set
            if (Pipe.EditingPipe.inputDock != null) {
                if (this is InputDock || Pipe.EditingPipe.inputDock.node == node || Pipe.EditingPipe.inputDock.type != type)
                    return false;
            }
            // Otherwise, if output is set
            else {
                if (this is OutputDock || Pipe.EditingPipe.outputDock.node == node || Pipe.EditingPipe.outputDock.type != type)
                    return false;
            }
            return true;
        }
    }
}
