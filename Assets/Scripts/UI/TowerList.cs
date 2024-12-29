using System.Collections;
using System.Collections.Generic;
using MapSystem;
using TowerSystem;
using UnityEngine;

public class TowerList : MonoBehaviour
{
    [SerializeField] GameObject grid;
    [SerializeField] List<Tower> towers = new();
    public List<Tower> Towers { private get { return towers; } set { towers = value; } }
    [SerializeField] float leftMargin = 0;
    [SerializeField] float rightMargin = 0;
    [SerializeField] float space = 0;

    public void AddTowers(Tower tower)
    {
        towers.Add(tower);

        // 하단에 타워 및 타워 발판 추가
        Vector3 firstPosition = transform.position;
        firstPosition.x += leftMargin;

        GameObject gridObject = Instantiate(grid);
        Vector3 setPositon = firstPosition;
        setPositon.x += (space * (towers.Count - 1));
        gridObject.transform.position = setPositon;

        gridObject.GetComponent<IPlaceable>().SetPlaceOnThis(tower.GetComponent<TowerDrag>());
        gridObject.transform.SetParent(transform);

        tower.GetComponent<TowerDrag>().SnapNode = gridObject.GetComponent<IPlaceable>();
        // 타워 갯수에 맞게 ScrollView 조정
        RectTransform rectTransform = transform as RectTransform;

        Vector3 lastPosition = towers[towers.Count - 1].transform.position;
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
