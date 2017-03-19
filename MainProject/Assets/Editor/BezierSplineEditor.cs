using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierSpline))]
public class BezierSplineEditor : Editor {

    private BezierSpline spline;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private float lineWidth = 3f;
    private int lineSteps = 10;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        spline = (BezierSpline) target;

        // Reset button
        if (GUILayout.Button("Reset"))
        {
            Undo.RecordObject(spline, "Reset");
            spline.Reset();
            EditorUtility.SetDirty(spline);
        }

        // Add curve button
        if (GUILayout.Button("Add Curve"))
        {
            Undo.RecordObject(spline, "Add Curve");
            spline.AddCurve();
            EditorUtility.SetDirty(spline);
        }
    }

    private void OnSceneGUI()
    {
        spline = (BezierSpline) target;
        handleTransform = spline.transform;
        handleRotation = handleTransform.rotation;

        Vector3 p0 = ShowPoint(0), p1, p2, p3;
        for (var i = 1; i < spline.points.Length; i+=3)
        {
            p1 = ShowPoint(i);
            p2 = ShowPoint(i + 1);
            p3 = ShowPoint(i + 2);

            // Draw lines between points
            Handles.color = Color.grey;
            Handles.DrawLine(p0, p1);
            Handles.DrawLine(p1, p2);
            Handles.DrawLine(p2, p3);

            //Draw curve
            Handles.DrawBezier(p0, p3, p1, p2, Color.cyan, null, lineWidth);
            p0 = p3;
        }
        ShowTangents();
    }

    private void ShowTangents()
    {
        Handles.color = Color.green;
        var point = spline.GetPoint(0f);
        Handles.DrawLine(point, point + spline.GetVelocity(0f).normalized);
        for (var i = 1; i <= lineSteps; i++)
        {
            point = spline.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + spline.GetVelocity(i / (float)lineSteps).normalized);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(spline.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spline, "Move Point");
            EditorUtility.SetDirty(spline);
            spline.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}
