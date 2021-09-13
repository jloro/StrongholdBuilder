using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExploManager : MonoBehaviour
{
    public static ExploManager instance = null;
    public Sprite soldierIcon;
    public Sprite scientistIcon;
    public SpriteRenderer mainHall;
    public List<ExploTeam> teams { get; private set; }
    public ExploTeam selectedTeam = null;
    public ExplorationEvent selectedEvent { get; private set; }

    [SerializeField] private int _maxSize;
    [SerializeField] private Button[] _teamButton;
    [SerializeField] private GameObject _teamDisplayer;
    [SerializeField] private GameObject _teamIconPrefab;
    [SerializeField] private ExploEventDisplayer _eventDisplayer;
    [SerializeField] private ScoutDisplayer[] _scoutDisplayers;
    //[SerializeField] private Button[] _scoutButtons;

    private ResoursesDisplayer _resourcesDisplayer;
    private Dictionary<ExploTeam, ExplorationEvent> _teamExploDic;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }
    private void Start()
    {
        teams = new List<ExploTeam>();
        _teamExploDic = new Dictionary<ExploTeam, ExplorationEvent>();
        ButtonAssignment();
        _scoutDisplayers = _teamDisplayer.GetComponentsInChildren<ScoutDisplayer>();
        _resourcesDisplayer = _teamDisplayer.GetComponentInChildren<ResoursesDisplayer>();
        TeamSelection(-1);
        HideExploration();
    }
    public void ShowExploration()
    {
        GetComponentInChildren<GroupEnabler>()?.DisactivateGroup();
    }
    public void HideExploration()
    {
        GetComponentInChildren<GroupEnabler>()?.DisactivateGroup();
    }
    public void SelectDestination(ExplorationEvent exploEvent)
    {
        selectedEvent = exploEvent;
    }
    public void DisplayEvent(ExplorationEvent exploEvent)//temporary
    {
        _eventDisplayer.DisplayEvent(exploEvent);
    }
    public void GoToDestination(ExploTeam team, ExplorationEvent exploEvent)
    {
        if (team != null && exploEvent != null && teams.Contains(team))
            _teamExploDic[team] = exploEvent;
        //selectedEvent = exploEvent;
        //_eventDisplayer.DisplayEvent(selectedEvent.GetExploEvent());

    }
    public bool CreateTeam(int nbScientist, int nbSoldier)
    {
        return AddTeam(new ExploTeam(nbScientist, nbSoldier, Instantiate(_teamIconPrefab, mainHall.transform.position, Quaternion.identity)));
    }
    public bool AddTeam(ExploTeam team)
    {
        int teamSize = teams.Count;
        if ( teamSize < _maxSize)
        {
            teams.Add(team);
            _teamExploDic.Add(team, null);
            _teamButton[teamSize].gameObject.SetActive(true);
            ButtonAssignment();
            return true;
        }
        return false;
    }
    private void ResetButtonsColors()
    {
        for (int i = 0; i < _teamButton.Length; i++)
        {
            _teamButton[i].GetComponent<Image>().color = _teamButton[i].colors.normalColor;
        }
    }
    public void TeamSelection(int index)
    {
        if (index >= 0 && index < teams.Count)
        {

            //if (selectedTeam == teams[index])
            //{ selectedTeam = null; }
            //else
             selectedTeam = teams[index];
            ResetButtonsColors();
            _teamButton[index].GetComponent<Image>().color = _teamButton[index].colors.selectedColor;
            var ev = _teamExploDic[selectedTeam];
            
            if (ev != null)
            {
                DisplayEvent(ev);
            }
            /*Camera.main.
                selectedTeam.icon.transform;*/
        }
        else { selectedTeam = null; }
        
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        List<Scout> teamLst = null;
        int size = 0;
        if (selectedTeam != null)
        {
            teamLst = selectedTeam.team;
            size = teamLst.Count;
            _resourcesDisplayer.UpdateDisplay(selectedTeam.loot);
            Utils.ShowCanvasGroup(_teamDisplayer.GetComponent<CanvasGroup>());
        }
        else 
        { 
            _resourcesDisplayer.UpdateDisplay(new ResourceCost(0));
            Utils.HideCanvasGroup(_teamDisplayer.GetComponent<CanvasGroup>());
        }
        for (int i = 0; i < _scoutDisplayers.Length; ++i)
        {
            if (i < size)
                _scoutDisplayers[i].Display(teamLst[i]);
            else
                _scoutDisplayers[i].Display(null);
        }
    }
    /// <summary>
    /// Enable and disable the button depending on the actual number of teams
    /// </summary>
    private void ButtonAssignment()
    {
        int teamSize = teams.Count;
        for (int i = 0; i < _teamButton.Length; ++i)
        {
            _teamButton[i].GetComponent<Image>().color = Color.white;
            if (i < teamSize)
                { _teamButton[i].interactable = true;/*gameObject.SetActive(true);*/ }
            else
                { _teamButton[i].interactable = false;/*gameObject.SetActive(false);*/ }
        }
    }
    public void RemoveTeam(ExploTeam team)
    {
        if (selectedTeam == team) { selectedTeam = null; }
        teams.Remove(team);
        _teamExploDic.Remove(team);
        foreach(Scout scout in team.team)
        {
            if (scout.scoutType == eScoutType.soldier)
                ResourceManager.instance.FirePeople(eJob.Soldier);
            else
                ResourceManager.instance.FirePeople(eJob.Scientist);
        }
        Destroy(team.icon);
        selectedTeam = null;
        ButtonAssignment();
    }
}
