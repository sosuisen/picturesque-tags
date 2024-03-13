namespace SvgGenerator;

using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using Svg;
using Svg.Transforms;

public enum HatchPatternType {
    DownwardDiagonal,
    DownwardDiagonalReverse,
    UpwardDiagonal,
    UpwardDiagonalThick,
    Horizontal,
    Vertical,
    Cross,
    DiagonalCross,
    Dots,
    PolkaDots,
    Checker,    
    Fill,
}

public class HatchPattern
{    
    public static SvgPatternServer GetSvgPatternServer(HatchPatternType type, Color color)
    {
        return type switch
        {
            HatchPatternType.DownwardDiagonal => DownwardDiagonalPattern(color),
            HatchPatternType.DownwardDiagonalReverse => DownwardDiagonalReversePattern(color),
            HatchPatternType.UpwardDiagonal => UpwardDiagonalPattern(color),
            HatchPatternType.UpwardDiagonalThick => UpwardDiagonalThickPattern(color),
            HatchPatternType.Horizontal => HorizontalPattern(color),
            HatchPatternType.Vertical => VerticalPattern(color),
            HatchPatternType.Cross => CrossPattern(color),
            HatchPatternType.DiagonalCross => DiagonalCrossPattern(color),
            HatchPatternType.Dots => DotsPattern(color),
            HatchPatternType.PolkaDots => PolkaDotsPattern(color),
            HatchPatternType.Checker => CheckerPattern(color),
            HatchPatternType.Fill => new SvgPatternServer(),
            _ => new SvgPatternServer(),
        };
    }

    private static SvgPatternServer DownwardDiagonalPattern(Color color)
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

    private static SvgPatternServer DownwardDiagonalReversePattern(Color color)
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
        pattern.Children.Add(new SvgLine
        {
            StartX = 2,
            StartY = 0,            
            EndX = 2,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 5,
        });
        pattern.Children.Add(new SvgLine
        {
            StartX = 8,
            StartY = 0,            
            EndX = 8,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 3,
        });
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
        pattern.Children.Add(new SvgLine
        {
            StartX = 5,
            StartY = 0,
            EndX = 5,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 2
        });
        pattern.PatternTransform = [new SvgRotate(45)];

        return pattern;
    }

    public static SvgPatternServer UpwardDiagonalThickPattern(Color color) {
        // 左下から右上への斜線
        var pattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 斜線を描くための線をパターンに追加
        pattern.Children.Add(new SvgLine
        {
            StartX = 5,
            StartY = 0,
            EndX = 5,
            EndY = 10,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 5
        });
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

    private static SvgPatternServer DotsPattern(Color color)
    {
        var pattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 7,
            Height = 7,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse,
        };
        pattern.Children.Add(new SvgLine
        {
            StartX = 1,
            StartY = 0,            
            EndX = 1,
            EndY = 2,
            Stroke = new SvgColourServer(color),
            StrokeWidth = 2,
        });
        return pattern;
    }

    private static SvgPatternServer PolkaDotsPattern(Color color)
    {
        var pattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 20,
            Height = 20,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse,
        };
        pattern.Children.Add(new SvgCircle
        {
            CenterX = 4,
            CenterY = 4,
            Radius = 4,
            Fill = new SvgColourServer(color),
        });
        pattern.Children.Add(new SvgCircle
        {
            CenterX = 14,
            CenterY = 14,
            Radius = 4,
            Fill = new SvgColourServer(color),
        });

        return pattern;
    }

    private static SvgPatternServer CheckerPattern(Color color)
    {
        var pattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 20,
            Height = 20,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse,
        };
        pattern.Children.Add(new SvgRectangle
        {
            X = 0,
            Y = 0,
            Width = 10,
            Height = 10,
            Fill = new SvgColourServer(color),
        });
        pattern.Children.Add(new SvgRectangle
        {
            X = 10,
            Y = 10,
            Width = 10,
            Height = 10,
            Fill = new SvgColourServer(color),
        });
        
        return pattern;
    }

}
