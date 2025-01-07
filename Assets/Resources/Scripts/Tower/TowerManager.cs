using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerSystem
{
    public class TowerManager : MonoBehaviour
    {
        [SerializeField] int towerCost = 50;
        [SerializeField] Tower normalTower;
        [SerializeField] TowerList towerList;
        private IGameManager gameManager;
        public IGameManager GameManager
        {
            get { return gameManager; }
        }

        public void Init(IGameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public void CreateTower(string type, int tier)
        {
            if (!gameManager.TrySpendMoney(towerCost)) return;
            Tower tower = null;
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
