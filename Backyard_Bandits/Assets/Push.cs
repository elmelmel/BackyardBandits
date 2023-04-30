using UnityEngine;

public class Push : MonoBehaviour
{
    private Rigidbody _rigidbody;

    void Start()
    {
        // Get the Rigidbody component attached to the box
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision collision)
    {
        Debug.Log("OnCollisionStay called");
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // Cache the forward vector of the player's transform to avoid repeated property access
            Vector3 playerForward = collision.transform.forward;

            // Calculate the direction the player is pushing the box
            Vector3 pushDirection = new Vector3(playerForward.x, playerForward.y, playerForward.z);

            // Push the box in the direction the player is pushing it
            _rigidbody.AddForce(pushDirection * 10f, ForceMode.Impulse);
        }
    }
}

