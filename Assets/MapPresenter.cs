using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapInterface
{
    public interface IMapPresenter
    {
        public void OnPathRequested();
    }

    public interface IMapView
    {
        UnityAction OnPathRequested { get; set; }
        void OnPathNodeEvent(GameObject node);  // 길인 노드 객체에 발생하는 이벤트
        void GetStart(out int startX, out int startY);
        void GetDest(out int destX, out int destY);
    }
}

public class MapPresenter : MapInterface.IMapPresenter
{
    private AStar aStar;
    private MapInterface.IMapView mapView;

    public MapPresenter(AStar aStar, MapView mapView)
    {
        this.aStar = aStar;
        this.mapView = mapView;

        this.mapView.OnPathRequested += OnPathRequested;
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
            mapView.OnPathNodeEvent(aStar.GetNode(path.x, path.y));
            path = path.parent;
        }
    }
}
