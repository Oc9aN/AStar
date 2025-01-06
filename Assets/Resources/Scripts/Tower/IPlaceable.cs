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
        public void SetParent(Transform parent, bool isMove = false, bool isSnapEvent = false);
        public Transform GetParent();
        public event UnityAction OnSnapEvent;
        public event UnityAction OnReleaseEvent;
    }
}