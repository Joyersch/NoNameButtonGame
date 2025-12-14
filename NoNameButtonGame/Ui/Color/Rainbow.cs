namespace NoNameButtonGame.Ui.Color;

public sealed class Rainbow : AnimatedColor
{
    public Rainbow()
    {
        Color = new Microsoft.Xna .Framework.Color[768];
        int c = 0;
        for (int i = 0; i < 256; i++)
            Color[i + c * 256] = new Microsoft.Xna.Framework.Color(i, 255 - i, 255);

        c++;
        for (int i = 0; i < 256; i++)
            Color[i + c * 256] = new Microsoft.Xna.Framework.Color(255, i, 255 - i);

        c++;
        for (int i = 0; i < 256; i++)
            Color[i + c * 256] = new Microsoft.Xna.Framework.Color(255 - i, 255, i);
    }
}