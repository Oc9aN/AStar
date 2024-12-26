using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // 가장 가까운 target을 찾고 범위를 나갈 때까지 공격, 이후 다시 반복
    [SerializeField] protected float radius;
    //int targetLayer => 1 << LayerMask.NameToLayer("Unit");

    protected GameObject target;

    protected Coroutine findingTargetCoroutine;
    private Coroutine AttackCoroutine;

    private void Start()
    {
        findingTargetCoroutine = StartCoroutine(GetTarget());
    }

    protected virtual IEnumerator Attack() { yield break; }

    protected IEnumerator GetTarget()
    {
        int targetLayer = 1 << LayerMask.NameToLayer("Unit");
        List<Collider> hitColliders = new();

        while (hitColliders.Count <= 0)
        {
            hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer)
        .Where(collider => Vector3.Distance(transform.position, collider.transform.position) <= radius).ToList();
            yield return null;
        }

        Debug.Log("타겟 감지");
        // 범위 내 객체를 거리순으로 정렬
        List<Collider> sortedColliders = hitColliders.OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position)).ToList();

        target = sortedColliders[0].gameObject;
        // 타겟 설정 후 공격
        AttackCoroutine = StartCoroutine(Attack());

        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
