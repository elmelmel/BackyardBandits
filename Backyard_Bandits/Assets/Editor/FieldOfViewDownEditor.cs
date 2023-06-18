using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(FieldOfViewDown))]
public class FieldOfViewDownEditor : Editor
{

    private void OnSceneGUI()
    {
        FieldOfViewDown fov = (FieldOfViewDown)target;

        // Get the current rotation of the object
        Quaternion rotation = fov.transform.rotation;

        // Calculate the rotation in the XZ plane (yaw) based on the object's current rotation
        float yawRotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0).eulerAngles.y;

        // Calculate the rotation in the XY plane (pitch) based on the object's current rotation
        float pitchRotation = Quaternion.Euler(rotation.eulerAngles.x, 0, rotation.eulerAngles.z).eulerAngles.x;

        // Combine the pitch and yaw rotations
        Quaternion combinedRotation = Quaternion.Euler(pitchRotation, yawRotation, 0);

        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = combinedRotation * DirectionFromAngle(0, -fov.angle / 2);
        Vector3 viewAngle02 = combinedRotation * DirectionFromAngle(0, fov.angle / 2);

        Handles.color = Color.red;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
