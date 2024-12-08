using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapManager : MonoBehaviour
{
    public MapInterface.IMapView mapView;
    public MapGenerator mapGenerator;
    public AStar aStar;

    public void Start()
    {
        new MapPresenter(aStar, mapGenerator, mapView);
    }
}
