using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ogxd.NodeGraph {

    public class ShuffleNode : Node {

        IntProperty intProp;

        public override void setConnections() {
            //stackParameters.Children.
            title = "Shuffle";
            addOutput(0);
            intProp = addProperty(new IntProperty { label = "Inputs", value = 2 });
            intProp.valueChanged += IntProp_valueChanged;
            IntProp_valueChanged();
        }

        private void IntProp_valueChanged() {
            clearInputs();
            for (int i = 0; i < (int)intProp.value; i ++) {
                addInput(0);
            }
        }

        public override object[] process(object[] ins, Dictionary<string, object> parameters) {
            var inputs = getInputs();
            Random rnd = new Random();
            int c = rnd.Next(0, inputs.Length);
            object[] results = new object[1];
            results[0] = ins[c];
            return results;
        }
    }
}
