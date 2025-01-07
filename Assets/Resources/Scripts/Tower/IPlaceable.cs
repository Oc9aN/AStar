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
        public void SetParent(Transform parent, bool active = false);
        public Transform GetParent();
        public event UnityAction OnReleaseEvent;
    }
}