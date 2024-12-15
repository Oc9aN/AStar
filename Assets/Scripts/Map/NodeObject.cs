using System;
using UnityEngine;

public class NodeObject : MonoBehaviour, IClickable
{
    public event Action<int, int> OnSetObstacleEvent;
    private int x;
    private int y;
    public void SetPosition(int x, int y) { this.x = x; this.y = y; }

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnClick()
    {
        SetObstacle();
    }

    public void SetObstacle()
    {
        Debug.Log($"장애물 설정: {x}, {y}");
        meshRenderer.material.color = Color.black;
        OnSetObstacleEvent?.Invoke(x, y);
    }
}
