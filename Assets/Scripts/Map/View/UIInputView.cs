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

        public (int x, int y) getMapSize()
        {
            int xSize = 0;
            int ySize = 0;
            int.TryParse(xSizeText.text, out xSize);
            int.TryParse(ySizeText.text, out ySize);
            return (xSize, ySize);
        }
    }
}