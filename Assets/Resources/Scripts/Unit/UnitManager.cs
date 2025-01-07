using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnitSystem
{
    public class UnitManager : MonoBehaviour, IAddableMoney
    {
        // 맵으로부터 path를 받아 각 유닛에게 전달
        public event UnityAction<int> OnAddMoney;

        [SerializeField] GameObject prefab;
        [SerializeField] float YMargin;
        private List<IUnit> unitList = new();
        private List<Vector3> path = new();
        public List<Vector3> Path { set { path = value; } }

        [ContextMenu("유닛 생성")]
        public void CreateUnit(string type)
        {
            if (path == null || path.Count < 1) return;
            // 유닛 생성
            IUnit unitObject = null;
            switch (type)
            {
                case "Normal":
                    unitObject = Instantiate(prefab, path[0], Quaternion.identity, transform).GetComponent<IUnit>();
                    break;
                default:
                    unitObject = Instantiate(prefab, path[0], Quaternion.identity, transform).GetComponent<IUnit>();
                    break;
            }
            unitList.Add(unitObject);

            MoveUnitByPath(unitObject);

            // 제거 이벤트 등록
            unitObject.OnRemoveEvent += () => OnAddMoney?.Invoke(unitObject.rewardMoney);
        }

        public void MoveUnitByPath(IUnit unit)
        {
            unit.MoveByPath(path, YMargin);
        }
    }
}