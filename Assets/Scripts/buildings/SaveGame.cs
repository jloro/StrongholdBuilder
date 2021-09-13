using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SA
{
	public static class SaveGame {
        
		static string path = Application.dataPath + "/save.txt";
        static bool restored = false;

		public static void SaveBuildingState(Building[] buildings) {
            BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Create);
			BuildingData[] d = new BuildingData[buildings.Length];
			int i = 0;
			foreach (Building building in buildings) {
				//BuildingData data = new BuildingData(building);
				//d[i] = data;
				i++;
				//formatter.Serialize(stream, data);
			}
			formatter.Serialize(stream, d);
			stream.Close();
		}

		public static BuildingData[] LoadBuildingState() {
            if (restored == false)
            {
                if (File.Exists(path))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    FileStream stream = new FileStream(path, FileMode.Open);
                    BuildingData[] data = (BuildingData[])formatter.Deserialize(stream);
                    restored = true;
                    return data;
                }
            }
			BuildingData[] buildings = new BuildingData[0];
			return buildings;
		}

	}
}

