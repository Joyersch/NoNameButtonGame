using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.Ui;

public sealed class MouseActionsMat : IMouseActions, IInteractable, IHitbox
{
    private readonly IHitbox _toCover;
    private readonly bool _sendSelfAsInvoker;
    private bool _hover;
    public bool IsHover => _hover;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;
    private static bool _wasPressed;

    public MouseActionsMat(IHitbox toCover, bool sendSelfAsInvoker = false)
    {
        _toCover = toCover;
        _sendSelfAsInvoker = sendSelfAsInvoker;
    }

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool @return = false;
        bool isMouseHovering = false;
        foreach (Rectangle rectangle in toCheck.Hitbox)
            if (_toCover.Hitbox.Any(h => h.Intersects(rectangle)))
                isMouseHovering = true;

        bool isPressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
        if (isMouseHovering)
        {
            if (!_hover)
                Enter?.Invoke(_sendSelfAsInvoker ? this : _toCover);

            if (!_wasPressed && isPressed)
            {
                Click?.Invoke(_sendSelfAsInvoker ? this : _toCover);
                @return = true;
                _wasPressed = true;
            }
        }
        else if (_hover)
            Leave?.Invoke(_sendSelfAsInvoker ? this : _toCover);

        _hover = isMouseHovering;

        return @return;
    }

    public Rectangle[] Hitbox => _toCover.Hitbox;

    public static void ResetState()
        => _wasPressed = LeftClicked;

    public static bool LeftClicked
        => Mouse.GetState().LeftButton == ButtonState.Pressed;
}