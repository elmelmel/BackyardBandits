﻿using System;
using System.Numerics;
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
	public float threshold = 0.5f;
	private UnityEngine.Quaternion rotationOffset;
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
	private void Start()
	{
		// Calculate rotation offset to account for player's initial rotation
		rotationOffset = UnityEngine.Quaternion.Euler(0f, -91.956f, 0f);
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
		Vector3 pullDir = new Vector3(transform.position.x - body.transform.position.x, 0.0f, 0.0f) - bodyCollider.size;
		if(_input.move.y < 0) body.AddForce(pullDir * strength, ForceMode.Impulse);
		
		// Get player's movement input or velocity vector
		Vector3 movementInput = _input.move;/* Get your movement input or velocity vector here */

		// Get player's local forward direction with rotation offset
		Vector3 playerForward = rotationOffset * transform.forward;
		playerForward.y = 0f; // Ensure forward is aligned with the ground plane

		// Normalize the movement input or velocity vector
		movementInput.Normalize();

		// Calculate dot product
		float dotProduct = Vector3.Dot(playerForward.normalized, movementInput.normalized);

		// Check if movement is forward
		bool isMovingForward = dotProduct > threshold;

		if (isMovingForward)
		{
			Debug.Log("Player is moving forward");
		}
		else
		{
			Debug.Log("Player is not moving forward");
		}
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