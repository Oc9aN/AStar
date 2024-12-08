using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapSettings
{
    public GameObject NodePrefab { get; set; }
    public Transform NodeParent { get; set; }
    public float NodeGap { get; set; }
    public int MapSizeX { get; set; }
    public int MapSizeY { get; set; }
}
public class MapManager : MonoBehaviour
{
    public MapView mapView;
    public AStar aStar;
    public Transform nodeParent;
    public GameObject node;
    [Header("노드 간격")]
    public float nodeGap;
    [Header("맵 크기")]
    public int mapSizeX;
    public int mapSizeY;

    public void Start()
    {
        var settings = new MapSettings
        {
            NodePrefab = node,
            NodeParent = nodeParent,
            NodeGap = nodeGap,
            MapSizeX = mapSizeX,
            MapSizeY = mapSizeY,
        };
        aStar.Init(settings);
        new MapPresenter(aStar, mapView);
    }
}
