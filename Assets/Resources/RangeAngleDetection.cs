using UnityEngine;

[System.Serializable]
public class RangeAngleDetection
{

    public float range = 5f;
    public float angle = 90f;

    public RangeAngleDetection(float _range , float _angle)
    {
       range = _range;
       angle = _angle;
    }

    public bool IsAngleDetected(Vector3 origin, Vector3 target, Vector3 originForward)
    {
        Vector3 delta = target - origin;
        float distance = delta.magnitude;

        if (distance > range)
            return false;

        float targetAngle = Vector2.Angle(new Vector2(delta.x, delta.z), new Vector2(originForward.x, originForward.z));

        return targetAngle < (angle / 2f);
    }
}
