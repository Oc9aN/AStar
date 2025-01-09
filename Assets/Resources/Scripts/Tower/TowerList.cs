using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace TowerSystem
{
    public class TowerList : MonoBehaviour
    {
        private List<TowerListNode> towerListNodes;

        private void Awake()
        {
            towerListNodes = GetComponentsInChildren<TowerListNode>().ToList();
        }

        public void AddTowers(IPlaceable placeable)
        {
            towerListNodes.ForEach(n =>
            {
                if (!n.HasChild())  // 빈 노드를 찾아서 배치
                    placeable.SetParent(n.transform);
            });
        }
    }
}