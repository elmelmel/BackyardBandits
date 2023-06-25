using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Animator")] 
		public Animator playerAnimator;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool pull;
		public bool rotateRight;
		public bool rotateLeft;


		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		/*
		 public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		*/

		public void OnPull(InputValue value)
		{
			PullInput(value.isPressed);
		}

		public void OnRotateRight(InputValue value)
		{
			RotateRightInput(value.isPressed);

		}
		public void OnRotateLeft(InputValue value)
		{
			RotateLeftInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		/*
		 public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		*/

		public void PullInput(bool newPullState)
		{
			pull = newPullState;
		}
		
		public void RotateRightInput(bool newRotateRightState)
		{
			rotateRight = newRotateRightState;
		}
		public void RotateLeftInput(bool newRotateLeftState)
		{
			rotateLeft = newRotateLeftState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}