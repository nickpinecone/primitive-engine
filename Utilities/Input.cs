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
    static Dictionary<MouseButton, bool> mouseStates = new();

    private static bool IsMouseButtonDown(MouseButton mouseButton)
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

        return buttonState == ButtonState.Pressed;
    }

    private static void UpdateMouse()
    {
        var mouseState = Mouse.GetState();

        foreach (var (button, isDown) in mouseStates)
        {
            mouseStates[button] = IsMouseButtonDown(button);
        }
    }

    public static bool IsMouseJustPressed(MouseButton mouseButton)
    {
        if (!mouseStates.ContainsKey(mouseButton))
        {
            mouseStates[mouseButton] = false;
        }

        return IsMouseButtonDown(mouseButton) && mouseStates[mouseButton] == false;
    }

    static Dictionary<Keys, bool> keyStates = new();

    private static void UpdateKeyboard()
    {
        var keyboardState = Keyboard.GetState();

        foreach (var (key, isDown) in keyStates)
        {
            keyStates[key] = keyboardState.IsKeyDown(key);
        }
    }

    public static bool IsKeyJustPressed(Keys key)
    {
        var keyboardState = Keyboard.GetState();

        if (!keyStates.ContainsKey(key))
        {
            keyStates[key] = false;
        }

        return keyboardState.IsKeyDown(key) && keyStates[key] == false;
    }

    public static void Update()
    {
        UpdateMouse();
        UpdateKeyboard();
    }
}