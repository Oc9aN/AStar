using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem
{
    //TODO: 게임 매니저와 연결
    public class UnitManager : MonoBehaviour
    {
        // 맵으로부터 path를 받아 각 유닛에게 전달
        [SerializeField] GameObject prefab;
        [SerializeField] float YMargin;
        private List<IUnit> unitList = new();
        private List<Vector3> path = new();
        public List<Vector3> Path { set { path = value; } }

        [ContextMenu("유닛 생성")]
        public void CreateUnit(string type)
        {
            if (path == null) return;
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
        }

        public void MoveUnitByPath(IUnit unit)
        {
            unit.MoveByPath(path, YMargin);
        }
    }
}