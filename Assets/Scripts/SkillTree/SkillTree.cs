using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree : MonoBehaviour
{
    public SkillNode[] rootNodes;
    public bool isLock { get; private set; }
    public bool isLockable = true;
    [SerializeField] private ConfirmationPanel _confirmationPanel;
    public ConfirmationPanel confirmationPanel { get { return _confirmationPanel; } }
    public Color unlockedColor = new Color(1, 1, 1, 1);
    public Color lockedColor = new Color(0.75f, 0.75f, 0.75f, 255);
    public Color activatedColor = new Color(0.0f, 0.75f, 0.0f, 255);
    public SkillDisplayer dislayer;


    public float currentSpeed { get; protected set; }//the actual speed of activation of the childs skillnodes
    protected virtual void Start()
    {
        currentSpeed = 1.0f;
        Invoke("UnlockRoots", 0.1f);
    }
    public void Lock()
    {
        if (isLockable)
            isLock = true;
    }
    public void UnLock()
    {
        isLock = false;
    }
    public void UnlockRoots()
    {
        for (int i = 0; i < rootNodes.Length; ++i)
        {
            rootNodes[i].Unlock();
        }
    }
}
