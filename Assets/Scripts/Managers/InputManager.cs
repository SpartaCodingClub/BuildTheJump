using System;

public enum InputType
{
    Player,
    UI,
    UI_Building
}

public class InputManager
{
    public InputSystem_Actions System { get; private set; }

    public void Initialize()
    {
        System = new();
        System.Enable();
    }
}