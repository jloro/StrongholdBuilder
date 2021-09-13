using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/DataResearch")]

public class DataResearch : ScriptableObject, ISkillData
{
    public string title;
    public string description;
    public ResourceCost cost;
    public Sprite img;
    public float time;
}
