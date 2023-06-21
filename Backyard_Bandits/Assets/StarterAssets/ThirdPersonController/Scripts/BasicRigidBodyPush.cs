using System;
using System.Numerics;
using Cinemachine;
using StarterAssets;
using UnityEditor.Searcher;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
[RequireComponent(typeof(PlayerInput))]
#endif

public class BasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;
	public bool canPush;
	[Range(0.5f, 5f)] public float strength = 1.1f;
	
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	private PlayerInput _playerInput;
#endif
	private CharacterController _controller;
	private StarterAssetsInputs _input;
	[SerializeField] GameObject _player;
	private GameObject thingToPull;
	private bool _pull = false;

	private Vector3 previousPosition;

	private void Start()
	{
		previousPosition = transform.position;
		_controller = GetComponent<CharacterController>();
		_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (canPush && !hit.gameObject.CompareTag("Pull")) PushRigidBodies(hit);
	}

	
	private void OnTriggerStay(Collider other)
	{
		if (_input.pull && other.gameObject.CompareTag("Pull") && canPush)
		{
			Debug.Log("OnTriggerStay called");
			_pull = true;
			PullRigidBodies(other);
		}
	}
	
	private void OnTriggerExit(Collider other)
	{
		_pull = false;

	}

	private void PullRigidBodies(Collider hit)
	{
		Debug.Log("pulling");
		Rigidbody body = hit.attachedRigidbody;
		if (body == null || body.isKinematic || !_pull) return;

		// make sure we only pull desired layer(s)
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		body.isKinematic = false;
		BoxCollider bodyCollider = body.GetComponent<BoxCollider>();
		//CapsuleCollider playerCollider = GetComponent<CapsuleCollider>();
		Vector3 pullDir = new Vector3(transform.position.x - body.transform.position.x, 0.0f, transform.position.z - body.transform.position.z) - bodyCollider.size;
		
		
		
		// Calculate the direction from the player to the body
		Vector3 playerToBody = body.position - transform.position;

		// Calculate the movement direction of the player
		Vector3 movementDirection = transform.position - previousPosition;

		// Normalize the vectors for dot product calculation
		playerToBody.Normalize();
		movementDirection.Normalize();

		// Calculate the dot product to determine if the player is walking away
		float dotProduct = Vector3.Dot(playerToBody, movementDirection);

		if (dotProduct < 0)
		{
			// Player is walking away from the body object
			body.AddForce(pullDir * strength, ForceMode.Impulse);
		}

		// Update the previous position for the next frame
		previousPosition = transform.position;

	}
	
	private void PushRigidBodies(ControllerColliderHit hit)
	{
		// https://docs.unity3d.com/ScriptReference/CharacterController.OnControllerColliderHit.html

		// make sure we hit a non kinematic rigidbody
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) return;

		// make sure we only push desired layer(s)
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0) return;

		// We dont want to push objects below us
		if (hit.moveDirection.y < -0.3f) return;
		
		// Apply the push and take strength into account
		body.isKinematic = false;
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z).normalized;
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}