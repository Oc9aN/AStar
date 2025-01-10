using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSystem
{
    [RequireComponent(typeof(TowerList))]
    public class TowerManager : MonoBehaviour, ISpendableMoney
    {
        public event UnityAction<GameObject> OnCreateTower;
        public event Func<int, bool> OnSpendMoney;
        [SerializeField] int towerCost = 50;
        [SerializeField] TowerDrag normalTower; // 드래그가 가능한 타워
        private TowerList towerList;

        private void Awake()
        {
            towerList = GetComponent<TowerList>();
        }

        public void TryCreateTowerByRandom(TypeTierData typeTierData)
        {
            (string type, int tier) = RandomManager.RandomTower(typeTierData.types, typeTierData.tierPercents[0].percents);
            CreateTower(type, tier);
        }

        private void CreateTower(string type, int tier)
        {
            if (!towerList.HasEmptyNode() || !OnSpendMoney(towerCost)) return;

            TowerDrag tower = null;
            switch (type)
            {
                case "Normal":
                    switch (tier)
                    {
                        case 0:
                            tower = Instantiate(normalTower, Camera.main.transform.position, Quaternion.identity);
                            break;
                    }
                    break;
                default:
                    tower = Instantiate(normalTower, Camera.main.transform.position, Quaternion.identity);
                    break;
            }

            OnCreateTower?.Invoke(tower.gameObject);
            towerList.AddTowers(tower.GetComponent<IPlaceable>());
        }
    }
}
