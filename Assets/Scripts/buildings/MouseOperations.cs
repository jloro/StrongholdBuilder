using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class MouseOperations : MonoBehaviour
    {
        public static MouseOperations singleton;

        private void Awake() {
            singleton = this;
        }

        void Update()
        {
            if (SelectBuilding.singleton.isBuildingSelected) {
                PlaceBuilding.singleton.Place();
            } else if (SelectBuilding.singleton.isWallSelected)
            {
                PlaceWall.singleton.Place();
            }
        }

        

        



        

        
    }
}
