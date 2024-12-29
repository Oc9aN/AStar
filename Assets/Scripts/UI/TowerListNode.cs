using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapSystem;
using TowerSystem;
using UnityEngine;

public class TowerListNode : MonoBehaviour, IPlaceable
{
    public Tower placedObejct { get; set; }
    private MeshRenderer meshRenderer => GetComponent<MeshRenderer>();
    public void OnTracking() => meshRenderer.material.color = Color.yellow;
    public void OnEndTarcking() => meshRenderer.material.color = Color.white;
    public void SetPlaceOnThis(TowerDrag tower)
    {
        if (placedObejct != null && placedObejct != tower.gameObject)
        {
            // 현재 오브젝트가 있으면 배치를 서로 바꿔줌
            TowerDrag target = placedObejct.transform.GetComponent<TowerDrag>();
            IPlaceable place = tower.GetComponent<TowerDrag>().PrevNode;
            target.SnapToPlace(place);
        }
        tower.transform.SetParent(transform);
        Vector3 newPosition = transform.position;
        newPosition.y += tower.YMargin;
        tower.transform.DOMove(newPosition, 0.5f);
        placedObejct = tower.GetComponent<Tower>();
    }
    public void OnReleaseObject()
    {
        placedObejct = null;
    }
}
