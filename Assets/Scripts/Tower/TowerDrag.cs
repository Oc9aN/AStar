using System.Collections;
using System.Collections.Generic;
using MapSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerSystem
{
    [RequireComponent(typeof(Tower))]
    public class TowerDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        public event UnityAction SnapRelaseAction;
        public event UnityAction<TowerDrag> SnapAction;
        // 높이 보정
        [SerializeField] float yMargin;
        public float YMargin { get { return yMargin; } }
        // 스냅할 노드드
        private IPlaceable snapNode = null;
        public IPlaceable SnapNode { get { return snapNode; } set { snapNode = value; } }
        // 이전에 스냅되어 있던 노드, 다른 타워와 교체시 사용
        private IPlaceable prevNode = null;
        public IPlaceable PrevNode { get { return prevNode; } }

        private Tower tower = null;
        public Tower Tower { get { return tower; } }

        private void Awake()
        {
            tower = transform.GetComponent<Tower>();
            SnapAction += (TowerDrag tower) => snapNode?.SetPlaceOnThis(tower);
            SnapAction += (_) => snapNode?.OnEndTarcking();
            SnapRelaseAction += () => snapNode?.OnReleaseObject();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            SnapRelaseAction?.Invoke();
            prevNode = snapNode;
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
            SnapAction.Invoke(this);
        }

        public void SnapToPlace(IPlaceable snap)
        {
            snapNode = snap;
            SnapAction.Invoke(this);
        }

        private void BottomTracking()
        {
            Debug.DrawRay(transform.position, Vector3.down, Color.yellow);

            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 3f);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IPlaceable>(out IPlaceable trackingNode))
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

        private void OnDestroy()
        {
            SnapAction = null;
            SnapRelaseAction = null;
        }
    }
}