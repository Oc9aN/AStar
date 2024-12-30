using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem
{
    public interface IUnit
    {
        public void MoveByPath(List<Vector3> path, float YMargin);
        public void OnDamaged(int damage);
    }
}