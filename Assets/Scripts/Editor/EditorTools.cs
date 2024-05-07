using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using ZPackage;
using System.Linq;

public class EditorTools
{

    const string screenshotPath = "Screenshots";
    const string Undo_distribbute = "Undo_dist";
    const string Undo_snap = "Undo_snap";

    [MenuItem("Tools/DeletePlayerPrefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Tools/Screenshot")]
    public static void Screenshot()
    {
        CreateDirectory(screenshotPath);

        string fileName = Path.Combine(screenshotPath, PlayerSettings.productName +
            System.DateTime.Now.ToString("@yyyy-MM-dd_hh-mm-ss") + ".png");

        ScreenCapture.CaptureScreenshot(fileName, 1);
        Debug.Log("Captured screenshot at " + fileName);
    }
    [MenuItem("Tools/Group Transform/Distribute Between")]
    private static void Distribute()
    {

        List<Transform> transforms = new List<Transform>(Selection.transforms);

        if (transforms.Count < 3) return;

        transforms.Sort((a, b) => a.GetSiblingIndex().CompareTo(b.GetSiblingIndex()));

        Vector3 point0 = transforms[0].position.ChangeY(0);
        Vector3 point1 = transforms[transforms.Count - 1].position.ChangeY(0);

        int c = transforms.Count - 1;
        float space = (point1 - point0).magnitude / (float)c;
        Vector3 v = (point1 - point0).normalized * space;

        for (int i = 1; i < c; i++)
        {
            Undo.RecordObject(transforms[i], Undo_distribbute);
            transforms[i].position = transforms[i - 1].position + v;
        }

        Debug.Log("Distribution completed.");
    }

    [MenuItem("Tools/Group Transform/Snap 1m")]
    public static void SnapTheThings()
    {
        foreach (Transform obj in Selection.transforms)
        {
            Undo.RecordObject(obj, Undo_snap);
            obj.position = obj.position.Round();
        }
    }
    [MenuItem("Tools/Group Transform/Snap 0.25m")]
    public static void Snap025()
    {
        foreach (Transform obj in Selection.transforms)
        {
            Undo.RecordObject(obj, Undo_snap);
            obj.position = obj.position.Round(0.25f);
        }
    }
    [MenuItem("Edit/Reset Selected Objects Position")]
    static void ResetPosition()
    {
        var transforms = Selection.gameObjects.Select(go => go.transform).ToArray();
        var so = new SerializedObject(transforms);
        // you can Shift+Right Click on property names in the Inspector to see their paths
        so.FindProperty("m_LocalPosition").vector3Value = Vector3.zero;
        so.ApplyModifiedProperties();
    }

    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

}
