using System;
using System.Collections.Generic;
using Common.ServiceLocator;
using Game.Input.DevicesListeners;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace Game.Input
{
    public class InputDevicesManager : IInitializableService
    {
        private KeyboardAndMouseInputListener keyboardAndMouseInputListener;
        
        private readonly List<InputActions> inputActions = new ();

        private bool inputEnabled = true;

        public void Initialize()
        {
            keyboardAndMouseInputListener = new();
            
            InputSystem.onDeviceChange += OnDeviceChange;

            InitializeDeviceListeners();
            EnableInput(true);
        }
        

        public void Dispose()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }
        
        private void InitializeDeviceListeners()
        {
            foreach (var device in InputSystem.devices)
            {
                // if (string.Equals(device.name, "Mouse", StringComparison.OrdinalIgnoreCase))
                // {
                //     //temp solution
                //     //to fix double pair of mouse and keyboard
                //     continue;
                // }
                PairDeviceToPlayer(device);
            }
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange inputDeviceChange)
        {
            switch (inputDeviceChange)
            {
                case InputDeviceChange.Added:
                    var actions = PairDeviceToPlayer(device);
                    if (inputEnabled && actions != null)
                    {
                        actions.Player.Enable();
                    }
                    break;
                case InputDeviceChange.Removed:
                    break;
                case InputDeviceChange.Disconnected:
                    break;
                case InputDeviceChange.Reconnected:
                    break;
                case InputDeviceChange.Enabled:
                    break;
                case InputDeviceChange.Disabled:
                    break;
                case InputDeviceChange.UsageChanged:
                    break;
                case InputDeviceChange.ConfigurationChanged:
                    break;
                case InputDeviceChange.SoftReset:
                    break;
                case InputDeviceChange.HardReset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(inputDeviceChange), inputDeviceChange, null);
            }
        }
        
        private InputActions PairDeviceToPlayer(InputDevice device, string controlScheme = "")
        {
            // var deviceListenerFactory = GetDeviceListenerFactory(device);
            // if (deviceListenerFactory == null)
            // {
            //     return null;
            // }

            return PairDeviceToPlayer(device, keyboardAndMouseInputListener, controlScheme);
        }

        private InputActions PairDeviceToPlayer(InputDevice device, IDeviceListener deviceListener, string controlScheme = "")
        {            
            var presets = new InputActions();
            inputActions.Add(presets);

            var user = InputUser.PerformPairingWithDevice(device);
            user.AssociateActionsWithUser(presets);

            var scheme = string.IsNullOrEmpty(controlScheme)
                ? deviceListener.GetControlScheme()
                : controlScheme;
            user.ActivateControlScheme(scheme);

            UnityEngine.Debug.Log($"<color=green>Connected Player_{user.index} {scheme} to {device.name}, id {device.deviceId} </color>");
            deviceListener.Setup(presets, device);

            return presets;
        }
        
        private void EnableInput(bool enable)
        {
            if (enable)
            {
                for (var i = 0; i < inputActions.Count; i++)
                {
                    inputActions[i]?.Player.Enable();
                }

                inputEnabled = true;
            }
            else
            {
                inputEnabled = false;

                for (var i = 0; i < inputActions.Count; i++)
                {
                    inputActions[i]?.Player.Disable();
                }
            }
        }
    }
}