using UnityEngine;

namespace MapSystem
{
    public class MapPresenter : MonoBehaviour
    {
        [SerializeField] MapGenerator mapGenerator;
        [SerializeField] MapView mapView;
        [SerializeField] UIInputView uiInputView;

        private AStar aStar;
        private void Awake()
        {
            aStar = new AStar();
            mapGenerator.OnMapModified += SetMap;
            mapView.OnPathRequested += OnPathRequested;
        }

        private void Start()
        {
            CreateMap();
        }

        private void SetMap(Node[,] map)
        {
            aStar.SetMap(map);
        }

        [ContextMenu("Create Map")]
        private void CreateMap()
        {
            (int x, int y) = uiInputView.getMapSize();
            mapGenerator.CreateMap(x, y);
        }

        public void OnPathRequested()
        {
            int startX;
            int startY;
            int destX;
            int destY;
            mapView.GetStart(out startX, out startY);
            mapView.GetDest(out destX, out destY);
            var path = aStar.Navigate(startX, startY, destX, destY);
            while (path != null)
            {
                Debug.Log(path.x + ", " + path.y);
                GameObject node = mapGenerator.GetNode(path.x, path.y);
                NodeObject nodeObject = node.GetComponent<NodeObject>();
                nodeObject.OnSetPathEvent.Invoke();
                // mapView.OnPathNodeEvent(mapGenerator.GetNode(path.x, path.y));
                path = path.parent;
            }
        }
    }
}