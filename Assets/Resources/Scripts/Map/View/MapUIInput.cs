using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MapSystem
{
    public class MapUIInput : MonoBehaviour
    {
        [SerializeField] TMP_Text xSizeText;
        [SerializeField] TMP_Text ySizeText;

        [SerializeField] TMP_Text xStart;
        [SerializeField] TMP_Text yStart;

        [SerializeField] TMP_Text xDest;
        [SerializeField] TMP_Text yDest;

        public (int x, int y) GetMapSize()
        {
            int xSize = 0;
            int ySize = 0;
            int.TryParse(xSizeText.text, out xSize);
            int.TryParse(ySizeText.text, out ySize);
            return (xSize, ySize);
        }

        public (int x, int y) GetStart()
        {
            int x = 0;
            int y = 0;
            int.TryParse(xStart.text, out x);
            int.TryParse(yStart.text, out y);
            return (x, y);
        }

        public (int x, int y) GetDest()
        {
            int x = 0;
            int y = 0;
            int.TryParse(xDest.text, out x);
            int.TryParse(yDest.text, out y);
            return (x, y);
        }
    }
}