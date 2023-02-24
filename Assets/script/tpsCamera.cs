using System.Collections.Generic;
using UnityEngine;

public class tpsCamera : MonoBehaviour
{
   public Transform target; // The target to orbit around
    public float distance = 5.0f; // The distance from the target
    public float xSpeed = 120.0f; // The speed of horizontal rotation
    public float ySpeed = 120.0f; // The speed of vertical rotation
    public float yMinLimit = -20.0f; // The minimum vertical angle
    public float yMaxLimit = 80.0f; // The maximum vertical angle
    public float distanceMin = .5f; // The minimum distance from the target
    public float distanceMax = 15f; // The maximum distance from the target
    public float smoothTime = 0.2f; // The smoothness of camera movement
    public float zoomSpeed = 5.0f; // The speed of zooming
    public LayerMask collisionLayers; // The layers to check for collisions

    private float x = 0.0f; // The current horizontal angle
    private float y = 0.0f; // The current vertical angle
    private Vector3 targetPosition; // The target's position
    private Vector3 dollyDirection; // The direction from the camera to the target
    private Vector3 smoothVelocity; // The velocity of camera movement

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        dollyDirection = transform.localPosition.normalized;
        targetPosition = target.position;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Rotate the camera based on mouse input
            x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // Zoom the camera based on mouse input
            distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            distance = Mathf.Clamp(distance, distanceMin, distanceMax);

            // Calculate the desired camera position based on the target's position and the camera's direction and distance
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 desiredCameraPosition = targetPosition - (rotation * Vector3.forward * distance);

            RaycastHit hit;

            // Check for collisions between the desired camera position and the target's position
            if (Physics.Linecast(targetPosition, desiredCameraPosition, out hit, collisionLayers))
            {
                // If there is a collision, adjust the camera position to the hit point
                distance = Mathf.Clamp(hit.distance, distanceMin, distanceMax);
                desiredCameraPosition = targetPosition - (rotation * Vector3.forward * distance);
            }

            // Lerp the camera's position towards the desired position
            transform.position = Vector3.SmoothDamp(transform.position, desiredCameraPosition, ref smoothVelocity, smoothTime);

            // Rotate the camera to look at the target
            transform.LookAt(targetPosition);
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }
}