using System;
using TowerSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapSystem
{
    public interface IParentable
    {
        public void OnTracking();
        public void OnEndTarcking();
        public void SetPlaceOnThis(IPlaceable placeable);
    }
    public class NodeObject : MonoBehaviour, IPointerClickHandler, IParentable
    {
        public Action OnSetPathEvent;
        public event Action<int, int> OnSetObstacleEvent;
        private int x;
        private int y;
        public void SetPosition(int x, int y) { this.x = x; this.y = y; }
        public IPlaceable placedObejct { get; set; }

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            placedObejct = null;
            OnSetPathEvent += SetPath;
            meshRenderer = GetComponent<MeshRenderer>();
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

        public void OnPointerClick(PointerEventData eventData)
        {
            SetObstacle();
        }

        public void OnTracking() => meshRenderer.material.color = Color.yellow;
        public void OnEndTarcking() => meshRenderer.material.color = Color.white;
        public void SetPlaceOnThis(IPlaceable tower)
        {
            if (transform.childCount > 0)
            {
                // 현재 오브젝트가 있으면 배치를 서로 바꿔줌
                IPlaceable placedObejct = transform.GetChild(0).GetComponent<IPlaceable>();
                Transform exchangeNode = tower.GetParent();
                tower.SetParent(null);
                placedObejct.SetParent(exchangeNode, true);
            }
            tower.SetParent(transform, true);
        }
    }
}