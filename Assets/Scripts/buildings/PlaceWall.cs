using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace SA
{
    public class PlaceWall : PlaceScript
    {
        public static PlaceWall singleton;

        public float maxWallSize = 4.0f;
        public float minWallSize = 1.0f;
        public Camera cam;

        [SerializeField]private Building poleBuildingComponent;
        [SerializeField]private Building wallBuildingComponent;
		private	List<Color>	poleShaderColor = new List<Color>();
		private	List<Color>	wallShaderColor = new List<Color>();
        private bool wrong;
        float currentTime = 0F;

        public bool placing;
        public bool placingPole;
		private	bool	externEndingPole = false;
        
        
        [SerializeField]private GameObject pole;
        [SerializeField]private GameObject endingPole;
        [SerializeField]private GameObject wall;
        List<Building> walls = new List<Building>();

        private void Awake()
        {
            singleton = this;
            placingPole = false;
        }

        override
        public void Place()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GameManager.instance.ignoreRaycast);
			RaycastHit	hit;
            Array.Sort(hits, delegate (RaycastHit first, RaycastHit second) {
                return (int)(first.distance - second.distance);
            });

            if (hits.Length != 0)
            {
                if (HitTerrain(hits, out hit))
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        Debug.Log("&&&&&&&&&&&     CANCEL WALL         &&&&&&&&&&&&&&&&");
                        placing = false;
                        placingPole = false;
                        Destroy(pole);
                        poleBuildingComponent = null;
                        pole = null;
                        SelectBuilding.singleton.isWallSelected = false;
                        Reset();
                    }
                   
                    else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        Building tmp = hits[0].transform.GetComponent<Building>();
                        if (tmp && tmp.GetComponent<Building>().type == Building.BuildingType.WallPole)
                            GenerateNewPole(tmp.gameObject);
						else if (hits.Length == 1 && hits[0].transform.gameObject.CompareTag(Tags.Terrain))
							GenerateNewPole(hit.point);
                    }
                    if (placingPole)
                    {
//                        Debug.Log("----> Placing pole");
                        AdjustPole(hit.point);
                        if (Input.GetMouseButtonUp(0))
                        {
                            placingPole = false;
                            if (poleBuildingComponent.inCollision)
                                Destroy(pole);
                            else
                                Placed(pole, poleBuildingComponent, poleShaderColor);
                            poleShaderColor.Clear();
                            poleBuildingComponent = null;
                            pole = null;
                        }
                    }

                    else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && placing)
                    {
                        SetEnd(hit.point);
                    }
                    else
                    {
                        if (placing)
                        {
                            Building tmp = hits[0].transform.GetComponent<Building>();
                            if (tmp && tmp.GetComponent<Building>().type == Building.BuildingType.WallPole && tmp.gameObject != pole && Vector3.Distance(pole.transform.position, tmp.transform.position) > minWallSize)
                            {
                                ChangeEndingPole(tmp.gameObject);
                                return;
                            }
                            Adjust(hit.point);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject() && placing)
                {
                    //Destroy(building);
                    placing = false;
                    Destroy(wall);
                    Destroy(endingPole);

                    Reset();

                }
            }
        }

        private void GenerateNewPoleAndWall(Vector3 polePosition)
        {
            pole = (GameObject)Instantiate(SelectBuilding.singleton.polePrefab.gameObject, polePosition, Quaternion.identity);
            pole.tag = Tags.Wall;
            wall = (GameObject)Instantiate(SelectBuilding.singleton.wallPrefab.gameObject, pole.transform.position, Quaternion.identity);
            wall.tag = Tags.Wall;

            poleBuildingComponent = pole.GetComponent<Building>();
            wallBuildingComponent = wall.GetComponent<Building>();
        }

        private void GenerateNewWall()
        {
            wall = (GameObject)Instantiate(SelectBuilding.singleton.wallPrefab.gameObject, pole.transform.position, Quaternion.identity);
            wall.tag = Tags.Wall;

            wallBuildingComponent = wall.GetComponent<Building>();
        }

        private void GenerateNewPole(GameObject startPole)
        {
            pole = startPole;
            pole.tag = Tags.Wall;

            poleBuildingComponent = pole.GetComponent<Building>();
            SetStart(new Vector3(0, 0, 0));
        }
        private void GenerateNewPole(Vector3 polePosition)
        {
//            Debug.Log("oui");
            pole = (GameObject)Instantiate(SelectBuilding.singleton.polePrefab.gameObject, polePosition, Quaternion.identity);
            pole.tag = Tags.Wall;

            poleBuildingComponent = pole.GetComponent<Building>();
            placingPole = true;
            List<Material> tmp = new List<Material>();
            poleBuildingComponent.renderer.GetMaterials(tmp);
            foreach (Material mat in tmp)
            {
                poleShaderColor.Add(mat.color);
            }
        }
		private	void	ChangeEndingPole(GameObject newEndingPole)
		{
			if (!externEndingPole)
				Destroy(endingPole);
			poleShaderColor.Clear();
			List<Material> tmp = new List<Material>();
			pole.GetComponent<Building>().renderer.GetMaterials(tmp);
			foreach (Material mat in tmp)
			{
				poleShaderColor.Add(mat.color);
			}
			endingPole = newEndingPole;
			endingPole.tag = Tags.Wall;
			endingPole.layer = 2;
            if (wall.GetComponent<SA.Building>().NbCol != 0)
                wall.GetComponent<SA.Building>().NbCol--;
            if (endingPole.GetComponent<SA.Building>().NbCol != 0)
                endingPole.GetComponent<SA.Building>().NbCol--;
            externEndingPole = true;
			poleBuildingComponent = endingPole.GetComponent<Building>();
		}
        override
        protected void SetStart(Vector3 point)
        {
//            Debug.Log("--------> SET START");
            placing = true;

			GenerateNewWall();
			List<Material> tmp = new List<Material>();
			wall.GetComponent<Building>().renderer.GetMaterials(tmp);
			foreach (Material mat in tmp)
			{
				wallShaderColor.Add(mat.color);
			}
			pole.GetComponent<Building>().renderer.GetMaterials(tmp);
			foreach (Material mat in tmp)
			{
				poleShaderColor.Add(mat.color);
			}
        }


        void Placed(GameObject prefab, Building buildingComponent, List<Color> shaderColor)
        {
            Renderer rend = prefab.GetComponent<Building>().renderer;

			List<Material> tmp = new List<Material>();
			rend.GetMaterials(tmp);
			for (int i = 0; i < tmp.Capacity; i++)
			{
				tmp[i].color = shaderColor[i];
			}
            // to be able to get Raycast for Mouse Events
            prefab.layer = 0;
            //b.Position = hitPoint;
            //b.Type = buildingType;
            //GridManager.singleton.addBuilding(b);
            prefab.tag = "Player";
            prefab.layer = 0;
            //walls.Add(prefab.GetComponent<Building>());
            buildingComponent.transform.position = prefab.transform.position;
            buildingComponent.transform.localScale = prefab.transform.localScale;
            buildingComponent.transform.rotation = prefab.transform.rotation;
            GameStateManager.singleton.AddBuilding(buildingComponent);
        }

        override
        protected void SetEnd(Vector3 point)
        {
            SA.SelectBuilding.singleton.StopBuilding();
            placing = false;
            if (wrong)
            {
                Destroy(wall);
                if (!externEndingPole)
                    Destroy(endingPole);
                Reset();
                return;
            }
			if (!externEndingPole)
				AdjustEndingPole(point);
            if (poleBuildingComponent.inCollision || wallBuildingComponent.inCollision)
            {
                Destroy(wall);
				if (!externEndingPole)
					Destroy(endingPole);
				else
				{
					Renderer rend = endingPole.GetComponent<Building>().renderer;

					List<Material> tmp = new List<Material>();
					rend.GetMaterials(tmp);
					for (int i = 0; i < tmp.Capacity; i++)
					{
						tmp[i].color = poleShaderColor[i];
					}
					endingPole.layer = 0;
					endingPole.tag = "Player";
				}
            }
            else
            {
                Placed(wall, wallBuildingComponent, wallShaderColor);
				if (!externEndingPole)
					Placed(endingPole, poleBuildingComponent, poleShaderColor);
                wall.tag = "Player";
                endingPole.tag = "Player";
                wall.layer = 0;
                endingPole.layer = 0;
                if (pole != null)
                {
                    pole.tag = "Player";
                    pole.layer = 0;
                }
            }
            GameStateManager.singleton.AddWall(walls);
            Reset();
            ObjectPlaced();
        }

        private void Reset()
        {
            pole = null;
            wall = null;
            endingPole = null;
			externEndingPole = false;
            wrong = false;
            walls.Clear();
			poleShaderColor.Clear();
			wallShaderColor.Clear();
        }

        override
        protected void Adjust(Vector3 hit)
        {
			if (!externEndingPole)
				AdjustEndingPole(hit);
            AdjustWall();
        }

        void AdjustPole(Vector3 hit)
        {
            pole.transform.position = hit;
            if (poleBuildingComponent.inCollision)
                WrongPlacement(poleBuildingComponent.renderer);
            else
                GoodPlacement(poleBuildingComponent.renderer);
        }
        void AdjustWall()
        {
            pole.transform.LookAt(endingPole.transform.position);

            float distance = Vector3.Distance(pole.transform.position, endingPole.transform.position);
            if (distance >= maxWallSize)
            {
                distance = maxWallSize;
            }

            bool clipPole = poleBuildingComponent.inCollisionWithPole;

            wall.transform.position = pole.transform.position + distance / 2 * pole.transform.forward;
            wall.transform.rotation = pole.transform.rotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, 0.9f * distance);

            if (!wrong && (poleBuildingComponent.inCollision || wallBuildingComponent.inCollision || distance < minWallSize))
            {
				if (!externEndingPole)
					WrongPlacement(endingPole.GetComponent<Building>().renderer);
				WrongPlacement(wall.GetComponent<Building>().renderer);
                wrong = true;
            }
            else if (wrong && !poleBuildingComponent.inCollision && !wallBuildingComponent.inCollision && distance >= minWallSize)
            {
				if (!externEndingPole)
					GoodPlacement(endingPole.GetComponent<Building>().renderer);
				GoodPlacement(wall.GetComponent<Building>().renderer);
                wrong = false;
            }
            

            if (distance >= maxWallSize && !poleBuildingComponent.inCollision && !wallBuildingComponent.inCollision)
            {
                Placed(pole, poleBuildingComponent, poleShaderColor);
                Placed(wall, wallBuildingComponent, wallShaderColor);

                GenerateNewPoleAndWall(pole.transform.position + distance * pole.transform.forward);
                Destroy(endingPole);
                endingPole = null;
                wallBuildingComponent = wall.GetComponent<Building>();

                wrong = true;
                if (clipPole)
                {
                    placing = false;
                    Destroy(wall);
                }
                
            }
        }

        void AdjustEndingPole(Vector3 point)
        {
            if (endingPole != null)
            {
                endingPole.transform.position = point;
            }
            else
            {
                endingPole = (GameObject)Instantiate(SelectBuilding.singleton.polePrefab.gameObject, point, Quaternion.identity);
                endingPole.tag = Tags.Wall;
				endingPole.layer = 2;
                poleBuildingComponent = endingPole.GetComponent<Building>();
            }
        }
       
    }

}
