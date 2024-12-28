using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace MapSystem
{
    public class MapView : MonoBehaviour
    {
        public event UnityAction OnPathRequested;
        public event UnityAction OnCreateMapRequested;

        [ContextMenu("Start")]
        public void RequestPath()
        {
            OnPathRequested?.Invoke();
        }

        public void CreateMap()
        {
            OnCreateMapRequested?.Invoke();
        }

        private void OnDestroy()
        {
            OnPathRequested = null;
            OnCreateMapRequested = null;
        }
    }
}