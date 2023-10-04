using UnityEngine.EventSystems;

namespace Game.Input
{
    public class InputConstants
    {
        public const int LeftMouseInputId = PointerInputModule.kMouseLeftId;
        public const int RightMouseInputId = PointerInputModule.kMouseRightId;
        public const int MiddleMouseInputId = PointerInputModule.kMouseMiddleId;
        
        public const string KeyboardAndMouseScheme = "KeyboardMouse";
        public const string GamepadScheme = "Gamepad";
        public const string TouchScheme = "Touch";
        public const string JoystickScheme = "Joystick";
        public const string XRScheme = "XR";
    }
}