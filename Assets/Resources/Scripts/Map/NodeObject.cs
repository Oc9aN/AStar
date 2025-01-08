using System;
using TowerSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapSystem
{
    public class NodeObject : MonoBehaviour, IParentable
    {
        public event Action<int, int> OnSetObstacleEvent;
        public event Action<int, int> OnSetNonObstacleEvent;
        private int x;
        private int y;
        public void SetPosition(int x, int y) { this.x = x; this.y = y; }
        public IPlaceable placedObejct { get; set; }

        private MeshRenderer meshRenderer;
        private Color MainColor = Color.white;
        private Color prevColor;

        private void Awake()
        {
            prevColor = MainColor;
            placedObejct = null;
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetPath()
        {
            meshRenderer.material.color = Color.red;
        }

        public void SetNormal()
        {
            meshRenderer.material.color = MainColor;
        }

        private void OnDestroy()
        {
            OnSetObstacleEvent = null;
            OnSetNonObstacleEvent = null;
        }

        public void OnTracking()
        {
            prevColor = meshRenderer.material.color;
            meshRenderer.material.color = Color.yellow;
        }
        public void OnEndTarcking()
        {
            meshRenderer.material.color = prevColor;
            prevColor = MainColor;
        }
        public void SetPlaceOnThis(IPlaceable tower)
        {
            if (transform.childCount > 0)
            {
                // 현재 오브젝트가 있으면 배치를 서로 바꿔줌
                IPlaceable placedObejct = transform.GetChild(0).GetComponent<IPlaceable>();
                Transform exchangeNode = tower.GetParent();
                tower.SetParent(null);
                placedObejct.SetParent(exchangeNode);
            }
            tower.SetParent(transform, true);
            tower.OnReleaseEvent += () => OnSetNonObstacleEvent?.Invoke(x, y);
            // 장애물 설정
            OnSetObstacleEvent?.Invoke(x, y);
        }
    }
}