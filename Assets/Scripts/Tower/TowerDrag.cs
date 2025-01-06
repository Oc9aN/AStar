using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerSystem
{
    [RequireComponent(typeof(Tower))]
    public class TowerDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IPlaceable
    {
        [SerializeField] float yMargin;
        public event UnityAction<IPlaceable> TrySnapAction;
        public event UnityAction OnSnapEvent;
        public event UnityAction OnReleaseEvent;
        public IParentable snapNode { get; set; }
        public Transform GetParent() => transform.parent;

        private Tower tower = null;
        private Tower Tower => tower ??= GetComponent<Tower>();

        private void Awake()
        {
            TrySnapAction += (IPlaceable tower) => snapNode?.SetPlaceOnThis(tower);
            TrySnapAction += (_) => snapNode?.OnEndTarcking();

            OnSnapEvent += Tower.ActiveTower;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPosition = ray.GetPoint((2f - ray.origin.y) / ray.direction.y);

            transform.position = rayPosition;

            BottomTracking();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // 위치 확인 후 배치 또는 복귀
            TrySnapAction.Invoke(this);
        }

        public void SnapToPlace(IParentable snap)
        {
            snapNode = snap;
            TrySnapAction.Invoke(this);
        }

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
                        snapNode?.OnEndTarcking();
                        snapNode = trackingNode;
                        trackingNode.OnTracking();
                    }
                }
            }
            else
                snapNode?.OnEndTarcking();
        }

        public void SetParent(Transform parent, bool isMove = false, bool isSnapEvent = false)
        {
            // 부모가 바뀐건 이전 노드가 놓아줬다는 뜻
            OnReleaseEvent?.Invoke();
            OnReleaseEvent = null;
            transform.SetParent(parent);
            if (isSnapEvent) OnSnapEvent?.Invoke();
            if (isMove)
            {
                Vector3 newPosition = transform.parent.position;
                newPosition.y += yMargin;
                transform.DOMove(newPosition, 0.5f);
            }
        }

        private void OnDestroy()
        {
            TrySnapAction = null;
        }
    }
}