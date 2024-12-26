using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem
{
    public class UnitManager : MonoBehaviour
    {
        // 맵으로부터 path를 받아 각 유닛에게 전달
        [SerializeField] GameObject prefab;
        [SerializeField] float YMargin;
        private List<Unit> unitList = new();
        private List<Vector3> path = new();
        public List<Vector3> Path { set { path = value; } }

        [ContextMenu("유닛 생성")]
        public void CreateUnit()
        {
            if (path == null) return;
            // 유닛 생성
            GameObject unitObject = Instantiate(prefab, path[0], Quaternion.identity, transform);
            unitList.Add(unitObject.GetComponent<Unit>());

            MoveUnitByPath(unitObject.GetComponent<Unit>());
        }

        public void MoveUnitByPath(Unit unit)
        {
            unit.MoveByPath(path, YMargin);
        }
    }
}