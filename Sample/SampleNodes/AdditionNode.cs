using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class AdditionNode : Node {

        public override void setConnections() {
            addInput(0);
            addInput(0);
            addOutput(0);
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters) {
            object[] results = new object[1];
            results[0] = (int)ins[0] + (int)ins[1];
            return results;
        }
    }
}
