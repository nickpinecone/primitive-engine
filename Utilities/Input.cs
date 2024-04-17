using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TowerDefense;

public enum MouseButton
{
    Left,
    Right,
    Middle
}

public static class Input
{
    static Dictionary<MouseButton, ButtonState> mouseButtonStates = new() {
        {MouseButton.Left, ButtonState.Released},
        {MouseButton.Right, ButtonState.Released},
        {MouseButton.Middle, ButtonState.Released},
    };

    public static void UpdateMouse()
    {
        var mouseState = Mouse.GetState();

        mouseButtonStates[MouseButton.Left] = mouseState.LeftButton;
        mouseButtonStates[MouseButton.Right] = mouseState.RightButton;
        mouseButtonStates[MouseButton.Middle] = mouseState.MiddleButton;
    }

    public static bool IsMouseJustPressed(MouseButton mouseButton)
    {
        var mouseState = Mouse.GetState();
        ButtonState buttonState;

        switch (mouseButton)
        {
            case MouseButton.Left:
                buttonState = mouseState.LeftButton;
                break;
            case MouseButton.Right:
                buttonState = mouseState.RightButton;
                break;
            case MouseButton.Middle:
                buttonState = mouseState.MiddleButton;
                break;
            default:
                return false;
        }

        var previousState = mouseButtonStates[mouseButton];

        return buttonState == ButtonState.Pressed && previousState != ButtonState.Pressed;
    }
}