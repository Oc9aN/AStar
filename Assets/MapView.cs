using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapView : MonoBehaviour, MapInterface.IMapView
{
    [Header("출발지와 목적지")]
    [SerializeField] private int startX;
    [SerializeField] private int startY;
    [SerializeField] private int destX;
    [SerializeField] private int destY;
    public UnityAction OnPathRequested { get; set; }

    private List<MeshRenderer> meshs = new List<MeshRenderer>();
    public void OnPathNodeEvent(GameObject node)
    {
        MeshRenderer mesh = node.GetComponent<MeshRenderer>();
        mesh.material.color = Color.red;
        meshs.Add(mesh);
    }

    public void GetStart(out int startX, out int startY)
    {
        startX = this.startX;
        startY = this.startY;
    }

    public void GetDest(out int destX, out int destY)
    {
        destX = this.destX;
        destY = this.destY;
    }

    [ContextMenu("Start")]
    public void RequestPath()
    {
        ClearMeshs();
        OnPathRequested?.Invoke();
    }

    private void ClearMeshs()
    {
        meshs.ForEach((n) => n.material.color = Color.white);
        meshs.Clear();
    }
}
