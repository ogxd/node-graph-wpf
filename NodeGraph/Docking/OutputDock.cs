using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Ogxd.NodeGraph {

    /// <summary>
    /// OutputDock for output data (processed by the Node)
    /// </summary>
    public sealed class OutputDock : Dock {

        /// <summary>
        /// Connected Pipes. Can be any number. The data gets distributed to all connected InputDocks
        /// </summary>
        public HashSet<Pipe> pipes = new HashSet<Pipe>();

        public OutputDock(Node node, int type) : base(node, type) {

        }

        internal override void onDockClick(object sender, RoutedEventArgs e) {
            if (node.isTemplate)
                return;

            if (Pipe.EditingPipe == null) {
                new Pipe(this, null);
                return;
            }

            if (isCompatibleWithEditingPipe()) {
                Pipe.EditingPipe.outputDock = this;
            }
        }

        internal override void onDockMouseEnter(object sender, MouseEventArgs e) {
            if (isCompatibleWithEditingPipe()) {
                BorderBrush = Brushes.White;
            }
        }

        internal override void onDockMouseLeave(object sender, MouseEventArgs e) {
            BorderBrush = Brushes.Black;
        }
    }
}
