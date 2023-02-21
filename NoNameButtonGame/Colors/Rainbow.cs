using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Colors;

internal class Rainbow : AnimatedColor
{
    protected override void Init()
    {
        Color = new Color[768];
        int c = 0;
        for (int i = 0; i < 256; i++)
        {
            Color[i + c * 256] = new Color(i, 255 - i, 255);
        }

        c++;
        for (int i = 0; i < 256; i++)
        {
            Color[i + c * 256] = new Color(255, i, 255 - i);
        }

        c++;
        for (int i = 0; i < 256; i++)
        {
            Color[i + c * 256] = new Color(255 - i, 255, i);
        }
    }
}