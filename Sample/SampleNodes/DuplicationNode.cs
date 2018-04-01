using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class DuplicaionNode : Node {

        public override void setConnections() {
            
            addInput(0);
            addOutput(0);
        }

        public override object[] process(object[] ins) {
            object[] results = new object[1];
            results[0] = (int)ins[0] + (int)ins[1];
            return results;
        }
    }
}
