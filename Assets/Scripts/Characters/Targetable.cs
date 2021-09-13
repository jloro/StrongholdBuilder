using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum eType
{
    building, 
    character
}
public abstract class Targetable : MonoBehaviour
{
    public eType targetType { get; protected set; }
	[Header("Targetable Settings")] 
    [SerializeField] protected int _hpMax;
    public int hpMax { get { return _hpMax; } }

    [Tooltip("Current health.")]
    [SerializeField]  protected int _hp;
    public int hp { get{ return _hp; } }
    public abstract bool TakeDamage(int amount);
    public bool isDead { get; protected set; }

    [HideInInspector] public List<ButtonAction> actions = new List<ButtonAction>();

    public string description;
    new public string name;

    virtual protected	void	Start()
	{
		isDead = false;
		_hp = _hpMax;
	}

    public void AddAction(UnityAction call, Sprite sprite, bool tooltip = false, string name = "", string desc = "", ResourceCost cost = new ResourceCost())
    {
        actions.Add(new ButtonAction(call, sprite, tooltip, name, desc, cost));
    }

    public void RemoveAction(Sprite sprite)
    {
        actions.Remove(actions.Find(i => i.sprite == sprite));
        UiManager.instance.RefreshUi();
    }
}
