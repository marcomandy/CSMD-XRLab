using UnityEngine;
public class AutomaticDoor : MonoBehaviour
{
    [Header("Door Panels")]
    public Transform leftDoor;
    public Transform rightDoor;
    [Header("Door Movement")]
    public float openDistance = 1.5f;
    public float speed = 2f;
    [Header("Door Light")]
    public Light doorLight;
    private Vector3 leftClosedPosition;
    private Vector3 rightClosedPosition;
    private Vector3 leftOpenPosition;
    private Vector3 rightOpenPosition;
    private bool doorOpen = false;
    
    void Start()
    {
// Store the closed positions
        leftClosedPosition = leftDoor.position;
        rightClosedPosition = rightDoor.position;
// Find the center between the two doors
        Vector3 doorCenter =
            (leftClosedPosition + rightClosedPosition) / 2f;
// Find the direction each door should move
        Vector3 leftDirection =
            (leftClosedPosition - doorCenter).normalized;
        Vector3 rightDirection =
            (rightClosedPosition - doorCenter).normalized;
// Calculate the open positions
        leftOpenPosition =
            leftClosedPosition + leftDirection * openDistance;
        rightOpenPosition =
            rightClosedPosition + rightDirection * openDistance;
// Start with the light GameObject inactive
        if (doorLight != null)
        {
            doorLight.gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if (doorOpen)
        {
// Open the doors
            leftDoor.position = Vector3.MoveTowards(
                leftDoor.position,
                leftOpenPosition,
                speed * Time.deltaTime
            );
            rightDoor.position = Vector3.MoveTowards(
                rightDoor.position,
                rightOpenPosition,
                speed * Time.deltaTime
            );
        }
        else
        {
// Close the doors
            leftDoor.position = Vector3.MoveTowards(
                leftDoor.position,
                leftClosedPosition,
                speed * Time.deltaTime
            );
            rightDoor.position = Vector3.MoveTowards(
                rightDoor.position,
                rightClosedPosition,
                speed * Time.deltaTime
            );
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            doorOpen = true;
            if (doorLight != null)
            {
                doorLight.gameObject.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other))
        {
            doorOpen = false;
            if (doorLight != null)
            {
                doorLight.gameObject.SetActive(false);
            }
        }
    }

    private bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player") ||
               other.transform.root.CompareTag("Player");
    }
}