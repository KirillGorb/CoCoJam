using UnityEngine;

public static class MathVector
{
    public static Vector2 RotateVector(this Vector2 vector, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        return new Vector2(
            cos * vector.x - sin * vector.y,
            sin * vector.x + cos * vector.y
        );
    }

    public static Vector3 RotateVector(this Vector3 vector, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        return new Vector3(
            cos * vector.x - sin * vector.y,
            sin * vector.x + cos * vector.y,
            vector.z
        );
    }
}

public class AngleToCollider
{
    private ContactPoint2D contactPoint;
    private Vector2 normal;

    public Vector2 GetMoveVector(Collision2D collider, Vector2 speed, float maxAngle = 45)
    {
        contactPoint = collider.contacts[0];
        normal = contactPoint.normal;

        var angle = Vector2.SignedAngle(speed.normalized, normal);
        if (speed.x > 0)
            angle -= 90;
        else
            angle += 90;

        if (Mathf.Abs(angle) > maxAngle)
            angle = 0;

        return speed.RotateVector(angle);
    }
}