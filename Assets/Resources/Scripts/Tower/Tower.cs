using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerSystem
{
    /// <summary>
    /// 단일 타겟 공격 타워
    /// </summary>
    public class Tower : MonoBehaviour, ITower
    {
        // 가장 가까운 target을 찾고 범위를 나갈 때까지 공격, 이후 다시 반복
        [SerializeField] protected TowerData data;
        private int targetLayer => 1 << LayerMask.NameToLayer("Unit");

        // 배치시 true
        private bool isActive;
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        protected GameObject target;

        protected Coroutine findingTargetCoroutine;
        protected Coroutine RangeCheckCoroutine;
        private Coroutine AttackCoroutine;

        public void ActiveTower() => RangeCheckCoroutine = StartCoroutine(GetTarget());

        public void InActiveTower() => StopAllCoroutines();

        public virtual IEnumerator Attack() { yield break; }

        public IEnumerator RangeCheck()
        {
            if (target == null) yield break;

            float distance = Vector3.Distance(transform.position, target.transform.position);
            while (distance < data.Radius && target != null)  // 사거리 체크
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.red);
                distance = Vector3.Distance(transform.position, target.transform.position);
                yield return null;
            }

            // 공격 멈추고 다음 타겟 선정
            StopCoroutine(AttackCoroutine);
            target = null;
            findingTargetCoroutine = StartCoroutine(GetTarget());
            yield break;
        }

        public IEnumerator GetTarget()
        {
            Debug.Log($"{transform.name} 타워 활성화");
            List<Collider> hitColliders = new();

            while (hitColliders.Count <= 0)
            {
                hitColliders = Physics.OverlapSphere(transform.position, data.Radius, targetLayer)
            .Where(collider => Vector3.Distance(transform.position, collider.transform.position) <= data.Radius).ToList();
                yield return null;
            }

            Debug.Log("타겟 감지");
            // 범위 내 객체를 거리순으로 정렬
            List<Collider> sortedColliders = hitColliders.OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position)).ToList();

            target = sortedColliders[0].gameObject;
            // 타겟과 거리 체크하면서 공격
            RangeCheckCoroutine = StartCoroutine(RangeCheck());
            AttackCoroutine = StartCoroutine(Attack());

            yield break;
        }

        private void OnDrawGizmos()
        {
            if (data == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, data.Radius);
        }
    }
}