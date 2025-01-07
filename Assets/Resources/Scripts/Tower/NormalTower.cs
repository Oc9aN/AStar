using System.Collections;
using UnitSystem;
using UnityEngine;

namespace TowerSystem
{
    public class NormalTower : Tower
    {
        protected override IEnumerator Attack()
        {
            if (target == null) yield break;
            IUnit targetUnit = target.GetComponent<IUnit>();
            while (true && targetUnit != null)
            {
                // 공격
                targetUnit.OnDamaged(data.Damage);
                yield return new WaitForSeconds(data.Delay);
            }
        }
    }
}