using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BuildingData")]

public class BuildingData : ScriptableObject
{
    public int woodCost;
    public int stoneCost;
    public int foodCost;
    public Sprite spriteDestroy;
    public Sprite spriteUpgrade;
    public Sprite spriteRepair;
    public Sprite spriteIcon;

    private void OnEnable()
    {        /*
        if (spriteDestroy == null)
            spriteDestroy = editorSpriteDestroy;
        if (spriteUpgrade == null)
            spriteUpgrade = editorSpriteUpgrade;
            */
    }
}
