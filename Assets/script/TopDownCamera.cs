using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TopDownCamera : MonoBehaviour
{
    public Transform target; // the target the camera follows
    public Vector3 offset; // the offset between the camera and the target

    public float smoothSpeed = 0.125f; // the speed at which the camera moves to its target position
    private void Start()
    {
        target = GameObject.Find("Player").transform;

    }
    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset; // the desired position of the camera
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // the smoothed position of the camera
        transform.position = smoothedPosition; // set the camera's position to the smoothed position

        transform.LookAt(target); // rotate the camera to look at the target
    }
}
