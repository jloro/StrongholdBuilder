using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SA;
using cakeslice;

public class Selection : MonoBehaviour
{
	private	List<GameObject>	_currentSelec = new List<GameObject>();
    public List<GameObject> currentSelec { get { return _currentSelec; } }
	private	Vector3				_firstClick = new Vector3(0.0f, 0.0f, 0.0f);
	public	bool				dragging { get; protected set; }
	private	Renderer			_renderer;
	private	Collider			_collider;
	[SerializeField]private	Vector3	_centroid;
    public int nbArcher { get; protected set; }
    public int nbInfantry { get; protected set; }

    public List<ButtonAction> actions = new List<ButtonAction>();
    public Sprite spriteStop;
    public Sprite spriteIconBehaviorAtt;
    public Sprite spriteIconBehaviorDef;


    #region UnityMethods
    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        nbArcher = 0;
        nbInfantry = 0;
        actions.Add(new ButtonAction(StopAll, spriteStop, true, "Stop all", "Stop all the selected units"));
        actions.Add(new ButtonAction(BehaviorAttAll, spriteIconBehaviorAtt, true, "Behavior attack", "All the selected units will attack all ennemies"));
        actions.Add(new ButtonAction(BehaviorDefAll, spriteIconBehaviorDef, true, "Behavior defense", "The selected units will ignore ennemies until it arrives"));
        dragging = false;
    }

    private void LateUpdate()
    {
        if (_currentSelec.Count == 0 && UiManager.instance.isMultipleSelecUI)
            UiManager.instance.ResetUI();
        else if (_currentSelec.Count > 0 && dragging && !UiManager.instance.isMultipleSelecUI)
            UiManager.instance.MultipleSelectionUI();
    }

    // Update is called once per frame
    private void Update()
    {
        if ((EventSystem.current.IsPointerOverGameObject() || SelectBuilding.singleton.isBuildingSelected || SelectBuilding.singleton.isWallSelected) && !dragging)
            return;
        if ((Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0) && Input.GetMouseButton(0))
        {
            dragging = true;
            DragSelect();
        }
        else if (!dragging && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000f, GameManager.instance.ignoreRaycast))
            {
                if (hit.transform.gameObject.CompareTag("Enemy"))
                    Attack(hit);
                else if (Input.GetKey(KeyCode.LeftShift))
                    AddSelectOne(hit);
                else
                    SelectOne(hit);
            }
        }
        else if (!dragging && Input.GetMouseButtonUp(1))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                MoveInFormation();
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                IgnoreEnemy();
                Move();
            }
            else 
                Move();
        }

        if (dragging && Input.GetMouseButtonUp(0))
        {
            _firstClick = new Vector3(0.0f, 0.0f, 0.0f);
            _renderer.enabled = false;
            _collider.enabled = false;
            dragging = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log("Trigger enter");
        if (other.gameObject.CompareTag(Tags.Player) && other.gameObject.GetComponent<Unit>() != null)
        {
            ChangeUnitList(other.gameObject, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && other.gameObject.GetComponent<Unit>() != null)
        {
            ChangeUnitList(other.gameObject, false);
        }
    }
    #endregion

    #region PublicMethods

    public void StopAll()
    {
        foreach (GameObject go in _currentSelec)
        {
            if (go)
                go.GetComponent<Unit>().StopRunning();
        }
    }

    public void BehaviorAttAll()
    {
        foreach (GameObject go in _currentSelec)
        {
            if (go)
                go.GetComponent<Unit>().IgnoreEnemy(false);
        }
    }

    public void BehaviorDefAll()
    {
        foreach (GameObject go in _currentSelec)
        {
            if (go)
                go.GetComponent<Unit>().IgnoreEnemy(true);
        }
    }

    public int CountBehavior(bool def)
    {
        int attN = 0;
        int defN = 0;
        for (int i = 0; i < _currentSelec.Count; i++)
        {
            if (!def && !_currentSelec[i].GetComponent<Unit>().ignoreEnemy)
                attN++;
            else if (def && _currentSelec[i].GetComponent<Unit>().ignoreEnemy)
                defN++;
        }
        if (def)
            return defN;
        return attN;
    }
    #endregion

    #region PrivateMethods

    private void IgnoreEnemy()
    {
        foreach (GameObject go in _currentSelec)
        {
            if (go)
                go.GetComponent<Unit>().IgnoreEnemy(true);
        }
    }

    private void ClearList()
    {
        foreach (GameObject go in _currentSelec)
        {
            if (go)
                go.GetComponent<Unit>().outlineComponent.enabled = false;
        }
        _currentSelec.Clear();
        _centroid = new Vector3(0, 0, 0);
        nbArcher = 0;
        nbInfantry = 0;
    }

    public void ChangeUnitList(GameObject unit, bool addIt)
    {
        if (addIt)
        {
            if (unit.GetComponent<Unit>().unitType == eUnit.archer)
                nbArcher++;
            else
                nbInfantry++;
            _currentSelec.Add(unit);
        }
        else
        {
            if (unit.GetComponent<Unit>().unitType == eUnit.archer)
                nbArcher--;
            else
                nbInfantry--;
            _currentSelec.Remove(unit);
        }
        unit.GetComponent<Unit>().outlineComponent.enabled = addIt;
    }

    private void SelectOne(RaycastHit hit)
    {
        ClearList();
        UiManager.instance.isMultipleSelecUI = false;
        if (hit.transform.gameObject.CompareTag(Tags.Player) && hit.transform.gameObject.GetComponent<Unit>() != null)
        {
            dragging = false;
            ChangeUnitList(hit.transform.gameObject, true);
        }
    }

    private void AddSelectOne(RaycastHit hit)
    {
        if (hit.transform.gameObject.CompareTag(Tags.Player) && hit.transform.gameObject.GetComponent<Unit>() != null)
        {
            ChangeUnitList(hit.transform.gameObject, true);
        }
    }

    private void Attack(RaycastHit hit)
    {
        GameObject target = hit.transform.parent.gameObject;
        foreach (GameObject go in _currentSelec)
        {
            go.GetComponent<Unit>().FocusTarget(target.GetComponent<Character>());
        }
    }

    private void DragSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GameManager.instance.ignoreRaycast);
        RaycastHit hit = new RaycastHit();
        bool hitted = false;
        foreach (RaycastHit el in hits)
        {
            if (el.transform.gameObject.CompareTag(Tags.Terrain))
            {
                hitted = true;
                hit = el;
            }
        }
        if (!hitted)
            return;
        if (_firstClick == new Vector3(0.0f, 0.0f, 0.0f))
        {
            _firstClick = hit.point;
            transform.position = _firstClick;
        }
        if (!_renderer.enabled)
        {
            ClearList();
            _renderer.enabled = true;
            _collider.enabled = true;
        }

        transform.position = _firstClick + (hit.point - _firstClick) / 2.0f;
        transform.localScale = new Vector3(hit.point.x - _firstClick.x, transform.localScale.y, hit.point.z - _firstClick.z);
    }

    private void MoveInFormation()
    {
        _centroid = new Vector3(0, 0, 0);
        for (int i = 0; i < _currentSelec.Count; i++)
            _centroid += _currentSelec[i].transform.position;
        _centroid /= _currentSelec.Count;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GameManager.instance.ignoreRaycast);
        foreach (RaycastHit el in hits)
        {
            if (el.transform.gameObject.CompareTag(Tags.Terrain))
            {
                foreach (GameObject go in _currentSelec)
                {
                    if (go)
                    {
                        go.GetComponent<Unit>().RemoveTarget();
                        go.GetComponent<Unit>().Run(go.transform.position - _centroid + el.point);
                    }
                }
                return;
            }
        }
    }

    private void Move()
    {
        _centroid = new Vector3(0, 0, 0);
        for (int i = 0; i < _currentSelec.Count; i++)
            _centroid += _currentSelec[i].transform.position;
        _centroid /= _currentSelec.Count;
        float nbLine = Mathf.Ceil(_currentSelec.Count / Mathf.Ceil(Mathf.Sqrt(_currentSelec.Count)));
        int j = 0;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, GameManager.instance.ignoreRaycast);
        float offset = 5.0f;
        foreach (RaycastHit el in hits)
        {
            if (el.transform.gameObject.CompareTag(Tags.Terrain))
            {
                Vector3 dir = Vector3.Normalize(el.point - _centroid);
                Vector3 left = Vector3.Normalize(Vector3.Cross(dir, new Vector3(0.0f, 1.0f, 0.0f)));
                Vector3 first = el.point + left * Mathf.Ceil(_currentSelec.Count / nbLine / 2.0f) * offset + dir * nbLine / 2.0f * offset;
                Vector3 pos = first;
                foreach (GameObject go in _currentSelec)
                {
                    if (go)
                    {
                        go.GetComponent<Unit>().RemoveTarget();
                        go.GetComponent<Unit>().Run(pos);
                        if (!go.GetComponent<Unit>().ignoreEnemy)
                            go.GetComponent<Unit>().mainTarget = pos;
                        pos -= left * offset;
                        j++;
                        if (j % Mathf.Ceil(_currentSelec.Count / nbLine) == 0)
                        {
                            pos = first - dir * (j / nbLine) * offset;
                        }
                    }
                }
                return;
            }
        }
    }

    #endregion



}
