using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MapSystem
{
    public class MapView : MonoBehaviour
    {
        [Header("출발지와 목적지")]
        [SerializeField] private int startX;
        [SerializeField] private int startY;
        [SerializeField] private int destX;
        [SerializeField] private int destY;
        public UnityAction OnPathRequested { get; set; }

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
            OnPathRequested?.Invoke();
        }
    }
}