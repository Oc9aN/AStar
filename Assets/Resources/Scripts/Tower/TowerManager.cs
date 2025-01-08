using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSystem
{
    public class TowerManager : MonoBehaviour, ISpendableMoney
    {
        public event Func<int, bool> OnSpendMoney;
        [SerializeField] int towerCost = 50;
        [SerializeField] TowerDrag normalTower; // 드래그가 가능한 타워
        [SerializeField] TowerList towerList;

        public void CreateTowerByRandom(TypeTierData typeTierData)
        {
            (string type, int tier) = RandomManager.RandomTower(typeTierData.types, typeTierData.tierPercents[0].percents);
            CreateTower("type", tier);
        }

        private void CreateTower(string type, int tier)
        {
            if (!OnSpendMoney(towerCost)) return;

            IPlaceable tower = null;
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

            towerList.AddTowers(tower);
        }
    }
}
