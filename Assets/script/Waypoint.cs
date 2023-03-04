using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public float waitTime = 2f;
    public string AnimationName;
    private void OnDrawGizmos()
    {
        // Draw a sphere at the position of the waypoint
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.1f);

        // Draw a line to indicate the rotation of the waypoint
        Gizmos.color = Color.yellow;
        Vector3 lineStart = transform.position;
        Vector3 lineEnd = lineStart + transform.forward * 0.5f;
        Gizmos.DrawLine(lineStart, lineEnd);

        // Draw an arrowhead to indicate the direction of the rotation
        Gizmos.color = Color.yellow;
        Vector3 arrowEnd = lineEnd + transform.forward * -0.5f + transform.right * 0.5f;
        Vector3 arrowStart = lineEnd + transform.forward * -0.5f - transform.right * 0.5f;
        Gizmos.DrawLine(lineEnd, arrowEnd);
        Gizmos.DrawLine(lineEnd, arrowStart);
    }

}