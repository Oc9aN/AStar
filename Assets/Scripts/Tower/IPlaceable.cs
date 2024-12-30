using System.Collections;
using System.Collections.Generic;
using MapSystem;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSystem
{
    public interface IPlaceable
    {
        public IParentable snapNode { get; set; }
        public void SetParent(Transform parent, bool isMove = false, UnityAction OnSnapEvent = null);
        public Transform GetParent();
        public void OnSnapEvent();
    }
}