using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MapSystem
{
    public class UIInputView : MonoBehaviour
    {
        [SerializeField] Text xSizeText;
        [SerializeField] Text ySizeText;

        [SerializeField] Text xStart;
        [SerializeField] Text yStart;

        [SerializeField] Text xDest;
        [SerializeField] Text yDest;

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