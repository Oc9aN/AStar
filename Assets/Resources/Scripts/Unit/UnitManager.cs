using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace UnitSystem
{
    public class UnitManager : MonoBehaviour, IAddableMoney, ICausingDamage
    {
        // 유닛에서 실행될 이벤트를 받아서 생성 후 전달
        public event UnityAction<int> OnAddMoney;
        public event UnityAction<int> CausingDamage;
        public event UnityAction OnRemoveAllUnitEvent;

        private IUnitFactory unitFactory;

        [SerializeField] GameObject normal;
        [SerializeField] GameObject fast;
        [SerializeField, ReadOnly] List<IUnit> unitList = new();
        // 맵으로부터 path를 받아 각 유닛에게 전달
        public List<Vector3> path { private get; set; }

        private void Awake()
        {
            unitFactory = new UnitFactory(normal, fast);
        }

        [ContextMenu("유닛 생성")]
        public void CreateUnit(string type)
        {
            if (path == null || path.Count < 1) return;

            // 팩토리를 사용하여 유닛 생성
            IUnit unitObject = unitFactory.CreateUnit(type, path[0], transform);

            unitList.Add(unitObject);

            unitObject.MoveByPath(path);

            // 이벤트 등록
            unitObject.OnDieEvent += () => OnAddMoney?.Invoke(unitObject.data.RewardMoney);
            unitObject.OnEndEvent += () => CausingDamage?.Invoke(unitObject.data.Damage);
            unitObject.OnDestroyEvent += () =>
            {
                unitList.Remove(unitObject);
                if (unitList.Count <= 0) OnRemoveAllUnitEvent?.Invoke(); // 모든 유닛이 제거된 경우 = level 끝
            };
        }
    }
}