using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MapSystem
{
    public class MapUIInput : MonoBehaviour
    {
        [SerializeField] int xSize = 0;
        [SerializeField] int ySize = 0;
        [SerializeField] int xStart = 0;
        [SerializeField] int yStart = 0;
        [SerializeField] int xDest = 0;
        [SerializeField] int yDest = 0;

        public (int x, int y) GetMapSize()
        {
            return (xSize, ySize);
        }

        public (int x, int y) GetStart()
        {
            return (xStart - 1, yStart - 1);
        }

        public (int x, int y) GetDest()
        {
            return (xDest - 1, yDest - 1);
        }
    }
}