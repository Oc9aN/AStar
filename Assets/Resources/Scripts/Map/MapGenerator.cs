using System;
using UnityEngine;
using UnityEngine.Events;

namespace MapSystem
{
    public class MapGenerator : MonoBehaviour
    {
        public event UnityAction<Node[,]> OnMapModified;
        // 노드
        [Header("노드 오브젝트")]
        [SerializeField] GameObject prefab;
        [Header("노드 간 간격")]
        [SerializeField] float nodeGap;
        private GameObject[,] objects = null;
        public GameObject[,] Objects { get { return objects; } }
        private Node[,] map = null;

        public GameObject GetNode(int x, int y) => objects[x, y];

        public void CreateMap(int mapSizeX, int mapSizeY)
        {
            transform.position = Vector3.zero;
            DestroyAllObjects();

            objects = new GameObject[mapSizeX, mapSizeY];
            map = new Node[mapSizeX, mapSizeY];

            // 노드 생성
            for (int x = 0; x < mapSizeX; x++)
            {
                for (int y = 0; y < mapSizeY; y++)
                {
                    GameObject node = Instantiate(prefab, transform);
                    node.transform.position = new Vector3(x * nodeGap, 0, y * nodeGap);
                    objects[x, y] = node;
                    map[x, y] = new Node(x, y, 0);
                    node.GetComponent<NodeObject>().SetPosition(x, y);
                    node.GetComponent<NodeObject>().OnSetObstacleEvent += (int x, int y) => { SetObstacle(x, y); MapModified(); };
                    node.GetComponent<NodeObject>().OnSetNonObstacleEvent += (int x, int y) => { SetNonObstacle(x, y); MapModified(); };
                }
            }

            // 맵 위치 조절
            Vector3 lastPosition = objects[mapSizeX - 1, mapSizeY - 1].transform.position;
            Vector3 newPosition = new Vector3(-lastPosition.x / 2f, 0f, -lastPosition.z / 2f);
            transform.position = newPosition;

            MapModified();
        }

        private void MapModified()
        {
            OnMapModified?.Invoke(map);
        }

        private void SetObstacle(int x, int y)
        {
            if (map == null) return;
            map[x, y].isObstacle = true;
        }

        private void SetNonObstacle(int x, int y)
        {
            if (map == null) return;
            map[x, y].isObstacle = false;
        }

        private void DestroyAllObjects()
        {
            if (objects == null)
                return;

            for (int x = 0; x < objects.GetLength(0); x++)
            {
                for (int y = 0; y < objects.GetLength(1); y++)
                {
                    if (objects[x, y] != null)
                    {
                        Destroy(objects[x, y]);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            OnMapModified = null;
        }
    }
}