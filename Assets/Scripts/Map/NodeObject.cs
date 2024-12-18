using System;
using UnityEngine;

namespace MapSystem
{
    public class NodeObject : MonoBehaviour, IClickable
    {
        public Action OnSetPathEvent;
        public event Action<int, int> OnSetObstacleEvent;
        private int x;
        private int y;
        public void SetPosition(int x, int y) { this.x = x; this.y = y; }

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            OnSetPathEvent += SetPath;
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OnClick()
        {
            SetObstacle();
        }

        public void SetObstacle()
        {
            Debug.Log($"장애물 설정: {x}, {y}");
            meshRenderer.material.color = Color.black;
            OnSetObstacleEvent?.Invoke(x, y);
        }

        public void SetPath()
        {
            meshRenderer.material.color = Color.red;
        }

        private void OnDestroy()
        {
            OnSetPathEvent = null;
            OnSetObstacleEvent = null;
        }
    }
}