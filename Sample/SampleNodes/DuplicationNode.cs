using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class DuplicationNode : Node {

        IntProperty intProp;

        public override void setConnections() {
            //stackParameters.Children.
            title = "Duplication";
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
            object[] results = new object[(int)parameters["Clones"]];
            for (int i = 0; i < (int)parameters["Clones"]; i++) {
                results[i] = ins[0];
            }
            return results;
        }
    }
}
