using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class ConsoleOutputNode : Node {

        public override void setConnections() {
            addInput(1);
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters) {
            Console.WriteLine("Result : " + ins[0]);
            return null;
        }
    }
}
