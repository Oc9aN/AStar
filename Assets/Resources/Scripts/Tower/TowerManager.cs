using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace TowerSystem
{
    [RequireComponent(typeof(TowerList))]
    public class TowerManager : MonoBehaviour, ISpendableMoney, IGameStateListener
    {
        public event UnityAction<GameObject> OnCreateTower;
        public UnityAction DelayedUpgrade;
        public event Func<int, bool> OnSpendMoney;

        // 타워 설정
        [Header("업그레이드에 필요한 동일 타워 수")]
        [SerializeField] int upgradeCount = 3;
        [Header("타워 뽑기 비용")]
        [SerializeField] int towerCost = 50;
        [Header("타워 종류")]
        [SerializeField] TowerDrag normalTower; // 드래그가 가능한 타워

        // 매니저 변수
        private TowerList towerList;
        private GameState gameState;

        // 타워를 리스트로 관리
        private List<ITower> towers = new();

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
            towerList.AddTowers(tower);

            AddTowerList(tower.GetComponent<ITower>());
        }

        private void AddTowerList(ITower tower)
        {
            towers.Add(tower);
            CheckUpgrade(tower);
        }

        private void CheckUpgrade(ITower tower)
        {
            // 동일 타워 갯수 확인
            List<ITower> upgradeTargets = towers.Where(n => n.GetTowerInfo() == tower.GetTowerInfo()).ToList();
            if (upgradeTargets.Count >= upgradeCount)
            {
                if (gameState != GameState.ON_WAVE)
                    Upgrade(upgradeTargets[0], upgradeTargets[1], upgradeTargets[2]);
                else
                    DelayedUpgrade += () => Upgrade(upgradeTargets[0], upgradeTargets[1], upgradeTargets[2]);
            }
        }

        private void Upgrade(ITower target, ITower sacrifice1, ITower sacrifice2)
        {
            if (!target.UpgradeTower())
                return;
            towers.Remove(sacrifice1);
            towers.Remove(sacrifice2);
            sacrifice1.Sacrificed();
            sacrifice2.Sacrificed();
            CheckUpgrade(target);
        }

        public void UpdateGameState(GameState state)
        {
            gameState = state;
        }
    }
}
