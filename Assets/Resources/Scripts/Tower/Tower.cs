using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

namespace TowerSystem
{
    /// <summary>
    /// 단일 타겟 공격 타워
    /// </summary>
    public class Tower : MonoBehaviour, ITower
    {
        // 가장 가까운 target을 찾고 범위를 나갈 때까지 공격, 이후 다시 반복
        // 데이터
        [SerializeField] protected TowerData[] data;
        [SerializeField] int maxLevel = 2;
        [SerializeField] protected int towerLevel = 0;

        // 배치시 true
        private bool isActive;
        public bool IsActive { get { return isActive; } set { isActive = value; } }

        // 타겟
        private int targetLayer => 1 << LayerMask.NameToLayer("Unit");
        protected GameObject target;

        // 코루틴
        protected Coroutine findingTargetCoroutine;
        protected Coroutine RangeCheckCoroutine;
        private Coroutine AttackCoroutine;

        public (TowerType type, int level) GetTowerInfo() => (data[towerLevel].Type, towerLevel);

        public bool UpgradeTower()
        {
            if (towerLevel + 1 <= maxLevel)
            {
                towerLevel += 1;
                return true;
            }
            return false;
        }
        public void Sacrificed() => Destroy(gameObject);

        public void ActiveTower() => RangeCheckCoroutine = StartCoroutine(GetTarget());

        public void InActiveTower() => StopAllCoroutines();

        public virtual IEnumerator Attack() { yield break; }

        public IEnumerator RangeCheck()
        {
            if (target == null)
                yield break;

            float distance = Vector3.Distance(transform.position, target.transform.position);
            while (distance < data[towerLevel].Radius && target != null)  // 사거리 체크
            {
                Debug.DrawLine(transform.position, target.transform.position, Color.red);
                distance = Vector3.Distance(transform.position, target.transform.position);
                yield return null;
            }

            // 공격 멈추고 다음 타겟 선정
            if (AttackCoroutine != null) StopCoroutine(AttackCoroutine);
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
                hitColliders = Physics.OverlapSphere(transform.position, data[towerLevel].Radius, targetLayer)
            .Where(collider => Vector3.Distance(transform.position, collider.transform.position) <= data[towerLevel].Radius).ToList();
                yield return null;
            }

            // 범위 내 객체를 거리순으로 정렬
            List<Collider> sortedColliders = hitColliders.OrderBy(collider => Vector3.Distance(transform.position, collider.transform.position)).ToList();

            target = sortedColliders[0].gameObject;
            Debug.Log($"타겟 감지 : {target.name}");
            // 타겟과 거리 체크하면서 공격
            RangeCheckCoroutine = StartCoroutine(RangeCheck());
            AttackCoroutine = StartCoroutine(Attack());

            yield break;
        }

        private void OnDrawGizmos()
        {
            if (data == null) return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, data[towerLevel].Radius);
        }
    }
}