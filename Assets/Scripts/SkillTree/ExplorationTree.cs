using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationTree : SkillTree
{
    [SerializeField] private Color _successColor;
    [SerializeField] private Color _failColor;
    public Color succesColor { get { return _successColor; } }
    public Color failColor { get { return _failColor; } }
    public void Print5()
    {
        Debug.Log("print 5");
    }
    public void Print12()
    {
        Debug.Log("print 12");
    }
}
