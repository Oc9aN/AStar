using System.Collections;
using UnitSystem;
using UnityEngine;

namespace TowerSystem
{
    public class NormalTower : Tower
    {
        //TODO: 이벤트 OnDestroy에서 놓아주기
        protected override IEnumerator Attack()
        {
            if (target == null) yield break;
            Unit targetUnit = target.GetComponent<Unit>();
            while (true)
            {
                // 공격
                targetUnit.OnDamaged(data.Damage);
                yield return new WaitForSeconds(data.Delay);
            }
        }
    }
}