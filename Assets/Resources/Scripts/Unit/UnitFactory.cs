using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitSystem
{
    public interface IUnitFactory
    {
        IUnit CreateUnit(string type, Vector3 position, Transform parent);
    }

    public class UnitFactory : IUnitFactory
    {
        private GameObject normal;
        private GameObject fast;

        public UnitFactory(GameObject normalPrefab, GameObject fastPrefab)
        {
            normal = normalPrefab;
            fast = fastPrefab;
        }

        public IUnit CreateUnit(string type, Vector3 position, Transform parent)
        {
            GameObject unitPrefab;
            switch (type)
            {
                case "Normal":
                    unitPrefab = normal;
                    break;
                case "Fast":
                    unitPrefab = fast;
                    break;
                default:
                    unitPrefab = normal;
                    break;
            }

            return Object.Instantiate(unitPrefab, position, Quaternion.identity, parent).GetComponent<IUnit>();
        }
    }
}