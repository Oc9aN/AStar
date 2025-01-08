using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITower
{
    public void ActiveTower();
    public void InActiveTower();
    public IEnumerator Attack();
    public IEnumerator RangeCheck();
    public IEnumerator GetTarget();
}
