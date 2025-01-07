using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MapSystem;
using TowerSystem;
using UnityEngine;

public class TowerListNode : MonoBehaviour, IParentable
{
    public Tower placedObejct { get; set; }
    private MeshRenderer meshRenderer = null;
    private MeshRenderer MeshRenderer => meshRenderer ??= GetComponent<MeshRenderer>();
    public void OnTracking() => MeshRenderer.material.color = Color.yellow;
    public void OnEndTarcking() => MeshRenderer.material.color = Color.white;
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
        tower.SetParent(transform);
    }
}
