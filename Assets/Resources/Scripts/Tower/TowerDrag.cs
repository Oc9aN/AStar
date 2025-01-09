using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerSystem
{
    [RequireComponent(typeof(ITower))]
    public class TowerDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPlaceable, IGameStateListener
    {
        [SerializeField] float yMargin;
        public event UnityAction OnReleaseEvent;
        public IParentable snapNode { get; set; }
        public Transform GetParent() => transform.parent;
        private ITower tower = null;
        private ITower Tower => tower ??= GetComponent<ITower>();
        private bool isActive;
        private GameState gameState;

        public void OnDrag(PointerEventData eventData)
        {
            if (isActive && gameState == GameState.ON_WAVE) return;

            // 타워 비활성화
            ActiveTower(false);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPosition = ray.GetPoint((2f - ray.origin.y) / ray.direction.y);

            transform.position = rayPosition;

            BottomTracking();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isActive) return;   // 활성화 된 객체는 드래그를 안한 셈
            // 위치 확인 후 배치 또는 복귀
            snapNode?.OnEndTarcking();
            snapNode?.SetPlaceOnThis(this);
        }

        // 하단 노드를 트래킹
        private void BottomTracking()
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.yellow);

            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3f);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IParentable>(out IParentable trackingNode))
                {
                    if (snapNode != trackingNode)
                    {
                        snapNode?.OnEndTarcking();  // 이전 노드가 있으면 End실행
                        snapNode = trackingNode;
                        snapNode.OnTracking();      // 새로운 노드로 다시 Tracking
                    }
                }
            }
            else
                snapNode?.OnEndTarcking();
        }

        public void SetParent(Transform parent, bool active = false)
        {
            // 부모가 바뀐건 이전 노드가 놓아줬다는 뜻
            OnReleaseEvent?.Invoke();
            OnReleaseEvent = null;
            transform.SetParent(parent);
            if (active) ActiveTower(true);
            if (parent != null)
            {
                Vector3 newPosition = transform.parent.position;
                newPosition.y += yMargin;
                transform.DOMove(newPosition, 0.5f);
            }
        }

        private void ActiveTower(bool active)
        {
            if (active) Tower.ActiveTower();
            else Tower.InActiveTower();
            isActive = active;
        }

        private void OnDestroy()
        {
            OnReleaseEvent = null;
        }

        public void UpdateGameState(GameState state)
        {
            gameState = state;
        }
    }
}