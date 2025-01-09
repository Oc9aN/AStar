using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
namespace MapSystem
{
    public class Node
    {
        public int x = 0;
        public int y = 0;
        public int f = 0;
        public int g = 0;
        public int h = 0;
        public bool isObstacle = false;

        public Node parent;

        public Node(int x, int y, int g)
        {
            this.x = x;
            this.y = y;
            this.g = g;
        }

        public void Init()
        {
            f = 0;
            g = 0;
            h = 0;
        }
    }

    enum Dir
    {
        Left, Right, Up, Down,
        LeftUp, RightUp, LeftDown, RightDown
    }

    public class AStar
    {
        public UnityAction<int, int> PathFindAction;

        private Node[,] map = null;

        public void SetMap(Node[,] map)
        {
            this.map = map;
        }

        private void InitMapData()
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    map[x, y].Init();
                }
            }
        }

        public List<Node> Navigate(int startX, int startY, int destX, int destY)
        {
            // 초기화
            InitMapData();
            List<Node> openList = new List<Node>();
            List<Node> closeList = new List<Node>();

            // 부모 노드
            Node parentNode;

            // 탐색 순서 9시 방향부터 시계방향으로
            Dir[] dir = new Dir[] { Dir.Left, Dir.LeftUp, Dir.Up, Dir.RightUp, Dir.Right, Dir.RightDown, Dir.Down, Dir.LeftDown };

            // 출발 및 목표 좌표 설정
            int startX_ = startX;
            int startY_ = startY;
            int destX_ = destX;
            int destY_ = destY;
            int curX = startX_;
            int curY = startY_;

            // 출발 노드 설정
            parentNode = new Node(curX, curY, 0);
            parentNode.h = (Mathf.Abs(curX - destX) + Mathf.Abs(curY - destY)) * 10;    // 맨하탄 거리 방식으로 측정
            parentNode.f = parentNode.g + parentNode.h;
            closeList.Add(parentNode);

            bool isDest = false;

            // 경로 탐색 시작
            while (!isDest)
            {
                // 현 노드에서 다음 노드 경로 탐색
                for (int i = 0; i < dir.Length; i++)
                {
                    (int nextX, int nextY) = GetNextPosition(curX, curY, dir[i]);
                    if (!ExistPosition(nextX, nextY))
                        continue;                   // 맵을 벗어난 위치인 경우

                    Node node = map[nextX, nextY];

                    if (node.isObstacle == true)
                        continue;                   // 장애물인 경우

                    // 이미 방문한 노드인 경우 OR 대각선 이동 불가인 경우
                    // switch (dir[i])
                    // {
                    //     case Dir.LeftUp:
                    //         if (IsObstacleInDirection(parentNode.x - 1, parentNode.y))
                    //             continue;
                    //         if (IsObstacleInDirection(parentNode.x, parentNode.y + 1))
                    //             continue;
                    //         break;
                    //     case Dir.RightUp:
                    //         if (IsObstacleInDirection(parentNode.x + 1, parentNode.y))
                    //             continue;
                    //         if (IsObstacleInDirection(parentNode.x, parentNode.y + 1))
                    //             continue;
                    //         break;
                    //     case Dir.LeftDown:
                    //         if (IsObstacleInDirection(parentNode.x - 1, parentNode.y))
                    //             continue;
                    //         if (IsObstacleInDirection(parentNode.x, parentNode.y - 1))
                    //             continue;
                    //         break;
                    //     case Dir.RightDown:
                    //         if (IsObstacleInDirection(parentNode.x + 1, parentNode.y))
                    //             continue;
                    //         if (IsObstacleInDirection(parentNode.x, parentNode.y + 1))
                    //             continue;
                    //         break;
                    // }
                    Node isClose = closeList.Find((n) => n.x == nextX && n.y == nextY);
                    if (isClose != null)
                        continue;
                    Node isOpen = openList.Find((n) => n.x == nextX && n.y == nextY);
                    if (isOpen != null)
                        continue;

                    // 부모 지정 후 open에 추가
                    node.parent = parentNode;
                    openList.Add(node);

                    // g값 구하기
                    // 대각선: 14, 한칸: 10
                    int distance = Mathf.Abs(nextX - startX) + Mathf.Abs(nextY - startY);
                    if (distance == 1) node.g += parentNode.g + 10;
                    else node.g += parentNode.g + 14;

                    node.h = (Mathf.Abs(nextX - destX) + Mathf.Abs(nextY - destY)) * 10;
                    node.f = node.g + node.h;

                    // 목적지 도착
                    if (node.h == 0)
                    {
                        Debug.Log("탐색 성공");
                        isDest = true;
                        Node path = node;
                        List<Node> pathList = new();
                        while (path != null)
                        {
                            pathList.Add(path);
                            path = path.parent;
                        }
                        return pathList;
                    }
                }
                // 방향을 다 둘러본 후

                // 갈 수 있는 방향이 없는 경우
                if (openList.Count == 0)
                {
                    Debug.Log("목적지에 도달할 수 없습니당");
                    return null;
                }

                // 최단 거리 찾기 = f가 최소인 노드
                Node fastNode = openList.FirstOrDefault(n => n.f == openList.Min(n => n.f));

                // 최단 거리 노드 close에 추가 및 부모노드로 설정, open리스트에서 제거
                closeList.Add(fastNode);
                parentNode = fastNode;
                openList.RemoveAll((n) => n.x == fastNode.x && n.y == fastNode.y);

                curX = fastNode.x;
                curY = fastNode.y;
            }
            return null;
        }

        private (int, int) GetNextPosition(int x, int y, Dir direction)
        {
            return direction switch
            {
                Dir.Left => (x - 1, y),
                Dir.Right => (x + 1, y),
                Dir.Up => (x, y + 1),
                Dir.Down => (x, y - 1),
                Dir.LeftUp => (x - 1, y + 1),
                Dir.RightUp => (x + 1, y + 1),
                Dir.LeftDown => (x - 1, y - 1),
                Dir.RightDown => (x + 1, y + 1),
                _ => (x, y)
            };
        }

        private bool ExistPosition(int x, int y) =>
        x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);

        private bool IsObstacleInDirection(int x, int y) =>
        ExistPosition(x, y) && map[x, y].isObstacle;
    }
}