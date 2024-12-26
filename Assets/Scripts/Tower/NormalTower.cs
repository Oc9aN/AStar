using System.Collections;
using UnityEngine;

public class NormalTower : Tower
{
    protected override IEnumerator Attack()
    {
        if (target == null) yield break;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        while (distance < radius)
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
            distance = Vector3.Distance(transform.position, target.transform.position);
            yield return null;
        }

        // 다음 타겟 선정
        findingTargetCoroutine = StartCoroutine(GetTarget());
        yield break;
    }
}
