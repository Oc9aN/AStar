using System.Collections;
using System.Collections.Generic;
using TowerSystem;
using UnityEngine;

public class UIEvent : MonoBehaviour
{
    [SerializeField] Tower temp;
    [SerializeField] TowerList towerList;
    public void OnCreateTower()
    {
        // 임시로 타워 생성
        Tower tower = Instantiate(temp, Camera.main.transform.position, Quaternion.identity);
        towerList.AddTowers(tower);
    }
}
