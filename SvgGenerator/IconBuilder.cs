namespace SvgGenerator;

using Svg;
using Svg.Pathing;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

public class IconBuilder
{
    public int Width { get; }
    public int Height { get; }
    private SvgDocument _svgDoc;
    private SvgGroup _bgGroup;
    private SvgGroup _fgGroup;



    public IconBuilder(int width, int height)
    {
        Width = width;
        Height = height;

        // https://github.com/svg-net/SVG
        // https://svg-net.github.io/SVG/api/Svg.html
        _svgDoc = new SvgDocument
        {
            Width = Width,
            Height = Height,
            ViewBox = new SvgViewBox(-Width / 2, -Height / 2, Width, Height),
        };

        _bgGroup = new SvgGroup();
        _svgDoc.Children.Add(_bgGroup);
        _fgGroup = new SvgGroup();
        _svgDoc.Children.Add(_fgGroup);
    }

    private Color GenerateRandomPastelColor()
    {
        Random random = new Random();
        // RGB成分を128〜255の範囲でランダムに選択（パステルカラーを生成するため）
        var a = 255;
        var r = random.Next(128, 256);
        var g = random.Next(128, 256);
        var b = random.Next(128, 256);

        return Color.FromArgb(a, r, g, b);
    }

    public IconBuilder generateBgFromText(string tag) {
        var mainColor = Color.Black;
        var subColor1 = Color.Black;
        var subColor2 = Color.Black;
        var hatchPattern = HatchPatternType.None;

        if (tag.Length == 0) return Default();

        var defaultOffset = Width / 10;
        if (tag.Length >= 5) {
            var char5_6 = tag.Substring(4, 2);
            byte[] hashBytes;
            using (MD5 md5 = MD5.Create())
            {
                hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(char5_6));
            }
            // 16バイトのハッシュ値の先頭4バイトをuintに変換
            var colorHash = BitConverter.ToUInt32(hashBytes, 0);
            subColor2 = Colors.GenerateColorFromHash(colorHash);
            var geomHash = BitConverter.ToUInt32(hashBytes, 4);

                        
            var radius = Width / 3;
            var centerX = Width - radius - defaultOffset;
            var centerY = Height - radius - defaultOffset;
            AddCircle(HatchPatternType.None, subColor2, radius, centerX, centerY);
        }

        if (tag.Length >= 3) {
            var char3_4 = tag.Substring(2, 2);
            subColor1 = Colors.GenerateColorFromText(char3_4);
            var radius = Width / 3;
            var centerX = radius + defaultOffset;
            var centerY = radius + defaultOffset;
            AddCircle(HatchPatternType.None, subColor1, radius, centerX, centerY);
        }

        if (tag.Length >= 3) {
            var char3 = tag.Substring(2, 1);
            hatchPattern = HatchPattern.GenerateHatchPatternFromText(char3);
        }

        if (tag.Length >= 1) {
            // tagの２文字目までを切り出し
            var char1_2 = tag.Substring(0, 2);
            mainColor = Colors.GenerateColorFromText(char1_2);            
        }
        return Default();
    }

    public IconBuilder addBackground(HatchPatternType type)
    {
        var col = GenerateRandomPastelColor();
        return addBackground(type, col);
    }

    public IconBuilder AddCircle(HatchPatternType hType, Color col, float radius, float centerX, float centerY)
    {
        // パターンを利用するにはSVGドキュメントへ追加する必要があるので注意
        // switch式。便利。
        var pattern = hType switch
        {
            HatchPatternType.DownwardDiagonal => HatchPattern.DownwardDiagonalPattern(col),
            HatchPatternType.UpwardDiagonal => HatchPattern.UpwardDiagonalPattern(col),
            HatchPatternType.Horizontal => HatchPattern.HorizontalPattern(col),
            HatchPatternType.Vertical => HatchPattern.VerticalPattern(col),
            HatchPatternType.Cross => HatchPattern.CrossPattern(col),
            HatchPatternType.DiagonalCross => HatchPattern.DiagonalCrossPattern(col),
            _ => new SvgPatternServer()
        };
        if (hType != HatchPatternType.None)
            _svgDoc.Children.Add(pattern);

        _bgGroup.Children.Add(new SvgCircle
        {
            Radius = radius,
            CenterX = centerX,
            CenterY = centerY,
            Fill = hType != HatchPatternType.None ? pattern : new SvgColourServer(col),
        });

        return this;
    }

    public IconBuilder addBackground(HatchPatternType type, Color col)
    {
        // パターンを利用するにはSVGドキュメントへ追加する必要がある
        // switch式。便利。
        var pattern = type switch
        {
            HatchPatternType.DownwardDiagonal => HatchPattern.DownwardDiagonalPattern(col),
            HatchPatternType.UpwardDiagonal => HatchPattern.UpwardDiagonalPattern(col),
            HatchPatternType.Horizontal => HatchPattern.HorizontalPattern(col),
            HatchPatternType.Vertical => HatchPattern.VerticalPattern(col),
            HatchPatternType.Cross => HatchPattern.CrossPattern(col),
            HatchPatternType.DiagonalCross => HatchPattern.DiagonalCrossPattern(col),
            _ => new SvgPatternServer()
        };
        if (type != HatchPatternType.None)
            _svgDoc.Children.Add(pattern);


        Random random = new Random();
        var bgRadius = random.Next(Width / 3, Width * 3 / 7);
        var centerX = (Width / 2 - bgRadius) / 2 - random.Next(0, (Width / 2 - bgRadius));
        var centerY = (Height / 2 - bgRadius) / 2 - random.Next(0, (Height / 2 - bgRadius));
        _bgGroup.Children.Add(new SvgCircle
        {
            Radius = bgRadius,
            CenterX = centerX,
            CenterY = centerY,
            // Fill = new SvgColourServer(GenerateRandomPastelColor()),
            Fill = type != HatchPatternType.None ? pattern : new SvgColourServer(col),
            // Stroke = new SvgColourServer(GenerateRandomPastelColor()),
            // StrokeWidth = 7
        });

        return this;
    }

    public IconBuilder addForeground()
    {

        // 円の半径（r）
        // 円の中心のX座標（cx）
        // 円の中心のY座標（cy）
        // 円周上の任意の点から開始して、4つのBézier曲線セグメントを使用して円を描きます。
        // 円の各四半分のために、制御点は半径の約0.552284749831
        // （これは、円を近似するための正確な制御点を得るための値です）倍離れた位置に配置されます。
        var svgPath = new SvgPath();
        _fgGroup.Children.Add(svgPath);
        svgPath.Stroke = new SvgColourServer(System.Drawing.Color.Black);
        svgPath.StrokeWidth = 5;
        svgPath.Fill = SvgPaintServer.None;

        Random random = new Random();

        float r = Width / 2 * random.Next(70, 90) / 100;
        // 中心基準点のゆらぎ
        float cx1 = random.Next(0, (int)(Width / 2 - r));
        float cy1 = random.Next(0, (int)(Width / 2 - r));
        float cx2 = random.Next(0, (int)(Width / 2 - r));
        float cy2 = random.Next(0, (int)(Width / 2 - r));
        float cx3 = random.Next(0, (int)(Width / 2 - r));
        float cy3 = random.Next(0, (int)(Width / 2 - r));
        float cx4 = random.Next(0, (int)(Width / 2 - r));
        float cy4 = random.Next(0, (int)(Width / 2 - r));

        // ハンドルの長さ
        float baseHandleLength = 0.552284749831f * r;
        float c1 = baseHandleLength * random.Next(70, 90) / 100;
        float c2 = baseHandleLength * random.Next(70, 90) / 100;
        float c3 = baseHandleLength * random.Next(70, 90) / 100;
        float c4 = baseHandleLength * random.Next(70, 90) / 100;
        float c5 = baseHandleLength * random.Next(70, 90) / 100;
        float c6 = baseHandleLength * random.Next(70, 90) / 100;
        float c7 = baseHandleLength * random.Next(70, 90) / 100;
        float c8 = baseHandleLength * random.Next(70, 90) / 100;

        // パスセグメントを追加（円を描くためのBézier曲線セグメント）
        svgPath.PathData = new SvgPathSegmentList {
new SvgMoveToSegment(false, new PointF(cx1, cy1 - r)),
new SvgCubicCurveSegment(false, new PointF(cx1 + c1, cy1 - r), new PointF(cx2 + r, cy2 - c2), new PointF(cx2 + r, cy2)),
new SvgCubicCurveSegment(false, new PointF(cx2 + r, cy2 + c3), new PointF(cx3 + c4, cy3 + r), new PointF(cx3, cy3 + r)),
new SvgCubicCurveSegment(false, new PointF(cx3 - c5, cy3 + r), new PointF(cx4 - r, cy4 + c6), new PointF(cx4 - r, cy4)),
new SvgCubicCurveSegment(false, new PointF(cx4 - r, cy4 - c7), new PointF(cx1 - c1, cy1 - r), new PointF(cx1, cy1 - r)),
new SvgClosePathSegment(false)
};
        return this;
    }

    public IconBuilder Default()
    {
        addBackground(HatchPatternType.None, Color.FromArgb(255, 128, 128, 128));
        addForeground();
        return this;
    }


    public string toSvg()
    {
        var embeddedSvg = "";
        using (var stream = new MemoryStream())
        {
            _svgDoc.Write(stream);
            embeddedSvg = System.Text.Encoding.UTF8.GetString(stream.GetBuffer());
        }
        return embeddedSvg;
    }
}
