using System.Collections;
using System.Collections.Generic;
using TowerSystem;
using UnityEngine;

namespace MapSystem
{
    public interface IParentable
    {
        public void OnTracking();
        public void OnEndTarcking();
        public void SetPlaceOnThis(IPlaceable placeable);
    }
}
