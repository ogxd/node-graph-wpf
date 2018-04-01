using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class AdditionNode : Node {

        public AdditionNode() : base() {
            addInput(0);
            addInput(0);
            addOutput(0);
        }

        public override void process() {
        }
    }
}
