using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BezierCurve))]
public class BezierCurveEditor : Editor
{
    private BezierCurve curve;
    private Transform handleTransform;
    private Quaternion handleRotation;
    private float lineWidth = 3f;
    private int lineSteps = 10;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        curve = (BezierCurve)target;

        // Reset button
        if (GUILayout.Button("Reset"))
        {
            Undo.RecordObject(curve, "Reset");
            EditorUtility.SetDirty(curve);
            curve.Reset();
        }
    }

    private void OnSceneGUI()
    {
        curve = (BezierCurve)target;
        handleTransform = curve.transform;
        handleRotation = handleTransform.rotation;

        Vector3[] handlePoints = new Vector3[curve.points.Length];
        for (var i = 0; i < handlePoints.Length; i++)
        {
            handlePoints[i] = ShowPoint(i);
        }

        // Draw lines between points
        Handles.color = Color.grey;
        Handles.DrawAAPolyLine(lineWidth, handlePoints);

        //Draw curve and tangents
        Handles.DrawBezier(handlePoints[0], handlePoints[3], handlePoints[1], handlePoints[2], Color.cyan, null, lineWidth);
        ShowTangents();
    }

    private void ShowTangents()
    {
        Handles.color = Color.green;
        var point = curve.GetPoint(0f);
        Handles.DrawLine(point, point + curve.GetVelocity(0f).normalized);
        for (var i = 1; i <= lineSteps; i++)
        {
            point = curve.GetPoint(i / (float)lineSteps);
            Handles.DrawLine(point, point + curve.GetVelocity(i / (float)lineSteps).normalized);
        }
    }

    private Vector3 ShowPoint(int index)
    {
        Vector3 point = handleTransform.TransformPoint(curve.points[index]);
        EditorGUI.BeginChangeCheck();
        point = Handles.DoPositionHandle(point, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(curve, "Move Point");
            EditorUtility.SetDirty(curve);
            curve.points[index] = handleTransform.InverseTransformPoint(point);
        }
        return point;
    }
}
