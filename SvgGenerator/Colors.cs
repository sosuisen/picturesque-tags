namespace SvgGenerator;

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
    public static Color GenerateColorFromText(string txt)
    {
        uint hash = 0;
        // GetHashCodeは.NET以外の環境と互換性がないため使わない
        // var hash = str.GetHashCode();
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(txt));
            // 16バイトのハッシュ値の先頭4バイトをintに変換
            hash = BitConverter.ToUInt32(hashBytes, 0);
        }
        var name = (ColorName)(hash % Enum.GetNames(typeof(ColorName)).Length);
        return Dict[name];
    }

    public static Color GenerateColorFromHash(uint hash) {
        var name = (ColorName)(hash % Enum.GetNames(typeof(ColorName)).Length);
        return Dict[name];
    }

    public static Dictionary<ColorName, Color> Dict { get; } = new() {
            { SvgGenerator.ColorName.Orange,
                ColorTranslator.FromHtml("#EBAE9B") },
            { SvgGenerator.ColorName.Blue,
                ColorTranslator.FromHtml("#9BA9EB") },
            { SvgGenerator.ColorName.Green,
                ColorTranslator.FromHtml("#C0EB9B") },
            { SvgGenerator.ColorName.Pink,
                ColorTranslator.FromHtml("#EB9BB6") },
            { SvgGenerator.ColorName.Cyan,
                ColorTranslator.FromHtml("#9BDAEB") },
            { SvgGenerator.ColorName.Yellow,
                ColorTranslator.FromHtml("#EBE49A") },
        };
}