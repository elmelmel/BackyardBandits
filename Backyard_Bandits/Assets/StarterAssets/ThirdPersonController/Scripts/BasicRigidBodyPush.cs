using System;
using System.Numerics;
using StarterAssets;
using UnityEngine;
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
	private bool isPushingPulling = false;
	private void Start()
	{
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
		if (canPush) PushRigidBodies(hit);
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
		
		// Calculate push direction from move direction, horizontal motion only
		//Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
		
		/*if(_input.pull)
		{
			
			thingToPull = body;
			
			Debug.Log("Pulling");
			//Vector3 pullDir = new Vector3(_input.move.x, 0.0f);
			Vector3 playerPosition = transform.position;
			Vector3 objectPosition = body.transform.position;
			Vector3 pullDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
			body.AddForce(-pullDir * strength, ForceMode.Impulse);
			
		}
		*/
		/*else
		{
			body.isKinematic = false;
			Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z).normalized;
			//body.transform.position += pushDir * Time.deltaTime * strength;
			body.AddForce(pushDir * strength, ForceMode.Impulse);
		}
		*/
		// Apply the push and take strength into account
		body.isKinematic = false;
		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z).normalized;
		//body.transform.position += pushDir * Time.deltaTime * strength;
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}