using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class DuplicaionNode : Node {

        IntProperty intProp;

        public override void setConnections() {
            //stackParameters.Children.
            addInput(0);
            intProp = addProperty(new IntProperty { label = "Clones", value = 2 });
            intProp.valueChanged += IntProp_valueChanged;
            IntProp_valueChanged();
        }

        private void IntProp_valueChanged() {
            clearOutputs();
            for (int i = 0; i < (int)intProp.value; i ++) {
                addOutput(0);
            }
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters) {
            object[] results = new object[1];
            results[0] = (int)ins[0] + (int)ins[1];
            return results;
        }
    }
}
