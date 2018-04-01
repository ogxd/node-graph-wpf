using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class IntNode : Node {

        public IntNode() : base() {
            addOutput(0);
        }

        public override void process() {
        }
    }
}
