using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class IntNode : Node {

        public override void setConnections() {
            addOutput(0);
        }

        public override object[] process(object[] ins) {
            object[] results = new object[1];
            results[0] = 10;
            return results;
        }
    }
}
