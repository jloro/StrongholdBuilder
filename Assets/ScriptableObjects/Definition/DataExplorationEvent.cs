using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Data/DataExplorationEvent")]
public class DataExplorationEvent : ScriptableObject, ISkillData
{
    public string title;
    public string description;
    public Sprite img;
    public int score;
    public eExploFunctions onSuccess;
    public eExploFunctions onFailure;
    public eExploEventType eventType;
    public string failDesciption;
    public string successDesciption;
}
