using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

    public Shop(Vector2 position, StorageData storage) : base(position, DefaultSize, DefaultTexture, DefaultMapping)
    {
        _storage = storage;
        var yOffset = DefaultSize / 2;
        _optionOne = new ShopOption(position + yOffset + new Vector2(32, 0) * 1 + new Vector2(128, 0) * 0, "Auto Beans",
            storage.Upgrade1, 500, 1.23F);
        _optionTwo = new ShopOption(position + yOffset + new Vector2(32, 0) * 2 + new Vector2(128, 0) * 1,
            "Jelly Beans",
            storage.Upgrade2, 10000, 2.34F);
        _optionThree = new ShopOption(position + yOffset + new Vector2(32, 0) * 3 + new Vector2(128, 0) * 2,
            "Magic Beans",
            storage.Upgrade3, 100000, 3.45F);
        _optionFour = new ShopOption(position + yOffset + new Vector2(32, 0) * 4 + new Vector2(128, 0) * 3,
            "Suspicious Beans",
            storage.Upgrade4, int.MaxValue, 4.56F);
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