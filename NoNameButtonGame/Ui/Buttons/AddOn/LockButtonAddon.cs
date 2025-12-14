using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Collision;
using NoNameButtonGame.Helpers;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame.Ui.Buttons.AddOn;

public sealed class LockButtonAddon : ButtonAddon
{
    public bool IsLocked { get; private set; } = true;

    private readonly BasicText _basicText;
    private Microsoft.Xna.Framework.Color _savedButtonColor;

    public LockButtonAddon(IButton button, float scale = 1F) : base(button)
    {
        _basicText = new BasicText("[locklocked]", GetPosition(), scale);

        button.Enter += _ => InvokeEnter();
        button.Click += delegate
        {
            if (!IsLocked)
                InvokeClick();
        };
        button.Leave += _ => InvokeLeave();
        _savedButtonColor = button.GetColor()[0];
        UpdateText();
        Lock();
    }

    public override bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
        => Button.UpdateInteraction(gameTime, !IsLocked ? toCheck : new EmptyHitbox());

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _basicText.Update(gameTime);
        Button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        _basicText.Draw(spriteBatch);
    }

    public void Unlock()
    {
        IsLocked = false;
        UpdateText();
        Button.ChangeColor(new[] { _savedButtonColor });
    }

    public void Lock()
    {
        IsLocked = true;
        UpdateText();
        var color = ColorHelper.DarkenColor(_savedButtonColor, 0.87F);
        Button.ChangeColor(new[] { color });
    }

    private void UpdateText()
    {
        _basicText.ChangeText(IsLocked
            ? "[locklocked]"
            : "[lockunlocked]");
        _basicText.ChangeColor(IsLocked ? Microsoft.Xna.Framework.Color.Gray : Microsoft.Xna.Framework.Color.DarkGray);
    }

    public override void Move(Vector2 newPosition)
    {
        base.Move(newPosition);
        _basicText.Move(newPosition);
    }

    public override void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        base.ChangeColor(input);
        _savedButtonColor = input[0];
    }

    public override void SetScale(ScaleProvider provider)
    {
        base.SetScale(provider);
        _basicText.SetScale(provider);
    }
}