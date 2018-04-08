using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class IntToHexNode : Node {

        public override void setConnections() {
            title = "To string";
            addInput(0);
            addOutput(1);
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters) {
            object[] results = new object[1];
            results[0] = ((int)ins[0]).ToString();
            return results;
        }
    }
}
