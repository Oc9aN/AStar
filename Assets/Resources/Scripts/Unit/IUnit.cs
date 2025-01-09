using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnitSystem
{
    public interface IUnit
    {
        public event UnityAction OnDestroyEvent;
        public event UnityAction OnDieEvent;
        public event UnityAction OnEndEvent;
        public void MoveByPath(List<Vector3> path);
        public void OnDamaged(int damage);
        public UnitData data { get; set; }
    }
}