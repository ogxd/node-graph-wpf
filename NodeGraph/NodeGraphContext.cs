using System.Windows.Media;

namespace Ogxd.NodeGraph {

    public enum NodeGraphOrientation {
        LeftToRight,
        RightToLeft,
        UpToBottom,
        BottomToUp
    }

    public class NodeGraphContext {

        public delegate Brush TypeToBrushHandler(int type);
        public delegate void PropertyChangedHandler(string propertyName);

        public event PropertyChangedHandler propertyChanged;

        private NodeGraphOrientation _orientation = NodeGraphOrientation.LeftToRight;
        public NodeGraphOrientation orientation {
            get {
                return _orientation;
            }
            set {
                if (_orientation == value)
                    return;

                _orientation = value;
                propertyChanged?.Invoke("orientation");
            }
        }

        public TypeToBrushHandler typeToBrushHandler { get; set; } = (type) => {
            return new SolidColorBrush(Extensions.GetUniqueColor(type));
        };

        public Brush getPipeColor(int type) {
            return typeToBrushHandler.Invoke(type);
        }
    }
}
