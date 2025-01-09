using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UnitSystem
{
    public class NormalUnit : MonoBehaviour, IUnit
    {
        public event UnityAction OnDestroyEvent;
        public event UnityAction OnDieEvent;
        public event UnityAction OnEndEvent;

        [SerializeField] UnitData unitData;
        public UnitData data { get { return unitData; } set { unitData = value; } }
        [SerializeField] private Image HpImage;
        [SerializeField, ReadOnly] private int hp;
        [SerializeField] float YMargin;
        private Canvas hpCanvas = null;
        private Canvas HpCanvas => hpCanvas ??= GetComponentInChildren<Canvas>();

        private void Awake()
        {
            hp = data.Hp;
        }

        private void Update()
        {
            // 체력바가 카메라를 바라봄
            HpCanvasSetUp();
        }

        private void HpCanvasSetUp()
        {
            if (HpCanvas == null) return;

            Vector3 dir = Camera.main.transform.position - HpCanvas.transform.position;

            HpCanvas.transform.rotation = Quaternion.LookRotation(dir, Vector3.back);
        }

        public void MoveByPath(List<Vector3> path)
        {
            // 패스전달 받아 따라 이동
            Sequence sequence = DOTween.Sequence();
            path.ForEach((v) =>
            {
                v.y += YMargin;
                sequence.Append(transform.DOMove(v, data.MoveSpeed).SetEase(Ease.Linear));
            });

            sequence.OnComplete(() => { OnEndEvent?.Invoke(); Destroy(gameObject); });

            sequence.Play();
        }

        public void OnDamaged(int damage)
        {
            hp -= damage;
            float hpPercent = hp / (float)data.Hp;
            if (hpPercent <= 0)
            {
                hpPercent = 0f; // 사망
                OnDieEvent?.Invoke();
                Destroy(gameObject);
            }
            Vector3 newScale = Vector3.one;
            newScale.x = hpPercent;
            HpImage.transform.localScale = newScale;
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
            OnDieEvent = null;
            OnEndEvent = null;
            OnDestroyEvent = null;
        }
    }
}