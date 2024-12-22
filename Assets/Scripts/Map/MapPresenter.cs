using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MapSystem
{
    public class MapPresenter : MonoBehaviour
    {
        [SerializeField] MapGenerator mapGenerator;
        [SerializeField] MapView mapView;
        [SerializeField] UIInputView uiInputView;
        [SerializeField] UnitSystem.UnitManager unitManager;
        private AStar aStar;
        private void Awake()
        {
            aStar = new AStar();
            // 이벤트 셋팅
            EventSetter();
        }

        private void EventSetter()
        {
            mapGenerator.OnMapModified += SetMap;
            mapView.OnPathRequested += OnPathRequested;
            mapView.OnCreateMapRequested += CreateMap;
        }

        private void Start()
        {
            CreateMap();
        }

        private void SetMap(Node[,] map)
        {
            aStar.SetMap(map);
        }

        private void CreateMap()
        {
            (int x, int y) = uiInputView.GetMapSize();
            mapGenerator.CreateMap(x, y);
        }

        public void OnPathRequested()
        {
            (int startX, int startY) = uiInputView.GetStart();
            (int destX, int destY) = uiInputView.GetDest();
            var path = aStar.Navigate(startX, startY, destX, destY);
            ShowPath(path);

            // Vector3 리스트로 UnitManager에게 전달
            unitManager.Path = GetVectorPath(path);
        }

        private void ShowPath(List<Node> path) => path.ForEach((n) =>
                        {
                            Debug.Log(n.x + ", " + n.y);
                            GameObject node = mapGenerator.GetNode(n.x, n.y);
                            NodeObject nodeObject = node.GetComponent<NodeObject>();
                            nodeObject.OnSetPathEvent.Invoke(); // 길로 선정되었다고 이벤트 전달
                        });

        private List<Vector3> GetVectorPath(List<Node> path)
        {
            List<Vector3> vectorPath = new();
            path.ForEach((n) =>
                        {
                            GameObject node = mapGenerator.GetNode(n.x, n.y);
                            vectorPath.Add(node.transform.position);
                        });
            vectorPath.Reverse(); // 리스트를 뒤집음
            return vectorPath;
        }
    }
}