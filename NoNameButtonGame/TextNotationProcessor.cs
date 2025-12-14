using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Ui.Color;
using NoNameButtonGame.Ui.Text;

namespace NoNameButtonGame;

public static class TextNotationProcessor
{
    public static BasicText Parse(string input, float scale)
    {
        string newText = input;
        float textScale = 1F;
        int headingLevel = 0;

        while (newText.StartsWith("#"))
        {
            headingLevel++;
            newText = newText[1..];
        }

        while (newText.StartsWith(" "))
        {
            newText = newText[1..];
        }

        if (headingLevel > 0)
        {
            textScale = 5 - headingLevel;
        }

        ColorBuilder builder = new ColorBuilder();
        Regex colorCodePattern = new Regex(@"\{#([0-9A-Fa-f]{6})\}");
        MatchCollection matches = colorCodePattern.Matches(newText);

        int colorIndex = 0;
        Color color = Color.White;
        string cleanText = string.Empty;
        foreach (Match match in matches)
        {
            builder.AddColor(color, match.Index - colorIndex);
            cleanText += newText[colorIndex..match.Index];
            color = ColorBuilder.FromString(match.Groups[1].Value);
            colorIndex = match.Index + match.Length;
        }

        builder.AddColor(color, newText.Length - colorIndex);
        cleanText += newText[colorIndex..newText.Length];

        var text = new  BasicText(cleanText, textScale * scale);
        text.ChangeColor(builder.GetColor());
        return text;
    }
}