using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Ogxd.NodeGraph {

    public class InputDock : Dock {

        public Pipe pipe;

        public InputDock(Node node, int type) : base(node, type) {

        }

        internal override void onDockClick(object sender, RoutedEventArgs e) {
            if (node.isTemplate)
                return;

            if (pipe != null) {
                pipe.Dispose();
            }

            if (Pipe.EditingPipe == null) {
                new Pipe(null, this);
                return;
            }

            if (isCompatibleWithEditingPipe()) {
                // Prevent from having duplicate pipes
                if (Pipe.EditingPipe.outputDock.pipes.Any(x => x.inputDock == this))
                    return;
                Pipe.EditingPipe.inputDock = this;
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
