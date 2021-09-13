using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SA;
using TMPro;
using System;

public class UiManager : MonoBehaviour
{
	[Header("Ressources")]
	[SerializeField] private TextMeshProUGUI _textWood;
	[SerializeField] private TextMeshProUGUI _textStone;
	[SerializeField] private TextMeshProUGUI _textFood;

	[Header("Populations")]
	[SerializeField] private TextMeshProUGUI _maxPopulation;
	[SerializeField] private TextMeshProUGUI _Workers;
	[SerializeField] private TextMeshProUGUI _Scientist;
	[SerializeField] private TextMeshProUGUI _Soldier;

	[Header("Description Zone")]
	[SerializeField] private TextMeshProUGUI _name;
	[SerializeField] private TextMeshProUGUI _description;
	[SerializeField] private TextMeshProUGUI _hpText;
	[SerializeField] private Slider _hpSlider;
	[SerializeField] private TextMeshProUGUI _zoneIcontext1Text;
	[SerializeField] private Image _zoneIconText1Icon;
	[SerializeField] private TextMeshProUGUI _workersOnBuildingText;
	[SerializeField] private Image _workersOnBuildingIcon;
	[SerializeField] private GameObject[] _upgradeStars;
	public RecruitmentDisplay recruitmenDisplays;

	[Header("Actions")]
	[SerializeField] private GameObject[] buttons;

	[Header("Misc")]
	[SerializeField] private GameObject mainMenuPanel;
	[SerializeField] private Button mainMenuButton;
	[SerializeField] private Button resumeButton;
    [HideInInspector] public bool inSkill;
    [SerializeField] private GameObject _skillTree;


	// [Header("Prefabs")]
	// [SerializeField] private GameObject _prefabActionBtn;

	[Header("---")]
	private GameObject _selectedObject;
	public GameObject selectedObject { get { return _selectedObject; } protected set { _selectedObject = value; } }
	[Tooltip("Has the selected building ressource ?")]
	[SerializeField] private bool _hasRessource;
	[Header("Multiple Selection")]
	[SerializeField] private Selection _selection;
	public bool isMultipleSelecUI;
	[SerializeField] private Sprite _spriteDef;
	[SerializeField] private Sprite _spriteAtt;
	[SerializeField] private TextMeshProUGUI _speed;
	public Tooltip tooltip;
	private delegate void TargetableUpdateFun(Targetable target);
	private TargetableUpdateFun onUpdate = null;
	private Targetable _targetToUpdate = null;

	static public UiManager instance;

	private void Start()
	{
		if (instance == null)
			instance = this;
	}

	private void OnEnable()
	{
		StartCoroutine(WaitList_coroutine());
		mainMenuPanel.SetActive(false);
        mainMenuButton.onClick.AddListener(delegate { GameTimer.ChangeTimeScale(0); });
        resumeButton.onClick.AddListener(delegate { GameTimer.ChangeTimeScale(1); });
    }

    private void Update()
	{
		processInputs();

			UpdateRessources(); // Should be a listener to an event in ResourceManager ?
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && !SelectBuilding.singleton.isBuildingSelected && !SelectBuilding.singleton.isWallSelected)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, 1000f, GameManager.instance.ignoreRaycast))
			{
				if (hit.transform.gameObject.CompareTag(Tags.Player))
				{
					if (_selectedObject && _selectedObject.GetComponent<Targetable>().targetType == eType.building)
						_selectedObject.GetComponent<Building>().ActiveOutline(false);
					_selectedObject = hit.transform.gameObject;
					if (_selectedObject.GetComponent<Targetable>().targetType == eType.building)
					{
						AudioClip buildingSound = _selectedObject.GetComponent<Building>().buildingSound;
						if (null != buildingSound)
						{
							CommonSounds.inst.audioSource.PlayOneShot(buildingSound, 0.7F);
						}
						_selectedObject.GetComponent<Building>().ActiveOutline(true);
					}
					RefreshUi();
				}
				else
				{
					if (_selectedObject && _selectedObject.GetComponent<Targetable>().targetType == eType.building)
						_selectedObject.GetComponent<Building>().ActiveOutline(false);
					_selectedObject = null;
					ResetUI();
				}
			}
		}
		else
		{
			onUpdate?.Invoke(_targetToUpdate);
		}
	}

	private void UpdateRessources()
	{
		_textWood.text = ResourceManager.instance.wood.ToString("0");
		_textStone.text = ResourceManager.instance.stone.ToString("0");
		_textFood.text = ResourceManager.instance.food.ToString("0");
		_maxPopulation.text = $"{ResourceManager.instance.population.ToString("0")} / {ResourceManager.instance.populationMax.ToString("0")}";
		_Workers.text = ResourceManager.instance.freeWorkers.ToString("0") + " / " + ResourceManager.instance.totalWorkers.ToString("0");
		_Scientist.text = ResourceManager.instance.freeScientists.ToString("0") + " / " + ResourceManager.instance.totalScientists.ToString("0");
		_Soldier.text = ResourceManager.instance.freeSoldiers.ToString("0") + " / " + ResourceManager.instance.totalSoldiers.ToString("0");
		if (_hasRessource)
		{
			_zoneIcontext1Text.text = _selectedObject.GetComponent<EarnRessources>().nbRessource.ToString();
			_workersOnBuildingText.text = _selectedObject.GetComponent<EarnRessources>().nbWorkers.ToString();
		}
		if (_selectedObject != null)//move out from this fct
		{
			_hpText.text = $"{_selectedObject.GetComponent<Targetable>().hp} / {_selectedObject.GetComponent<Targetable>().hpMax}";
			_hpSlider.maxValue = _selectedObject.GetComponent<Targetable>().hpMax;
			_hpSlider.value = _selectedObject.GetComponent<Targetable>().hp;
		}
		if (isMultipleSelecUI)
		{
			_name.text = $"total: {_selection.currentSelec.Count.ToString()}";
			_description.text = $"archer: {_selection.nbArcher}," +
				$"infantry: {_selection.nbInfantry}";
			_zoneIcontext1Text.text = _selection.CountBehavior(true).ToString();
			_workersOnBuildingText.text = _selection.CountBehavior(false).ToString();
		}
	}

	public void ResetUI()
	{
		_targetToUpdate = null;
		onUpdate = null;
		recruitmenDisplays.Hide();
		_selectedObject = null;
		_hasRessource = false;
		_zoneIconText1Icon.enabled = false;
		_zoneIcontext1Text.enabled = false;
		_workersOnBuildingText.enabled = false;
		_workersOnBuildingIcon.enabled = false;
		_name.enabled = false;
		_description.enabled = false;
		// _hpText.enabled = false;
		_hpSlider.gameObject.SetActive(false);
		isMultipleSelecUI = false;
		for (int i = 0; i < buttons.Length; i++)
		{
			if (i < SelectBuilding.singleton.selectActions.Count)
			{
				buttons[i].SetActive(true);
				buttons[i].GetComponent<Button>().onClick = SelectBuilding.singleton.selectActions[i].action;
				buttons[i].GetComponent<Image>().sprite = SelectBuilding.singleton.selectActions[i].sprite;
                if (SelectBuilding.singleton.selectActions[i].tooltip)
                {
                    buttons[i].GetComponent<TooltipTrigger>().enabled = true;
                    buttons[i].GetComponent<TooltipTrigger>().text = SelectBuilding.singleton.selectActions[i].text;
                }
                else
                    buttons[i].GetComponent<TooltipTrigger>().enabled = false;
            }
			else
			{
				buttons[i].SetActive(false);

			}
		}
		for (int i = 0; i < _upgradeStars.Length; i++)
			_upgradeStars[i].SetActive(false);
	}
	public void RefreshBarrackUi(Targetable target )
	{
		Barrack barrack = (Barrack)target;
		if (barrack.isSpawning)
		{
			_description.enabled = false;
			recruitmenDisplays.DisplayBarrack(barrack);
		}
		else
		{
			_description.enabled = true;
		}
	}
	public void RefreshUi()
	{
        if (_selectedObject == null)
        {
            return;
        }
		Targetable target = _selectedObject.GetComponent<Targetable>();

		_name.enabled = true;
		_description.enabled = true;
		/* hide recruitmen display */
		recruitmenDisplays.Hide();
		_targetToUpdate = null;
		onUpdate = null;

		// _hpText.enabled = true;
		_hpSlider.gameObject.SetActive(true);
		_name.text = target.name;
		_description.text = target.description;
		_hpText.text = $"{target.hp} / {target.hpMax}";
		_hpSlider.maxValue = target.hpMax;
		_hpSlider.value = target.hp;
		_hasRessource = _selectedObject.GetComponent<EarnRessources>() != null;
		if (target.targetType == eType.building)
		{
			try
			{
				Barrack barrack = barrack = (Barrack)target;
				RefreshBarrackUi(barrack);
				_targetToUpdate = barrack;
				onUpdate = RefreshBarrackUi;
			}
			catch //(InvalidCastException e)
			{ }
		}
		if (_hasRessource)
		{
			_zoneIconText1Icon.enabled = true;
			_zoneIcontext1Text.enabled = true;
			_workersOnBuildingText.enabled = true;
			_workersOnBuildingIcon.enabled = true;
			_zoneIconText1Icon.sprite = _selectedObject.GetComponent<EarnRessources>().iconRessource;
			_zoneIcontext1Text.text = _selectedObject.GetComponent<EarnRessources>().nbRessource.ToString();
			_workersOnBuildingText.text = _selectedObject.GetComponent<EarnRessources>().nbWorkers.ToString();
		}
		if (target.targetType == eType.character)
		{
			_zoneIconText1Icon.enabled = true;
			_zoneIcontext1Text.enabled = true;
			_workersOnBuildingIcon.enabled = true;
			_workersOnBuildingText.enabled = false;
			_zoneIconText1Icon.sprite = _selectedObject.GetComponent<Unit>().spriteIconAttack;
			_zoneIcontext1Text.text = _selectedObject.GetComponent<Unit>().damage.ToString();
			_workersOnBuildingIcon.sprite = _selectedObject.GetComponent<Unit>().ignoreEnemy ? _spriteDef : _spriteAtt;
		}
		else
		{
			_zoneIconText1Icon.enabled = false;
			_zoneIcontext1Text.enabled = false;
			_workersOnBuildingText.enabled = false;
			_workersOnBuildingIcon.enabled = false;
			int lvl = 1;

			for (int i = 0; i < _upgradeStars.Length; i++)
			{
				if (target.GetComponent<Building>().lvl >= lvl)
					_upgradeStars[i].SetActive(true);
				else
					_upgradeStars[i].SetActive(false);
				lvl++;
			}
		}
		for (int i = 0; i < buttons.Length; i++)
		{
			if (i < target.actions.Count)
			{
				buttons[i].SetActive(true);
				buttons[i].GetComponent<Button>().onClick = target.actions[i].action;
				buttons[i].GetComponent<Image>().sprite = target.actions[i].sprite;
                //Debug.Log(target.actions[i].tooltip);
                if (target.actions[i].tooltip)
                {
                    buttons[i].GetComponent<TooltipTrigger>().enabled = true;
                    buttons[i].GetComponent<TooltipTrigger>().text = target.actions[i].text;
                }
                else
                    buttons[i].GetComponent<TooltipTrigger>().enabled = false;

            }
			else
				buttons[i].SetActive(false);
		}
	}

	public void MultipleSelectionUI()
	{
		_zoneIconText1Icon.enabled = true;
		_zoneIcontext1Text.enabled = true;
		_workersOnBuildingText.enabled = true;
		_workersOnBuildingIcon.enabled = true;
		// _hpText.enabled = false;
		_hpSlider.gameObject.SetActive(false);
		if (_selectedObject && _selectedObject.GetComponent<Targetable>().targetType == eType.building)
			_selectedObject.GetComponent<Building>().ActiveOutline(false);
		isMultipleSelecUI = true;
		_name.enabled = true;
		_description.enabled = true;
		_name.text = $"total: {_selection.currentSelec.Count.ToString()}";
		_description.text = $"archer: {_selection.nbArcher}," +
			$"infantry: {_selection.nbInfantry}";
		for (int i = 0; i < buttons.Length; i++)
		{
			if (i < _selection.actions.Count)
			{
				buttons[i].SetActive(true);
				buttons[i].GetComponent<Button>().onClick = _selection.actions[i].action;
				buttons[i].GetComponent<Image>().sprite = _selection.actions[i].sprite;
				if (_selection.actions[i].tooltip)
                {
                    buttons[i].GetComponent<TooltipTrigger>().enabled = true;
                    buttons[i].GetComponent<TooltipTrigger>().text = _selection.actions[i].text;
                }
                else
                    buttons[i].GetComponent<TooltipTrigger>().enabled = false;
			}
			else
				buttons[i].SetActive(false);
		}
		_zoneIcontext1Text.text = _selection.CountBehavior(true).ToString();
		_zoneIconText1Icon.sprite = _spriteDef;
		_workersOnBuildingText.text = _selection.CountBehavior(false).ToString();
		_workersOnBuildingIcon.sprite = _spriteAtt;
	}

	private IEnumerator WaitList_coroutine()
	{
		while (!SelectBuilding.singleton || (SelectBuilding.singleton && SelectBuilding.singleton.selectActions.Count == 0))
			yield return new WaitForEndOfFrame();
		ResetUI();
	}

	public void UpdateSpeed()
	{
		_speed.text = "x" + GameTimer.timeScale.ToString();
	}

	private void processInputs()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (!inSkill)
            {
                if (mainMenuPanel.activeSelf)
                    resumeButton.onClick.Invoke();
                else
                    mainMenuButton.onClick.Invoke();
            }
            else
            {
                inSkill = false;
                _skillTree.SetActive(false);
            }
		}
	}

    public void SetInSkill(bool value)
    {
        inSkill = value;
    }
}
