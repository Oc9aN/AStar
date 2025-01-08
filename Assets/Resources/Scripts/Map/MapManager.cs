using System.Collections.Generic;
using UnitSystem;
using UnityEngine;
using UnityEngine.Events;

namespace MapSystem
{
    [RequireComponent(typeof(MapGenerator))]
    [RequireComponent(typeof(MapUIInput))]
    public class MapManager : MonoBehaviour
    {
        public event UnityAction<List<Vector3>> OnFindPath;
        private MapGenerator mapGenerator;
        private MapUIInput uiInputView;
        private AStar aStar;
        private List<Node> path = null;
        private void Awake()
        {
            mapGenerator = GetComponent<MapGenerator>();
            uiInputView = GetComponent<MapUIInput>();

            aStar = new AStar();
            // 이벤트 셋팅
            EventSetter();
        }

        private void EventSetter()
        {
            mapGenerator.OnMapModified += SetMap;
        }

        private void Start()
        {
            CreateMap();
        }

        private void SetMap(Node[,] map)
        {
            // 맵이 바뀔때마다 경로 재탐색
            aStar.SetMap(map);
            OnPathRequested();
        }

        private void CreateMap()
        {
            (int x, int y) = uiInputView.GetMapSize();
            mapGenerator.CreateMap(x, y);
        }

        public void OnPathRequested()
        {
            if (path != null && path.Count > 0)
                ResetPath(path);
            (int startX, int startY) = uiInputView.GetStart();
            (int destX, int destY) = uiInputView.GetDest();
            path = aStar.Navigate(startX, startY, destX, destY);
            if (path != null) ShowPath(path);

            // Vector3 리스트로 이벤트 발생
            OnFindPath?.Invoke(GetVectorPath(path));

            // 길 초기화
            void ResetPath(List<Node> path) => path.ForEach((n) =>
                            {
                                GameObject node = mapGenerator.GetNode(n.x, n.y);
                                NodeObject nodeObject = node.GetComponent<NodeObject>();
                                nodeObject.SetNormal(); // 기본으로 복구
                            });
            // 길 표시
            void ShowPath(List<Node> path) => path.ForEach((n) =>
                            {
                                Debug.Log(n.x + ", " + n.y);
                                GameObject node = mapGenerator.GetNode(n.x, n.y);
                                NodeObject nodeObject = node.GetComponent<NodeObject>();
                                nodeObject.SetPath(); // 길로 선정
                            });
            // 위치 리스트 반환
            List<Vector3> GetVectorPath(List<Node> path)
            {
                if (path == null) return null;

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
}