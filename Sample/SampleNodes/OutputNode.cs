using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class ConsoleOutputNode : Node {

        public ConsoleOutputNode() : base() {
            addInput(1);
        }

        public override void process() {
        }
    }
}
