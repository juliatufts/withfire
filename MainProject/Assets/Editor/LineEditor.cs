using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Line))]
public class LineEditor : Editor {

    private void OnSceneGUI()
    {
        Line line = (Line) target;
        Transform handleTransform = line.transform;
        Quaternion handleRotation = handleTransform.rotation;
        Vector3 p0 = handleTransform.TransformPoint(line.p0);   // transform to world space
        Vector3 p1 = handleTransform.TransformPoint(line.p1);

        Handles.color = Color.cyan;
        Handles.DrawLine(p0, p1);

        EditorGUI.BeginChangeCheck();
        p0 = Handles.DoPositionHandle(p0, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p0 = handleTransform.InverseTransformPoint(p0);
        }

        EditorGUI.BeginChangeCheck();
        p1 = Handles.DoPositionHandle(p1, handleRotation);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(line, "Move Point");
            EditorUtility.SetDirty(line);
            line.p1 = handleTransform.InverseTransformPoint(p1);
        }
    }
}
