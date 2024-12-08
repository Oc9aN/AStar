using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public static event Action<Node[,]> MapGenerated;
    // 노드
    [Header("노드 오브젝트")]
    public GameObject prefab;
    [Header("노드 간 간격")]
    public float nodeGap;
    // 맵의 크기
    [Header("맵의 크기")]
    public int mapSizeX;
    public int mapSizeY;
    private GameObject[,] objects = null;
    private Node[,] map = null;

    public GameObject GetNode(int x, int y) => objects[x, y];

    [ContextMenu("Create Map")]
    public void CreateMap()
    {
        objects = new GameObject[mapSizeX, mapSizeY];
        map = new Node[mapSizeX, mapSizeY];

        // 노드 생성
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                GameObject grid = Instantiate(prefab, transform);
                grid.transform.position = new Vector3(i * nodeGap, 0, j * nodeGap);
                objects[i, j] = grid;
                map[i, j] = new Node(i, j, 0);
            }
        }
        MapGenerated?.Invoke(map);
    }
}
