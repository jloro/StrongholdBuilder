using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveUI))]
public class WaveUIDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        WaveUI waveUI = (WaveUI) target;
        if (GUILayout.Button("UnlockTip", GUILayout.Height(30)))
        {
            waveUI.UnlockTip();
        }
    }
}
