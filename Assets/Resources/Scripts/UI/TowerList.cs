using System.Collections;
using System.Collections.Generic;
using MapSystem;
using TowerSystem;
using UnityEngine;

public class TowerList : MonoBehaviour
{
    [SerializeField] GameObject grid;
    [SerializeField] float leftMargin = 0;
    [SerializeField] float rightMargin = 0;
    [SerializeField] float space = 0;

    public void AddTowers(IPlaceable tower)
    {
        // 하단에 타워 및 타워 발판 추가
        Vector3 firstPosition = transform.position;
        firstPosition.x += leftMargin;

        GameObject gridObject = Instantiate(grid);
        gridObject.transform.SetParent(transform);

        Vector3 setPositon = firstPosition;
        setPositon.x += space * transform.childCount;
        gridObject.transform.position = setPositon;

        tower.SetParent(gridObject.transform);
        // 타워 갯수에 맞게 ScrollView 조정
        RectTransform rectTransform = transform as RectTransform;

        Vector3 lastPosition = gridObject.transform.position;
        lastPosition.x += rightMargin;
        Vector2 listPosition = rectTransform.InverseTransformPoint(lastPosition);

        if (rectTransform.rect.max.x <= listPosition.x)
        {
            Vector2 newOffsetMax = rectTransform.offsetMax;
            newOffsetMax.x = listPosition.x - rectTransform.rect.max.x;
            rectTransform.offsetMax = newOffsetMax;
        }
    }
}
