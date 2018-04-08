using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Ogxd.NodeGraph {

    public class NodeGraphContext {

        public double pipeStiffness { get; set; } = 50;

        public delegate Brush TypeToBrushHandler(int type);

        public TypeToBrushHandler typeToBrushHandler { get; set; } = (type) => {
            return new SolidColorBrush(Extensions.GetUniqueColor(type));
        };

        public Brush getPipeColor(int type) {
            return typeToBrushHandler.Invoke(type);
        }
    }
}
