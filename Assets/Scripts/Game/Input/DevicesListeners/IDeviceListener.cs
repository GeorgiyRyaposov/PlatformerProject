using UnityEngine.InputSystem;

namespace Game.Input.DevicesListeners
{
    public interface IDeviceListener
    {
        void Setup(InputActions input, InputDevice device);
        string GetControlScheme();
    }
}