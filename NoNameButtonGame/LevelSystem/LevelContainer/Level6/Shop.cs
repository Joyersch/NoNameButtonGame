using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Shop : GameObject, IInteractable
{
    public new static Vector2 DefaultSize => Display.Display.Size / 2;

    private StorageData _storage;

    private ShopOption _optionOne;
    private ShopOption _optionTwo;
    private ShopOption _optionThree;
    private ShopOption _optionFour;

    public long BeanCount { get; private set; }

    public Shop(Vector2 position, Vector2 size, StorageData storage) : base(position, DefaultSize, DefaultTexture,
        DefaultMapping)
    {
        _storage = storage;
        var yOffset = DefaultSize / 2;
        _optionOne = new ShopOption(size.Y, "Auto Beans", storage.Upgrade1, 500, 1.23F);
        _optionOne.GetCalculator(position, size).OnX(0.20F).BySizeX(-0.5F).Move();
        _optionTwo = new ShopOption(size.Y, "Jelly Beans", storage.Upgrade2, 10000, 2.34F);
        _optionTwo.GetCalculator(position, size).OnX(0.40F).BySizeX(-0.5F).Move();
        _optionThree = new ShopOption(size.Y, "Magic Beans", storage.Upgrade3, 100000, 3.45F);
        _optionThree.GetCalculator(position, size).OnX(0.60F).BySizeX(-0.5F).Move();
        _optionFour = new ShopOption(size.Y, "Suspicious Beans", storage.Upgrade4, int.MaxValue, 4.56F);
        _optionFour.GetCalculator(position, size).OnX(0.80F).BySizeX(-0.5F).Move();
    }


    public void IncreaseBeanCount()
    {
    }

    public override void Update(GameTime gameTime)
    {
        _optionOne.Update(gameTime, BeanCount);
        _optionTwo.Update(gameTime, BeanCount);
        _optionThree.Update(gameTime, BeanCount);
        _optionFour.Update(gameTime, BeanCount);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _optionOne.UpdateInteraction(gameTime, toCheck);
        _optionTwo.UpdateInteraction(gameTime, toCheck);
        _optionThree.UpdateInteraction(gameTime, toCheck);
        _optionFour.UpdateInteraction(gameTime, toCheck);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _optionOne.Draw(spriteBatch);
        _optionTwo.Draw(spriteBatch);
        _optionThree.Draw(spriteBatch);
        _optionFour.Draw(spriteBatch);
    }
}