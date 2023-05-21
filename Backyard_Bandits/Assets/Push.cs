using System;
using TMPro;
using UnityEngine;

public class Push : MonoBehaviour
{
    
    public KeyCode pushPullKey;
    public float pushPullForce = 10f;
    private bool isTouching = false;
    private bool isPushingPulling = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the push/pull key is held down
        if (false)
        {
            isPushingPulling = isTouching;
        }
        else
        {
            isPushingPulling = false;
        }
    }

    private void FixedUpdate()
    {
        // Apply force to the object if the player is pushing/pulling and touching it
        if (isPushingPulling && isTouching)
        {
            // Get the direction of the push/pull based on player's input
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 pushPullDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

            // Apply the force to the object's rigidbody
            rb.AddForce(pushPullDirection * pushPullForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is touching the object
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Check if the player is no longer touching the object
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouching = false;
        }
    }
    
    
    /*
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

    private void OnCollisionExit(Collision collision)
    {
        _rigidbody.velocity = Vector3.zero; 
    }
    */
}

