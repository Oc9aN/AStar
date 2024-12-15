using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public event Action<Node[,]> OnMapModified;
    // 노드
    [Header("노드 오브젝트")]
    [SerializeField] GameObject prefab;
    [Header("노드 간 간격")]
    [SerializeField] float nodeGap;
    // 맵의 크기
    // private int mapSizeX;
    // private int mapSizeY;
    private GameObject[,] objects = null;
    private Node[,] map = null;

    public GameObject GetNode(int x, int y) => objects[x, y];

    [ContextMenu("Create Map")]
    public void CreateMap(int mapSizeX, int mapSizeY)
    {

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
}
