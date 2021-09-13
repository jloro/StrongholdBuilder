using UnityEngine;
using UnityEngine.UI;

public class ResoursesDisplayer : MonoBehaviour
{
    [SerializeField] private Text _food;
    [SerializeField] private Text _wood;
    [SerializeField] private Text _stone;

    public void UpdateDisplay(ResourceCost cost)
    {
        _food.text = cost.food.ToString();
        _wood.text = cost.wood.ToString();
        _stone.text = cost.stone.ToString();
    }
}
