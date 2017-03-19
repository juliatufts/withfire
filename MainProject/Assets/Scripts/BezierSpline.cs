using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierSpline : MonoBehaviour {

    public Vector3[] points;
    public int CurveCount
    {
        get { return (points.Length - 1) / 3; }
    }

    public Vector3 GetPoint(float t)
    {
        int i = 0;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int) t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(
            Bezier.GetPoint(points[i], points[i+1], points[i+2], points[i+3], t)
        );
    }

    public Vector3 GetVelocity(float t)
    {
        int i = 0;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }

        return transform.TransformPoint(
            Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }

    public void AddCurve()
    {
        Vector3 lastPoint = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        points[points.Length - 3] = lastPoint + 1f * Vector3.right;
        points[points.Length - 2] = lastPoint + 2f * Vector3.right;
        points[points.Length - 1] = lastPoint + 3f * Vector3.right;
    }

    public void Reset()
    {
        points = new Vector3[] { 0f * Vector3.right,
                                 1f * Vector3.right,
                                 2f * Vector3.right,
                                 3f * Vector3.right };
    }
}
