namespace SvgGenerator;
using Svg;
using Svg.Pathing;
using System.Drawing;

public class SvgGenerator(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;

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

    public string GenerateSvg()
    {

        var embeddedSvg = "";

        // https://github.com/svg-net/SVG
        // https://svg-net.github.io/SVG/api/Svg.html
        var svgDoc = new SvgDocument
        {
            Width = Width,
            Height = Height,
            ViewBox = new SvgViewBox(-Width / 2, -Height / 2, Width, Height),
        };

        var group = new SvgGroup();
        svgDoc.Children.Add(group);

        // 斜線パターンを作成
        // 左上から右下への斜線
        var downwardDiagonalPattern = new SvgPatternServer
        {
            ID = "pat" + Guid.NewGuid(), // 固有のIDが必須なので注意
            Width = 10,
            Height = 10,
            PatternUnits = SvgCoordinateUnits.UserSpaceOnUse
        };
        // 斜線を描くための線をパターンに追加
        var line = new SvgLine
        {
            StartX = 0,
            StartY = 0,
            EndX = 10,
            EndY = 10,
            // Stroke = new SvgColourServer(System.Drawing.Color.Black),
            Stroke = new SvgColourServer(GenerateRandomPastelColor()),
            Fill = new SvgColourServer(GenerateRandomPastelColor()),
            StrokeWidth = 3
        };
        downwardDiagonalPattern.Children.Add(line);
        // パターンを利用するにはSVGドキュメントへ追加する必要がある
        svgDoc.Children.Add(downwardDiagonalPattern);

        Random random = new Random();
        var bgRadius = random.Next(Width / 3, Width * 3 / 7);
        var centerX = (Width / 2 - bgRadius) / 2 - random.Next(0, (Width / 2 - bgRadius));
        var centerY = (Height / 2 - bgRadius) / 2 - random.Next(0, (Height / 2 - bgRadius));
        group.Children.Add(new SvgCircle
        {
            Radius = bgRadius,
            CenterX = centerX,
            CenterY = centerY,
            // Fill = new SvgColourServer(GenerateRandomPastelColor()),
            Fill = downwardDiagonalPattern
            // Stroke = new SvgColourServer(GenerateRandomPastelColor()),
            // StrokeWidth = 7
        });

        // 円の半径（r）
        // 円の中心のX座標（cx）
        // 円の中心のY座標（cy）
        // 円周上の任意の点から開始して、4つのBézier曲線セグメントを使用して円を描きます。
        // 円の各四半分のために、制御点は半径の約0.552284749831
        // （これは、円を近似するための正確な制御点を得るための値です）倍離れた位置に配置されます。
        var svgPath = new SvgPath();
        group.Children.Add(svgPath);
        svgPath.Stroke = new SvgColourServer(System.Drawing.Color.Black);
        svgPath.StrokeWidth = 5;
        svgPath.Fill = SvgPaintServer.None;

        float cx = 0;
        float cy = 0;
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

        using (var stream = new MemoryStream())
        {
            svgDoc.Write(stream);
            embeddedSvg = System.Text.Encoding.UTF8.GetString(stream.GetBuffer());
        }
        return embeddedSvg;
    }
}
