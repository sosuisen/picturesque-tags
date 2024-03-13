namespace SvgGenerator;

using Svg;
using Svg.Pathing;
using Svg.Transforms;
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

    // 文字が表意文字 Ideogram であるかどうかを判定
    private bool IsIdeogram(char c)
    {
        // ひらがなの範囲
        var hiraganaRanges = new[] { (0x3040, 0x309F) };
        // カタカナの範囲
        var katakanaRanges = new[] { (0x30A0, 0x30FF), (0x31F0, 0x31FF) };
        // 漢字の範囲（日本語及び中国語共通）
        var kanjiRanges = new[] { (0x4E00, 0x9FFF), (0x3400, 0x4DBF), (0xF900, 0xFAFF) };

        // ひらがなか判定
        if (hiraganaRanges.Any(range => c >= range.Item1 && c <= range.Item2))
        {
            return false;
        }
        // カタカナか判定
        else if (katakanaRanges.Any(range => c >= range.Item1 && c <= range.Item2))
        {            
            return false;
            // カタカナは元の英語よりも短い数で意味を表しがちなので true にしておくのもよさそう？
            // return true;
        }
        // 漢字か判定
        else if (kanjiRanges.Any(range => c >= range.Item1 && c <= range.Item2))
        {
            return true;
        }
        // それ以外
        else
        {
            return false;
        }
    }

    public IconBuilder generateBgFromText(string tag)
    {
        char[] chars = tag.ToCharArray();
        if (chars.Length == 0) return Default();

        // tagを先頭から1文字ずつ読んで数を数える
        var charCount = 0;
        var block1 = "";
        var block2 = "";
        var block3 = "";
        var block4 = "";

        var tmpStr = "";
        for (int i = 0; i < chars.Length; i++)
        {
            var c = chars[i];
            if (IsIdeogram(c))
            {
                charCount += 2;
            }
            else
            {
                charCount += 1;
            }
            tmpStr += c;

            // if (block1 == "" && (charCount >= 4 || i == chars.Length - 1))
            if (block1 == "" && (charCount >= 3 || i == chars.Length - 1))
            {
                block1 = tmpStr;
                tmpStr = "";
                charCount = 0;
            }
            else if (block1 != "" && block2 == "" && (charCount >= 3 || i == chars.Length - 1))
            {
                block2 = tmpStr;
                tmpStr = "";
                charCount = 0;
            }
            else if (block2 != "" && block3 == "" && (charCount >= 3 || i == chars.Length - 1))
            {
                block3 = tmpStr;
                tmpStr = "";
                charCount = 0;
            }
            else if (block3 != "" && block4 == "" && i == chars.Length - 1)
            {
                block4 = tmpStr;
                tmpStr = "";
                charCount = 0;
            }
        }

        uint hash1, hash2, hash3, hash4;
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes1 = md5.ComputeHash(Encoding.UTF8.GetBytes(block1));
            byte[] hashBytes2 = md5.ComputeHash(Encoding.UTF8.GetBytes(block2));
            byte[] hashBytes3 = md5.ComputeHash(Encoding.UTF8.GetBytes(block3));
            byte[] hashBytes4 = md5.ComputeHash(Encoding.UTF8.GetBytes(block4));
            // 16バイトのハッシュ値の先頭4バイトをuintに変換
            hash1 = BitConverter.ToUInt32(hashBytes1, 0);
            hash2 = BitConverter.ToUInt32(hashBytes2, 0);
            hash3 = BitConverter.ToUInt32(hashBytes3, 0);
            hash4 = BitConverter.ToUInt32(hashBytes4, 0);
        }


        // ColorName x ShapeType x GeometryType種類の組み合わせパターンについて
        // hash1の値をもとに組に含まれる値を決定
        // hash1の値をColorName x ShapeType x GeometryTypeの組み合わせ数に丸める。
        // patterns1は0からColorName x ShapeType x GeometryType-1の値を取る
        var index1 = hash1 % (Enum.GetNames(typeof(ColorName)).Length
            * Enum.GetNames(typeof(ShapeType)).Length
            * Enum.GetNames(typeof(GeometryType)).Length);

        // 商はColorNameのインデックスになる。uint型のhashに対応するにはlongが必要
        // remainder: 余り
        long mainColorNameIndex = Math.DivRem(index1,
            Enum.GetNames(typeof(ShapeType)).Length
            * Enum.GetNames(typeof(GeometryType)).Length
             , out long remainder);

        // 商はShapeTypeのインデックスになる。余りはGeometryTypeのインデックスになる。
        long mainShapeTypeIndex = Math.DivRem(remainder,
            Enum.GetNames(typeof(GeometryType)).Length
            , out long mainGeometryTypeIndex);

        var mainColor = Colors.Dict[(ColorName)mainColorNameIndex];
        var mainGeom = Geometry.Dict[AreaName.BgMain][(GeometryType)mainGeometryTypeIndex];

        // HatchPatternTypeの決定
        var mainHatchPatternType = HatchPatternType.Fill;
        if (block2 != "")
            mainHatchPatternType = (HatchPatternType)(hash2 % Enum.GetNames(typeof(HatchPatternType)).Length);
        
        // サブシェイプ1
        if (block3 != ""){
            var index3 = hash3 % (Enum.GetNames(typeof(ColorName)).Length
            * Enum.GetNames(typeof(GeometryType)).Length);
            long sub1ColorNameIndex = Math.DivRem(index3,
                Enum.GetNames(typeof(GeometryType)).Length,
                 out long sub1GeometryTypeIndex);
            var sub1Color = Colors.Dict[(ColorName)sub1ColorNameIndex];
            var sub1Geom = Geometry.Dict[AreaName.BgSub1][(GeometryType)sub1GeometryTypeIndex];
            AddCircle(HatchPatternType.Fill,
                sub1Color,
                sub1Geom.Radius * Width,
                sub1Geom.CenterX * Width,
                sub1Geom.CenterY * Width);
        }

        if(block4 != ""){
            var index4 = hash4 % (Enum.GetNames(typeof(ColorName)).Length
            * Enum.GetNames(typeof(GeometryType)).Length);
            long sub2ColorNameIndex = Math.DivRem(index4,
                Enum.GetNames(typeof(GeometryType)).Length,
                 out long sub2GeometryTypeIndex);
            var sub2Color = Colors.Dict[(ColorName)sub2ColorNameIndex];
            var sub2Geom = Geometry.Dict[AreaName.BgSub2][(GeometryType)sub2GeometryTypeIndex];
            AddCircle(HatchPatternType.Fill,
                sub2Color,
                sub2Geom.Radius * Width,
                sub2Geom.CenterX * Width,
                sub2Geom.CenterY * Width);
        }

        switch ((ShapeType)mainShapeTypeIndex)
        {
            case ShapeType.Circle:
                AddCircle(mainHatchPatternType,
                    mainColor,
                    mainGeom.Radius * Width,
                    mainGeom.CenterX * Width,
                    mainGeom.CenterY * Width);
                break;
            case ShapeType.Diamond:
                AddDiamond(mainHatchPatternType,
                    mainColor,
                    mainGeom.Radius * Width,
                    mainGeom.CenterX * Width,
                    mainGeom.CenterY * Width);
                break;
            case ShapeType.Pentagon:
                AddPentagon(mainHatchPatternType,
                    mainColor,
                    mainGeom.Radius * Width,
                    mainGeom.CenterX * Width,
                    mainGeom.CenterY * Width);
                break;
            default:
                break;
        }

        return this;
    }

    public IconBuilder AddCircle(HatchPatternType hType, Color col, float radius, float centerX, float centerY)
    {
        // パターンを利用するにはSVGドキュメントへ追加する必要があるので注意
        // switch式。便利。
        var pattern = HatchPattern.GetSvgPatternServer(hType, col);
        if (hType != HatchPatternType.Fill)
            _svgDoc.Children.Add(pattern);

        _bgGroup.Children.Add(new SvgCircle
        {
            Radius = (SvgUnit)radius,
            CenterX = (SvgUnit)centerX,
            CenterY = (SvgUnit)centerY,
            Fill = hType != HatchPatternType.Fill ? pattern : new SvgColourServer(col),
        });

        return this;
    }

    public IconBuilder AddDiamond(HatchPatternType hType, Color col, float radius, float centerX, float centerY)
    {
        var pattern = HatchPattern.GetSvgPatternServer(hType, col);
        if (hType != HatchPatternType.Fill)
            _svgDoc.Children.Add(pattern);

        _bgGroup.Children.Add(new SvgPolygon
        {
            Points = new SvgPointCollection
            {
                (SvgUnit)(centerX + radius), (SvgUnit)centerY,
                (SvgUnit)centerX, (SvgUnit)(centerY + radius),
                (SvgUnit)(centerX - radius), (SvgUnit)centerY,
                (SvgUnit)centerX, (SvgUnit)(centerY - radius),
            },
            Fill = hType != HatchPatternType.Fill ? pattern : new SvgColourServer(col),
        });

        return this;
    }

    public IconBuilder AddPentagon(HatchPatternType hType, Color col, float radius, float centerX, float centerY)
    {
        var pattern = HatchPattern.GetSvgPatternServer(hType, col);
        if (hType != HatchPatternType.Fill)
            _svgDoc.Children.Add(pattern);

        _bgGroup.Children.Add(new SvgPolygon
        {
            Points = new SvgPointCollection
            {
                (SvgUnit)(centerX + radius * Math.Cos(0 * Math.PI / 5)), (SvgUnit)(centerY + radius * Math.Sin(0 * Math.PI / 5)),
                (SvgUnit)(centerX + radius * Math.Cos(2 * Math.PI / 5)), (SvgUnit)(centerY + radius * Math.Sin(2 * Math.PI / 5)),
                (SvgUnit)(centerX + radius * Math.Cos(4 * Math.PI / 5)), (SvgUnit)(centerY + radius * Math.Sin(4 * Math.PI / 5)),
                (SvgUnit)(centerX + radius * Math.Cos(6 * Math.PI / 5)), (SvgUnit)(centerY + radius * Math.Sin(6 * Math.PI / 5)),
                (SvgUnit)(centerX + radius * Math.Cos(8 * Math.PI / 5)), (SvgUnit)(centerY + radius * Math.Sin(8 * Math.PI / 5)),
            },
            Fill = hType != HatchPatternType.Fill ? pattern : new SvgColourServer(col),
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
        AddCircle(HatchPatternType.Fill, Color.FromArgb(255, 128, 128, 128), Width / 2, Width / 2, Width / 2);
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
