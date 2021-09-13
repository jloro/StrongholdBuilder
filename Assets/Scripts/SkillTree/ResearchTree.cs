using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchTree : SkillTree
{
    [SerializeField] private CanvasGroup _militaryPanel;
    [SerializeField] private CanvasGroup _productionPanel;
    [SerializeField] private CanvasGroup _uiGroup;
    [SerializeField] private UnitData infantryData;
    [SerializeField] private UnitData bowmanData;
    [SerializeField] private ResearchDisplayer _onGoingRsrchDisplayer;
    public LoadingBar loadingBar;
    protected override void Start()
    {
        base.Start();
        if (_uiGroup == null) { _uiGroup = GetComponent<CanvasGroup>(); }
    }
    public void ToggleMilitaryPanel()
    {
        Utils.ShowCanvasGroup(_militaryPanel);
        Utils.HideCanvasGroup(_productionPanel);
    }
    public void ToggleProductionPanel()
    {
        Utils.ShowCanvasGroup(_productionPanel);
        Utils.HideCanvasGroup(_militaryPanel);
    }
    public void IncreaseCapacitySawmill()
    {
        EarnRessources.IncreaseCapacity(eResources.Wood, 5);
    }
    public void IncreaseEfficiencySawmill()
    {
        EarnRessources.IncreaseEfficiency(eResources.Wood);
    }
    public void IncreaseEfficiencyMine()
    {
        EarnRessources.IncreaseEfficiency(eResources.Stone);
    }
    public void IncreaseEfficiencyFarm()
    {
        EarnRessources.IncreaseEfficiency(eResources.Food);
    }
    public void UnlockBowmen()
    {
        Barrack.EnableArcher();
    }
    public void UpgradeInfantryLp()
    {
        Unit.UpgradeLpUnit(eUnit.infantry, 5);
        infantryData.UpgradeHealth();
    }
    public void UpgradeBowmanLp()
    {
        bowmanData.UpgradeHealth();
    }
    public void UpgradeInfantryDmg()
    {
        Unit.UpgradeDamageUnit(eUnit.infantry, 1);
        infantryData.UpgradeDamage();
    }
    public void UpgradeBowmanDmg()
    {
        bowmanData.UpgradeDamage();
        Unit.UpgradeDamageUnit(eUnit.archer, 1);
    }
    public void FasterRecruitment()
    {
        Barrack.EnableFastRecruitment();
    }
    public void Show()
    {
        Utils.ShowCanvasGroup(_uiGroup);
    }
    public void Hide()
    {
        Utils.HideCanvasGroup(_uiGroup);
    }
    public void DisplayOnGoingResearch(DataResearch data)
    {
        _onGoingRsrchDisplayer.Display(data);
        _onGoingRsrchDisplayer.Show();
        loadingBar.CreateTimer(data.time);
    }
    public void StopOnGoingResearch()
    {
        loadingBar.CancelTimer();
        _onGoingRsrchDisplayer.Hide();
    }
}
