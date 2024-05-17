using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadraticProjectileCurve : MonoBehaviour
{
    private Transform pointA;
    public Transform PointA { get { return pointA; } set { pointA = value; }}
    private Transform pointB;
    public Transform PointB { get { return PointB; } set { pointB = value; }}

    private Transform heightTopPoint;
    public Transform HeightTopPoint { get { return heightTopPoint; } set {  heightTopPoint = value; }}
    public Vector3 Evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(pointA.position, heightTopPoint.position, t);
        Vector3 cb = Vector3.Lerp(heightTopPoint.position, pointB.position, t);

        return Vector3.Lerp(ac,cb, t);
    }

    private void OnDrawGizmos()
    {
        if (pointA == null || pointB == null || heightTopPoint == null) return;

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(Evaluate(i / 20f), 0.1f);
        }
    }
}
