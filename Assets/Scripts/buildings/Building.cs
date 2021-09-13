using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SA
{
	public class Building : Targetable {

        [Header("Building Sound")]
        public AudioClip buildingSound;

        [Header("Building Settings")] 
        public ResourceCost cost;
        private ResourceCost repairCost = new ResourceCost(50, 0, 50);
        private ResourceCost upgradeCost = new ResourceCost(200, 200, 200);

        new public	Renderer	renderer;
		public enum BuildingType { Farm = 0, Mine = 1, WallPole = 2, Wall = 3, Sawmill = 4, Home = 5, CityHall = 6, Bunker = 7, Barrack = 8, Academy = 9, Tower = 10, Gate = 11 }

        public BuildingType type;

        private bool colWithPole = false;
        private bool _placed;
        public bool placed { get { return _placed; } protected set { _placed = value; } }

        public Debris debris;
        [SerializeField] private GameObject _wallDestroyEvent;
        [SerializeField] protected int _lvl;
        [SerializeField] private int _lvlMax;
        public int lvl { get { return _lvl; } }
        [SerializeField] private List<GameObject> _lvlMeshs;
        public BuildingData data;
        [SerializeField]protected cakeslice.Outline outline;
        private NavMeshObstacle _obstacle;

        int nbCol = 0;
        protected bool onSlot = false;
        //private bool collision = false;
        public int NbCol
        {
            set { nbCol = value; }
            get { return nbCol; }
        }

        public bool inCollision { get { return nbCol != 0; }}
        public bool onRightSlot { get { return onSlot; } }

        public bool inCollisionWithPole { get { return colWithPole; } set { colWithPole = value; } }
        #region UnityMethods

        virtual protected void OnEnable()
		{
            if (data != null)
            {
                AddAction(Destroy, data.spriteDestroy, true, "Destroy", "Destroy this building.");
                if (_lvlMax != 0)
                    AddAction(ImproveBuilding, data.spriteUpgrade, true, "Upgrade", "Upgrade this building.", upgradeCost);
                AddAction(Repair, data.spriteRepair, true, "Repair", "Repair this building.", repairCost);
            }
            _obstacle = GetComponent<NavMeshObstacle>();
        }

		override protected	void	Start()
		{
			base.Start();
            targetType = eType.building;
            _placed = false;
		}

        private void OnCollisionEnter(Collision col)
        {
            if (!col.gameObject.CompareTag(Tags.Terrain) && !col.gameObject.CompareTag(Tags.Wall))
            {
                nbCol++;
            }
            if (col.gameObject.name.Equals("Stone tower(Clone)") && gameObject.name.Equals("Stone tower(Clone)"))
            {
                colWithPole = true;
                // Destroy(col.gameObject);
                // GameStateManager.singleton.RemoveBuilding(col.gameObject.GetComponent<Building>());

            }
        }

        private void OnCollisionExit(Collision col)
        {
            if (!col.gameObject.CompareTag(Tags.Terrain) && !col.gameObject.CompareTag(Tags.Wall))
            {
                nbCol--;
            }
            if (col.gameObject.name.Equals("Stone tower(Clone)"))
            {
                colWithPole = false;
            }
        }

        #endregion

        #region PublicMethods

        virtual public void ActiveOutline(bool state) { outline.enabled = state; }

        virtual public void Destroy()
        {
            if (UiManager.instance.selectedObject == gameObject)
                UiManager.instance.ResetUI();
            if (_hp > 0 && (type == BuildingType.Farm || type == BuildingType.Mine || type == BuildingType.Sawmill))
                ResourceManager.instance.FirePeople(eJob.Worker, GetComponent<EarnRessources>().nbWorkers);
            Destroy(gameObject);
            GameStateManager.singleton.RemoveBuilding(this);
            AudioClip destroyBuilding = CommonSounds.inst.destroyBuilding;
			if (null != destroyBuilding) {
				AudioSource.PlayClipAtPoint(destroyBuilding, Camera.main.transform.position);
			}
        }

        virtual public void Destroy(float time)
        {
            if (UiManager.instance.selectedObject == gameObject)
                UiManager.instance.ResetUI();
            Destroy(gameObject, time);
            GameStateManager.singleton.RemoveBuilding(this);
        }

        virtual public void ImproveBuilding()
        {
            if (!upgradeCost.CanAfford())
                return;
            ResourceManager.instance.ConsumeResource(upgradeCost);
            if (_lvl < _lvlMax)
            {
                if (_lvlMeshs.Count != 0)
                {
                    _lvlMeshs[_lvl + 1].GetComponent<cakeslice.Outline>().enabled = _lvlMeshs[_lvl].GetComponent<cakeslice.Outline>().enabled;
                    _lvlMeshs[_lvl].SetActive(false);
                    _lvlMeshs[_lvl + 1].SetActive(true);
                    outline = _lvlMeshs[_lvl + 1].GetComponent<cakeslice.Outline>();
                }
                _lvl++;
                if (_lvl == _lvlMax)
                    RemoveAction(data.spriteUpgrade);
                UiManager.instance.RefreshUi();
            }

        }

        public override bool TakeDamage(int amount)
        {
            Broken(true);
            _hp -= amount;
            if (_hp <= 0)
            {
                isDead = true;
                _hp = 0;
                Die();
                return true;
            }
            return false;
        }

        public void Die()
        {
            Debris tmp = Instantiate(debris, transform.position, Quaternion.identity);
            tmp.Setup(GetComponent<BoxCollider>());
            if (type == BuildingType.Wall || type == BuildingType.WallPole)
                Instantiate(_wallDestroyEvent, transform.position, Quaternion.identity);
            this.Destroy();
        }

        public bool Place()
        {
            if (cost.CanAfford())
            {
                ResourceManager.instance.ConsumeResource(cost);
                _placed = true;
                if (null != buildingSound) {
                    CommonSounds.inst.audioSource.PlayOneShot(buildingSound, 0.7F);
                }
                    
                return true;
            }
            return false;
        }

        public void Broken(bool state)
        {
            List<Material> tmp = new List<Material>();
            renderer.GetMaterials(tmp);
            for (int i = 0; i < tmp.Count; i++)
            {
                if (state)
                {
                    MaterialPropertyBlock tmpBlock = new MaterialPropertyBlock();
                    float a = 1.0f - (float)_hp / (float)_hpMax;
                    tmpBlock.SetColor("_Color", new Color(Mathf.Lerp(tmp[i].color.r, 1, a), Mathf.Lerp(tmp[i].color.g, 0, a), Mathf.Lerp(tmp[i].color.b, 0, a), 1));
                    renderer.SetPropertyBlock(tmpBlock, i);
                }
                else
                    renderer.SetPropertyBlock(null, i);
            }

        }

        public void Repair()
        {            
            if (_hp < _hpMax && repairCost.CanAfford()) {
                ResourceManager.instance.ConsumeResource(repairCost);
                _hp = Mathf.Clamp(_hp + 10, 0, _hpMax);
            }       

            if (_hp == _hpMax)
                Broken(false);
            else
                Broken(true);

        }

        virtual public void Placed()
        {
            if (_obstacle)
                _obstacle.enabled = true;
        }
        #endregion

        #region PrivateMethods

        #endregion




        

		

		

		

        

        

    }
}
