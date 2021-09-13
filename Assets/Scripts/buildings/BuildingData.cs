/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	[System.Serializable]
	public class BuildingData {
		public int type;
        public int level;
		public float[] position;
        public float[] localScale;
        public float[] rotation;

        public BuildingData(Building building) {
			type = (int)building.type;
			position = new float[3];
            level = building.level;
            localScale = new float[3];
            rotation = new float[4];
            position[0] = building.transform.position.x;
			position[1] = building.transform.position.y;
			position[2] = building.transform.position.z;
            localScale[0] = building.transform.localScale.x;
            localScale[1] = building.transform.localScale.y;
            localScale[2] = building.transform.localScale.z;
            rotation[0] = building.transform.rotation.x;
            rotation[1] = building.transform.rotation.y;
            rotation[2] = building.transform.rotation.z;
            rotation[3] = building.transform.rotation.w;
        }
	}
}*/
