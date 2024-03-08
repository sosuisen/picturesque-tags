namespace SvgGenerator;

using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using Svg;
using Svg.Transforms;

public enum HatchPatternType {
    DownwardDiagonal,
    UpwardDiagonal,
    Horizontal,
    Vertical,
    Cross,
    DiagonalCross
}

public class HatchPattern
{    
    public static HatchPatternType GenerateHatchPatternFromText(string txt){
        uint hash = 0;
        // GetHashCodeは.NET以外の環境と互換性がないため使わない
        // var hash = str.GetHashCode();
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(txt));
            // ハッシュ値の先頭4バイトをintに変換
            hash = BitConverter.ToUInt32(hashBytes, 0);
        }
        return (HatchPatternType)(hash % 6);
    }

    public static SvgPatternServer DownwardDiagonalPattern(Color color)
    {
        // 左上から右下への斜線
        var pattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse,
        };
        // 斜線を描くための線をパターンに追加
        var line = new SvgLine
        {
            StartX = 5,
            StartY = 0,            
            EndX = 5,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3,
        };

        // 下のような斜線は、繰り返しの部分で必ず切れてしまう。
        // そのため、垂直線を45度回転させることで斜線パターンを作成する。
        /*
        var line = new SvgLine
        {
            StartX = 0,
            StartY = 0,            
            EndX = 10,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 1,
        };
        */
        pattern.Children.Add(line);
        pattern.PatternTransform = [new SvgRotate(-45)];

        return pattern;
    }

    public static SvgPatternServer UpwardDiagonalPattern(Color color)
    {
        // 左下から右上への斜線
        var pattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 斜線を描くための線をパターンに追加
        var line = new SvgLine
        {
            StartX = 5,
            StartY = 0,
            EndX = 5,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        pattern.Children.Add(line);
        pattern.PatternTransform = [new SvgRotate(45)];

        return pattern;
    }

    public static SvgPatternServer HorizontalPattern(Color color)
    {
        // 水平線
        var horizontalPattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 水平線を描くための線をパターンに追加
        var line = new SvgLine
        {
            StartX = 0,
            StartY = 5,
            EndX = 10,
            EndY = 5,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        horizontalPattern.Children.Add(line);

        return horizontalPattern;
    }

    public static SvgPatternServer VerticalPattern(Color color)
    {
        // 垂直線
        var verticalPattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 垂直線を描くための線をパターンに追加
        var line = new SvgLine
        {
            StartX = 5,
            StartY = 0,
            EndX = 5,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        verticalPattern.Children.Add(line);

        return verticalPattern;
    }

    public static SvgPatternServer CrossPattern(Color color)
    {
        // 十字
        var crossPattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 十字を描くための線をパターンに追加
        var line1 = new SvgLine
        {
            StartX = 0,
            StartY = 5,
            EndX = 10,
            EndY = 5,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        var line2 = new SvgLine
        {
            StartX = 5,
            StartY = 0,
            EndX = 5,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        crossPattern.Children.Add(line1);
        crossPattern.Children.Add(line2);

        return crossPattern;
    }

    public static SvgPatternServer DiagonalCrossPattern(Color color)
    {

        // 斜め十字
        var diagonalCrossPattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 斜め十字を描くための線をパターンに追加
        var line1 = new SvgLine
        {
            StartX = 0,
            StartY = 0,
            EndX = 10,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        var line2 = new SvgLine
        {
            StartX = 0,
            StartY = 10,
            EndX = 10,
            EndY = 0,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3
        };
        diagonalCrossPattern.Children.Add(line1);
        diagonalCrossPattern.Children.Add(line2);

        return diagonalCrossPattern;
    }
}
