using System;
using DG.Tweening;
using TowerSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapSystem
{
    public interface IPlaceable
    {
        public Tower placedObejct { get; set; }
        public void OnTracking();
        public void OnEndTarcking();
        public void SetPlaceOnThis(TowerDrag gameObject);
        public void OnReleaseObject();
    }
    public class NodeObject : MonoBehaviour, IPointerClickHandler, IPlaceable
    {
        public Action OnSetPathEvent;
        public event Action<int, int> OnSetObstacleEvent;
        private int x;
        private int y;
        public void SetPosition(int x, int y) { this.x = x; this.y = y; }
        public Tower placedObejct { get; set; }

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
        public void SetPlaceOnThis(TowerDrag tower)
        {
            if (placedObejct != null && placedObejct != tower.gameObject)
            {
                // 현재 오브젝트가 있으면 배치를 서로 바꿔줌
                TowerDrag target = placedObejct.transform.GetComponent<TowerDrag>();
                IPlaceable place = tower.PrevNode;
                target.SnapToPlace(place);
            }
            tower.transform.SetParent(transform);
            Vector3 newPosition = transform.position;
            newPosition.y += tower.YMargin;
            tower.transform.DOMove(newPosition, 0.5f);
            placedObejct = tower.Tower;
            placedObejct.ActiveTower();
        }
        public void OnReleaseObject()
        {
            placedObejct.InActiveTower();
            placedObejct = null;
        }
    }
}