using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VIP))]
public class VIPInspector : Editor
{
    private VIP vip;
    private void OnSceneGUI()
    {
        vip = target as VIP;
        Handles.color = Color.green;
        Handles.DrawLine(vip.transform.position, vip.currentDest);
        Handles.DrawWireDisc(vip.target.transform.position, new Vector3(0, 0, 1), vip.maxRadius);
        Handles.DrawWireDisc(vip.target.transform.position, new Vector3(0, 0, 1), vip.minRadius);
    }
}
