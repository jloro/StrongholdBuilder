using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GameStateManager : MonoBehaviour
    {

        private List<Building> buildings = new List<Building>();

        public void AddBuilding(Building b) {
            buildings.Add(b);
        }

        public void RemoveBuilding(Building b) {
            buildings.Remove(b);
        }

        public void AddWall(List<Building> walls)
        {
            foreach(Building wall in walls)
            {
                AddBuilding(wall);
            }
        }

        public static GameStateManager singleton;

        void Awake()
        {
            singleton = this;
        }
        /*
        public void SaveGameState() {
			SaveGame.SaveBuildingState(buildings.ToArray());
		}

        public void RestoreGameState() {
            BuildingData[] buildingsData = SaveGame.LoadBuildingState();
            buildings.Clear();
            foreach (BuildingData savedBuilding in buildingsData)
            {
                InstantiateBuilding(savedBuilding);
            }

            void InstantiateBuilding(BuildingData savedBuilding)
            {
                GameObject prefab = SelectBuilding.singleton.prefabs[(Building.BuildingType)savedBuilding.type][savedBuilding.level];
                if (prefab != null)
                {
                    Vector3 position = new Vector3(savedBuilding.position[0], savedBuilding.position[1], savedBuilding.position[2]);
                    Vector3 localScale = new Vector3(savedBuilding.localScale[0], savedBuilding.localScale[1], savedBuilding.localScale[2]);
                    Quaternion rotation = new Quaternion(savedBuilding.rotation[0], savedBuilding.rotation[1], savedBuilding.rotation[2], savedBuilding.rotation[3]);
                    GameObject go = (GameObject)Instantiate(prefab, position, Quaternion.identity);
                    if ((Building.BuildingType)savedBuilding.type == Building.BuildingType.Wall || (Building.BuildingType)savedBuilding.type == Building.BuildingType.WallPole)
                    {
                        go.tag = Tags.Placed;
                    }
                    go.transform.localScale = localScale;
                    go.transform.rotation = rotation;
                    go.layer = 0;
                    Building building = go.GetComponent<Building>();

                    building.transform.position = position;
                    building.transform.localScale = localScale;
                    building.transform.rotation = rotation;
                    building.level = savedBuilding.level;
                    building.type = (Building.BuildingType)savedBuilding.type;
                    buildings.Add(building);
                } else
                {
                    Debug.Log("BUILDING TYPE NOT FOUND");
                }
            }

        }*/
    }
}

