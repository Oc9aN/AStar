using UnityEngine;
using UnityEngine.Events;
public class MapPresenter : MonoBehaviour
{
    [SerializeField] MapGenerator mapGenerator;
    [SerializeField] AStar aStar;
    [SerializeField] PathView mapView;
    [SerializeField] UIInputView uiInputView;

    private void Awake()
    {
        mapGenerator.OnMapModified += (map) => { this.aStar.Map = map; };
        mapView.OnPathRequested += OnPathRequested;
    }

    private void Start()
    {
        CreateMap();
    }

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
            mapView.OnPathNodeEvent(mapGenerator.GetNode(path.x, path.y));
            path = path.parent;
        }
    }
}
