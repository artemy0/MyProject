using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathBez
{
    private static Vector2 p0, p1, p2;
    private static Quaternion p0Rot, p2Rot;
    private static float startTime;

    private static float progress;
    private static bool isMovinBez;

    public static void SetBezInitPoints(Transform startPoint, Transform сontrolPoint, Transform endPoint, float moveSpeed)
    {
        startTime = Time.time;

        //точки для расчёта движения по кривой Безье
        p0 = startPoint.position;
        p1 = сontrolPoint.position;
        p2 = endPoint.position;

        //кваторнионы для поворота игрока
        p0Rot = startPoint.rotation;
        p2Rot = endPoint.rotation;
    }

    public static Vector2 GetBezQuadPoint() //нахождение кривой безье
    {
        Vector2 q0 = Vector2.Lerp(p0, p1, progress);
        Vector2 q1 = Vector2.Lerp(p1, p2, progress);

        Vector2 b = Vector2.Lerp(q0, q1, progress);

        return b; //позиция игрока на кривой
    }

    public static Quaternion GetBezQuadQuaternion() //нахождения поворота объета на кривой безье
    {
        Quaternion r = Quaternion.Lerp(p0Rot, p2Rot, progress);

        return r;
    }

    private static IEnumerator TimeCounter(float moveSpeed)
    {
        progress = (Mathf.PI / 6) * moveSpeed * (Time.time - startTime);

        yield return null;
    }
}
