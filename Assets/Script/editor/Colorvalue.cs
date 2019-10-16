using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Colorvalue:EditorWindow
{
    //#shift &alt %Ctrl
    [MenuItem("Mytools/setpos #%&C")]
    static void AddWindow()
    {
        Rect wr = new Rect(0, 0, 300, 300);
        Colorvalue window = (Colorvalue)EditorWindow.GetWindowWithRect(typeof(Colorvalue), wr, true, "setpos");
        window.Show();
    }
    static string[] xyzvalue = new string[6];
    static Vector3 xyzstep=Vector3.one;
    static Vector3 startpos = Vector3.zero;
    static GameObject targetObject;
    static List<Transform> childObject = new List<Transform>();
    static void Getchild(Transform tragetObject)
    {
        foreach(Transform child in tragetObject)
        {
            if (child.tag == "boxs" && child.GetComponent<Transform>()!=null)
            {
                childObject.Add(child);
            }
            if (child.childCount > 0)
            {
                Getchild(child);
            }
        }
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(125, 200, 60, 30), "重新排列"))
        {
            startpos = new Vector3(float.Parse(xyzvalue[0]), float.Parse(xyzvalue[1]), float.Parse(xyzvalue[2]));
            xyzstep = new Vector3(float.Parse(xyzvalue[3]), float.Parse(xyzvalue[4]), float.Parse(xyzvalue[5]));
            targetObject = Selection.activeGameObject;//获得选中对象
            if (targetObject)
            {
                childObject.Clear();
                Getchild(targetObject.transform);

                foreach (Transform child in childObject)
                {
                    child.transform.position = startpos;
                    startpos += xyzstep;
                }
            }
            //Debug.Log("!!!");
        }
        GUI.Label(new Rect(62, 2, 60, 20), "x");
        GUI.Label(new Rect(122, 2, 60, 20), "y");
        GUI.Label(new Rect(184, 2, 60, 20), "z");
        GUI.Label(new Rect(2, 20, 60, 20), "startpos");
        xyzvalue[0] = GUI.TextField(new Rect(62, 20, 60, 20),xyzvalue[0]);
        xyzvalue[1]= GUI.TextField(new Rect(122, 20, 60, 20), xyzvalue[1]);
        xyzvalue[2] = GUI.TextField(new Rect(184, 20, 60, 20), xyzvalue[2]);
        GUI.Label(new Rect(62, 39, 60, 20), "x");
        GUI.Label(new Rect(122, 39, 60, 20), "y");
        GUI.Label(new Rect(184, 39, 60, 20), "z");
        GUI.Label(new Rect(2, 59, 60, 20), "step");
        xyzvalue[3] = GUI.TextField(new Rect(62, 59, 60, 20), xyzvalue[3]);
        xyzvalue[4] = GUI.TextField(new Rect(122, 59, 60, 20), xyzvalue[4]);
        xyzvalue[5] = GUI.TextField(new Rect(184, 59, 60, 20), xyzvalue[5]);
    }
    private void OnInspectorUpdate()
    {
        this.Repaint();
    }
}
