using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IClickable
{
    public void OnClick();
}

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 레이 발사
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // 클릭한 오브젝트 처리
                Debug.Log($"Clicked on: {hit.collider.gameObject.name}");

                var clickable = hit.collider.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.OnClick();
                }
            }
        }
    }
}
