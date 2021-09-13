using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class SelectBuilding : MonoBehaviour
    {
        public Building farmPrefab;
        public Building minePrefab;
        public Building sawmillPrefab;
        public Building homePrefab;
        public Building cityHallPrefab;
        public Building bunkerPrefab;
        public Building barrackPrefab;
        public Building academyPrefab;
        public Building towerPrefab;
        public Building gatePrefab;
        public Building wallPrefab;
        public Building polePrefab;

        [HideInInspector]
        public Dictionary<Building.BuildingType, GameObject> prefabs;

        public static SelectBuilding singleton;

        private GameObject selectedBuilding;

        private Building.BuildingType buildingType;
        [SerializeField]private bool buildingIsSelected = false;
        private bool wallIsSelected = false;
        private bool removing = false;
        

        public GameObject getSelectedBuilding { get { return selectedBuilding; } }
        public bool isBuildingSelected { get { return buildingIsSelected; } }
        public bool isWallSelected { get { return wallIsSelected; } set { wallIsSelected = value; } }
        public bool isRemoving { get { return removing; } }

        [HideInInspector]public List<ButtonAction> selectActions = new List<ButtonAction>();
        public Sprite spriteStopBuilding;
        void Awake()
        {
            singleton = this;
        }

        private void Start()
        {
            prefabs = new Dictionary<Building.BuildingType, GameObject>();
            prefabs[Building.BuildingType.Farm] = farmPrefab.gameObject;
            prefabs[Building.BuildingType.Mine] = minePrefab.gameObject;
            prefabs[Building.BuildingType.Sawmill] = sawmillPrefab.gameObject;
            prefabs[Building.BuildingType.Home] = homePrefab.gameObject;
            prefabs[Building.BuildingType.CityHall] = cityHallPrefab.gameObject;
            prefabs[Building.BuildingType.Bunker] = bunkerPrefab.gameObject;
            prefabs[Building.BuildingType.Barrack] = barrackPrefab.gameObject;
            prefabs[Building.BuildingType.Academy] = academyPrefab.gameObject;
            prefabs[Building.BuildingType.Tower] = towerPrefab.gameObject;

            prefabs[Building.BuildingType.WallPole] = polePrefab.gameObject;
            prefabs[Building.BuildingType.Wall] = wallPrefab.gameObject;

            prefabs[Building.BuildingType.Gate] = gatePrefab.gameObject;

            selectActions.Add(new ButtonAction(SelectFarm, farmPrefab.data.spriteIcon, true, farmPrefab.name, farmPrefab.description, farmPrefab.cost));
            selectActions.Add(new ButtonAction(SelectMine, minePrefab.data.spriteIcon, true, minePrefab.name, minePrefab.description, minePrefab.cost));
            selectActions.Add(new ButtonAction(SelectSawmill, sawmillPrefab.data.spriteIcon, true, sawmillPrefab.name, sawmillPrefab.description, sawmillPrefab.cost));
            selectActions.Add(new ButtonAction(SelectHome, homePrefab.data.spriteIcon, true, homePrefab.name, homePrefab.description, homePrefab.cost));
            //selectActions.Add(new ButtonAction(SelectCityHall, cityHallPrefab.data.spriteIcon, true, cityHallPrefab.name, cityHallPrefab.description, cityHallPrefab.cost));
            selectActions.Add(new ButtonAction(SelectBunker, bunkerPrefab.data.spriteIcon, true, bunkerPrefab.name, bunkerPrefab.description, bunkerPrefab.cost));
            selectActions.Add(new ButtonAction(SelectBarrack, barrackPrefab.data.spriteIcon, true, barrackPrefab.name, barrackPrefab.description, barrackPrefab.cost));
            selectActions.Add(new ButtonAction(SelectAcademy, academyPrefab.data.spriteIcon, true, academyPrefab.name, academyPrefab.description, academyPrefab.cost));
            selectActions.Add(new ButtonAction(SelectGate, gatePrefab.data.spriteIcon, true, gatePrefab.name, gatePrefab.description, gatePrefab.cost));
            selectActions.Add(new ButtonAction(SelectWall, wallPrefab.data.spriteIcon, true, wallPrefab.name, wallPrefab.description, wallPrefab.cost));
        }

        private void StartPlacing() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GameManager.instance.ignoreRaycast);
			RaycastHit	hit;
            if (hits.Length != 0)
            {
                if (HitTerrain(hits, out hit))
                {
                    Vector3 buildingPosition = SnapToGrid(hit.point);
                    buildingPosition.y = 30;
                    PlaceBuilding.singleton.StartPlacing(buildingPosition);
                }
            }
        }


        public void SelectFarm()
        {
            selectedBuilding = farmPrefab.gameObject;
            buildingType = Building.BuildingType.Farm;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectMine()
        {
            selectedBuilding = minePrefab.gameObject;
            buildingType = Building.BuildingType.Mine;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectSawmill()
        {
            selectedBuilding = sawmillPrefab.gameObject;
            buildingType = Building.BuildingType.Sawmill;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        protected bool HitTerrain(RaycastHit[] hits, out RaycastHit hit)

    {
        
		foreach (RaycastHit el in hits)
		{
			if (el.transform.gameObject.CompareTag(Tags.Terrain))
			{
				hit = el;
				return true;
			}
		}
		hit = new RaycastHit();
		return false;
    }

    protected Vector3 SnapToGrid(Vector3 point)
    {
        Vector3 snapPoint = new Vector3();

        snapPoint.x = Mathf.FloorToInt(point.x / 1);
        snapPoint.y = 0.5f;
        snapPoint.z = Mathf.FloorToInt(point.z / 1);

        return snapPoint;
    }

        public void SelectHome()
        {
            selectedBuilding = homePrefab.gameObject;

            
            // GameObject buildin = (GameObject)Instantiate(SelectBuilding.singleton.getSelectedBuilding, buildingPosition, Quaternion.identity);
            buildingType = Building.BuildingType.Home;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectCityHall()
        {
            selectedBuilding = cityHallPrefab.gameObject;
            buildingType = Building.BuildingType.CityHall;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectBunker()
        {
            selectedBuilding = bunkerPrefab.gameObject;
            buildingType = Building.BuildingType.Bunker;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectBarrack()
        {
            selectedBuilding = barrackPrefab.gameObject;
            buildingType = Building.BuildingType.Barrack;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectAcademy()
        {
            selectedBuilding = academyPrefab.gameObject;
            buildingType = Building.BuildingType.Academy;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectTower()
        {
            selectedBuilding = towerPrefab.gameObject;
            buildingType = Building.BuildingType.Tower;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectGate()
        {
            selectedBuilding = gatePrefab.gameObject;
            buildingType = Building.BuildingType.Gate;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
            StartPlacing();
        }

        public void SelectWall()
        {
            wallIsSelected = true;
            buildingIsSelected = false;
            // StartPlacingWall();
        }

        public void StopBuilding()
        {
            buildingIsSelected = false;
            wallIsSelected = false;
            removing = false;
        }

        public void RemoveBuilding()
        {
            removing = true;
            buildingIsSelected = false;
            wallIsSelected = false;
        }
    }

}
