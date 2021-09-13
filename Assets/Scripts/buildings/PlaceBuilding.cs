using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class PlaceBuilding : PlaceScript
    {
        public static PlaceBuilding singleton;
        private GameObject building;
        private bool placing = false;
        private Building buildingComponent;
        //private Color shaderColor;
        //private Color specularColor;
		private	List<Color>	shaderColor = new List<Color>();
		private	List<Color>	specularColor = new List<Color>();

        [SerializeField] private GameObject buildingFieldOfView;
        [SerializeField] private Vector3 sizeFieldOfView = new Vector3(300, 300, 0);

        private void Awake()
        {
			singleton = this;
        }

		override
        public void Place()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GameManager.instance.ignoreRaycast);
			RaycastHit	hit;

            // FOR DEBUG
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            // END FOR DEBUG

            if (hits.Length != 0)
            {
                if (HitTerrain(hits, out hit))
                {
                    Vector3 buildingPosition = SnapToGrid(hit.point);
                    buildingPosition.y = 30;
                    
                    if (Input.GetMouseButtonDown(0))// && !EventSystem.current.IsPointerOverGameObject() && placing)
                    {
                        if (EventSystem.current.IsPointerOverGameObject()) {
                        foreach(var go in raycastResults)
                        {
                            if (go.gameObject.CompareTag("button")) {
                                Destroy(building);
                                placing = false;
                                building = null;
                                buildingComponent = null;
                                return;
                            }
                        }
                        }
                        SetEnd(hit.point);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        //Debug.Log("CANCEL PLACE BUILDING");
                        SA.SelectBuilding.singleton.StopBuilding();
                        Destroy(building);

                        placing = false;
                        building = null;
                        buildingComponent = null;
                    }

                    else if (placing)
                    {
                        //Debug.Log("AD");
                        Adjust(buildingPosition);
                    }

                }
                else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                   // Debug.Log("DESR");
                    Destroy(building);
                    placing = false;
                    building = null;
                    buildingComponent = null;
                }
            }
        }

        public void StartPlacing(Vector3 buildingPosition)
        {
//            Debug.Log("START");
			shaderColor.Clear();
            building = (GameObject)Instantiate(SelectBuilding.singleton.getSelectedBuilding, buildingPosition, Quaternion.identity);
            
            placing = true;
            buildingComponent = building.GetComponent<Building>();
            Renderer rend = buildingComponent.renderer;

			List<Material> tmp = new List<Material>();
			rend.GetMaterials(tmp);
			foreach (Material mat in tmp)
			{
				shaderColor.Add(mat.color);
			}
            // ERROR HERE BELOW
            // specularColor = rend.material.GetColor("Specular");
        }

        override
        protected void SetStart(Vector3 buildingPosition)
        {
            Debug.Log("START");
			shaderColor.Clear();
            building = (GameObject)Instantiate(SelectBuilding.singleton.getSelectedBuilding, buildingPosition, Quaternion.identity);
            
            placing = true;
            buildingComponent = building.GetComponent<Building>();
            Renderer rend = buildingComponent.renderer;

			List<Material> tmp = new List<Material>();
			rend.GetMaterials(tmp);
			foreach (Material mat in tmp)
			{
				shaderColor.Add(mat.color);
			}
            // ERROR HERE BELOW
            // specularColor = rend.material.GetColor("Specular");
        }

        override
        protected void SetEnd(Vector3 point)
        {
            if (buildingComponent == null) {
                return;
            }
//            Debug.Log("END");
            SA.SelectBuilding.singleton.StopBuilding();
            if (buildingComponent.inCollision
                || (!buildingComponent.onRightSlot && (buildingComponent.type == Building.BuildingType.Farm || buildingComponent.type == Building.BuildingType.Mine || buildingComponent.type == Building.BuildingType.Sawmill))
                || !buildingComponent.cost.CanAfford())
            {
                Destroy(building);
            }
            else
            {
//                Debug.Log("place");
                buildingComponent.Place();
                Placed(point);
            }
            placing = false;
            building = null;
            buildingComponent = null;
        }

        override
        protected void Adjust(Vector3 buildingPosition)
        {
            Rotate(building);
            building.transform.position = buildingPosition;
            if ((buildingComponent.inCollision) 
                || (!buildingComponent.onRightSlot && (buildingComponent.type == Building.BuildingType.Farm || buildingComponent.type == Building.BuildingType.Mine || buildingComponent.type == Building.BuildingType.Sawmill))
                || !buildingComponent.cost.CanAfford())
            {
                WrongPlacement(building.GetComponent<Building>().renderer);
            }
            else
            {
                GoodPlacement(building.GetComponent<Building>().renderer);
            }
        }

        void Placed(Vector3 hitPoint)
        {
			List<Material> tmp = new List<Material>();
			buildingComponent.renderer.GetMaterials(tmp);
			for (int i = 0; i < tmp.Capacity; i++)
			{
				tmp[i].color = shaderColor[i];
			}
            //rend.material.shader = Shader.Find("Specular");
            //rend.material.SetColor("_SpecColor", specularColor);
            // to be able to get Raycast for Mouse Events
            building.layer = 0;
            buildingComponent.transform.position = hitPoint;
            buildingComponent.transform.localScale = building.transform.localScale;
            buildingComponent.transform.rotation = building.transform.rotation;

            if (buildingFieldOfView)
            {
                GameObject fov = Instantiate(buildingFieldOfView, building.transform);
                fov.transform.localScale = sizeFieldOfView;
            }

            GameStateManager.singleton.AddBuilding(buildingComponent);
            ObjectPlaced();
            buildingComponent.Placed();
        }
    }

}
