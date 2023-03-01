using UnityEngine;

public class Door : MonoBehaviour
{
    public float openDistance = 2.0f; // The distance at which the door will open
    public float closeDistance = 4.0f; // The distance at which the door will close
    public float smoothTime = 0.2f; // The smoothness of door movement
    public float openAngle = 90.0f; // The angle to rotate the door when opening
    public float closeAngle = 0.0f; // The angle to rotate the door when closing
    public bool locked = false; // Whether the door is locked or not
    public AudioClip openSound; // The sound to play when the door opens
    public AudioClip closeSound; // The sound to play when the door closes

    private Transform player; // The player's Transform component
    private bool isOpen = false; // Whether the door is currently open or closed
    private float targetAngle = 0.0f; // The angle the door is currently moving towards
    private float smoothVelocity; // The velocity of door movement
    private bool isPlayingAudio = false; // Whether the audio source is currently playing
    private AudioSource audioSource; // The AudioSource component to play door sounds
    public Transform door_base;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!locked && !isOpen && !isPlayingAudio)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                // Calculate the direction from the door to the other object
                Vector3 direction = other.transform.position - transform.position;

                // Calculate the angle between the direction and the door's forward direction
                float angle = Vector3.Angle(direction, transform.forward);

                // Determine the target angle based on the angle between the direction and the door's forward direction
                if (angle > 90.0f)
                {
                    targetAngle = openAngle;
                }
                else
                {
                    targetAngle = -openAngle;
                }

                isOpen = true;
                isPlayingAudio = true;
                audioSource.PlayOneShot(openSound);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!locked && isOpen && !isPlayingAudio)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                targetAngle = closeAngle;
                isOpen = false;
                isPlayingAudio = true;
                audioSource.PlayOneShot(closeSound);
            }
        }
    }

    void Update()
    {
        if (!locked && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > closeDistance && isOpen && !isPlayingAudio)
            {
                // Close the door when the player moves away
                targetAngle = closeAngle;
                isOpen = false;
                isPlayingAudio = true;
                audioSource.PlayOneShot(closeSound);
            }
        }

        // Smoothly rotate the door towards the target angle
        float currentAngle =
            Mathf.SmoothDampAngle(door_base.transform.localEulerAngles.y, targetAngle, ref smoothVelocity, smoothTime);
        door_base.transform.localEulerAngles =
            new Vector3(door_base.transform.localEulerAngles.x, currentAngle, door_base.transform.localEulerAngles.z);

        // Set isPlayingAudio back to false when the audio clip finishes playing
        if (isPlayingAudio && !audioSource.isPlaying)
        {
            isPlayingAudio = false;
        }
    }
}