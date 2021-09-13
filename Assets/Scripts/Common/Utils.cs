using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void ShowCanvasGroup(CanvasGroup group)
    {
        group.alpha = 1f;
        group.blocksRaycasts = true;
    }

    public static void HideCanvasGroup(CanvasGroup group)
    {
        if (group)
        {
            group.alpha = 0f; //this makes everything transparent
            group.blocksRaycasts = false;
        }
    }
}
