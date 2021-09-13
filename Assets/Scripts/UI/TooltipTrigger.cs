using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    public string text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        UiManager.instance.tooltip.Active(text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.instance.tooltip.Desactive();
    }
}
