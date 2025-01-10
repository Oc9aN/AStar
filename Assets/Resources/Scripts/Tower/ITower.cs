using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    public void ActiveTower();
    public void InActiveTower();
    public (TowerType type, int level) GetTowerInfo();
    public bool UpgradeTower();
    public void Sacrificed();
}
