using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UnitSystem
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private UnitData data;
        [SerializeField] private Image HpImage;
        private int hp;

        private void Awake()
        {
            hp = data.Hp;
        }

        public void MoveByPath(List<Vector3> path, float YMargin)
        {
            // 패스전달 받아 따라 이동
            Sequence sequence = DOTween.Sequence();
            path.ForEach((v) =>
            {
                v.y += YMargin;
                sequence.Append(transform.DOMove(v, data.MoveSpeed));
            });

            sequence.Play();
        }

        public void OnDamaged(int damage)
        {
            hp -= damage;
            float hpPercent = hp / data.Hp;
            if (hpPercent <= 0) hpPercent = 0f; // 사망
            Vector3 newScale = Vector3.one;
            newScale.x = hpPercent;
            HpImage.transform.localScale = newScale;
        }
    }
}