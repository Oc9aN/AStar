using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnitSystem
{
    public interface IUnit
    {
        public event UnityAction OnRemoveEvent;
        public void MoveByPath(List<Vector3> path, float YMargin);
        public void OnDamaged(int damage);
        public int rewardMoney { get; set; }
    }
}