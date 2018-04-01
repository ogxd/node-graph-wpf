using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class IntToHexNode : Node {

        public IntToHexNode() : base() {
            addInput(0);
            addOutput(1);
        }

        public override void process() {
            //intValue.ToString("X");
        }
    }
}
