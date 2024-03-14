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

    public static Color ChangeBrightness(Color color, double brightnessFactor)
    {
        double hue, saturation, brightness;
        RGBToHSL(color, out hue, out saturation, out brightness);

        // 明度を調整
        brightness *= brightnessFactor;
        brightness = Math.Min(Math.Max(brightness, 0), 1); // 明度は0から1の範囲に収める

        return HSLToRGB(hue, saturation, brightness);
    }

    private static void RGBToHSL(Color color, out double hue, out double saturation, out double brightness)
    {
        double r = color.R / 255f;
        double g = color.G / 255f;
        double b = color.B / 255f;

        double max = Math.Max(r, Math.Max(g, b));
        double min = Math.Min(r, Math.Min(g, b));
        double delta = max - min;

        brightness = (max + min) / 2;

        if (delta == 0)
        {
            hue = 0;
            saturation = 0;
        }
        else
        {
            saturation = delta / (1 - Math.Abs(2 * brightness - 1));

            if (max == r)
            {
                hue = 60 * (((g - b) / delta) % 6);
            }
            else if (max == g)
            {
                hue = 60 * (((b - r) / delta) + 2);
            }
            else
            {
                hue = 60 * (((r - g) / delta) + 4);
            }
        }

        if (hue < 0)
        {
            hue += 360;
        }
    }

    private static Color HSLToRGB(double hue, double saturation, double brightness)
    {
        double c = (1 - Math.Abs(2 * brightness - 1)) * saturation;
        double x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
        double m = brightness - c / 2;

        double rPrime, gPrime, bPrime;

        if (hue >= 0 && hue < 60)
        {
            rPrime = c;
            gPrime = x;
            bPrime = 0;
        }
        else if (hue >= 60 && hue < 120)
        {
            rPrime = x;
            gPrime = c;
            bPrime = 0;
        }
        else if (hue >= 120 && hue < 180)
        {
            rPrime = 0;
            gPrime = c;
            bPrime = x;
        }
        else if (hue >= 180 && hue < 240)
        {
            rPrime = 0;
            gPrime = x;
            bPrime = c;
        }
        else if (hue >= 240 && hue < 300)
        {
            rPrime = x;
            gPrime = 0;
            bPrime = c;
        }
        else
        {
            rPrime = c;
            gPrime = 0;
            bPrime = x;
        }

        byte r = (byte)((rPrime + m) * 255);
        byte g = (byte)((gPrime + m) * 255);
        byte b = (byte)((bPrime + m) * 255);

        return Color.FromArgb(r, g, b);
    }
}