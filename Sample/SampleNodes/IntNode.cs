using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class IntNode : Node {

        public override void setConnections() {
            title = "Integer";
            addOutput(0);
            addProperty(new IntProperty { label = "Integer", value = 5 });
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters) {
            object[] results = new object[1];
            results[0] = (int)parameters["Integer"];
            return results;
        }
    }
}
