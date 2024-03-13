namespace PicturesqueTags;

using System.Drawing;
using System.Security.Cryptography;
using System.Text;

// https://color.adobe.com/ja/
// Use triad to get color
// triad01: #EBAE9B, #9BA9EB, #C0EB9B
// triad02: #EB9BB6, #9BDAEB, #EBE49A

public enum ColorName
{
    Orange,
    Blue,
    Green,
    Pink,
    Cyan,
    Yellow,
}

public class Colors
{
    public static Dictionary<ColorName, Color> Dict { get; } = new() {
            { PicturesqueTags.ColorName.Orange,
                ColorTranslator.FromHtml("#EBAE9B") },
            { PicturesqueTags.ColorName.Blue,
                ColorTranslator.FromHtml("#9BA9EB") },
            { PicturesqueTags.ColorName.Green,
                ColorTranslator.FromHtml("#C0EB9B") },
            { PicturesqueTags.ColorName.Pink,
                ColorTranslator.FromHtml("#EB9BB6") },
            { PicturesqueTags.ColorName.Cyan,
                ColorTranslator.FromHtml("#9BDAEB") },
            { PicturesqueTags.ColorName.Yellow,
                ColorTranslator.FromHtml("#EBE49A") },
        };
}