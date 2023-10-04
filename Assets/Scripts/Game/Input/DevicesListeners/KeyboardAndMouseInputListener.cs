using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input.DevicesListeners
{
    public class KeyboardAndMouseInputListener : IDeviceListener, IDisposable, InputActions.IPlayerActions
    {
        private readonly InputEventsHolder inputEventsHolder;
        
        private readonly List<InputActions> inputActions = new ();

        public KeyboardAndMouseInputListener()
        {
            inputEventsHolder = InputMediator.InputEventsHolder;
        }

        public void Setup(InputActions input, InputDevice device)
        {
            inputActions.Add(input);

            input.devices = new UnityEngine.InputSystem.Utilities.ReadOnlyArray<InputDevice>(
                new InputDevice[] { device });
            
            input.Player.SetCallbacks(this);
        }
        

        public void Dispose()
        {
            foreach (var inputAction in inputActions)
            {
                inputAction.Player.RemoveCallbacks(this);
            }
        }

        public string GetControlScheme()
        {
            return InputConstants.KeyboardAndMouseScheme;
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveInput(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            JumpInput(context.performed);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            InteractInput(context.performed);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inputEventsHolder.OnPauseClick();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            SprintInput(context.performed);
        }
        
        private void MoveInput(Vector2 newMoveDirection)
        {
            inputEventsHolder.Move = newMoveDirection;
        } 

        private void LookInput(Vector2 newLookDirection)
        {
            inputEventsHolder.Look = newLookDirection;
        }

        private void JumpInput(bool newJumpState)
        {
            inputEventsHolder.Jump = newJumpState;
        }

        private void SprintInput(bool newSprintState)
        {
            inputEventsHolder.Sprint = newSprintState;
        }
        
        private void InteractInput(bool interact)
        {
            inputEventsHolder.Interact = interact;
        }
    }
}