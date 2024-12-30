using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TowerSystem
{
    public interface IPlaceable
    {
        public IParentable snapNode { get; set; }
        public void SetParent(Transform parent, bool isMove = false);
        public Transform GetParent();
    }
    [RequireComponent(typeof(Tower))]
    public class TowerDrag : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPlaceable
    {
        [SerializeField] float yMargin;
        public event UnityAction<TowerDrag> SnapAction;

        public IParentable snapNode { get; set; }
        public void SetParent(Transform parent, bool isMove = false)
        {
            transform.SetParent(parent);
            if (isMove)
            {
                Vector3 newPosition = transform.parent.position;
                newPosition.y += yMargin;
                transform.DOMove(newPosition, 0.5f);
            }
        }
        public Transform GetParent() => transform.parent;

        private void Awake()
        {
            SnapAction += (TowerDrag tower) => snapNode?.SetPlaceOnThis(tower);
            SnapAction += (_) => snapNode?.OnEndTarcking();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            //prevNode = snapNode;
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

        public void SnapToPlace(IParentable snap)
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

        private void OnDestroy()
        {
            SnapAction = null;
        }
    }
}