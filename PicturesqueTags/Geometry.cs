using Svg.FilterEffects;

namespace PicturesqueTags;

public enum ShapeType
{
    Circle,
    Diamond,
    Pentagon,
}

public enum GeometryType
{
    LargeUpLeft,
    LargeUpRight,
    LargeDownLeft,
    LargeDownRight,
    MiddleUpLeft,
    MiddleUpRight,
    MiddleDownLeft,
    MiddleDownRight,
    SmallUpLeft,
    SmallUpRight,
    SmallDownLeft,
    SmallDownRight,
}

public class Geometry
{
    // BgMainDictは、BgMainの配置を決めるための辞書
    // BgMainの中心位置は、対角線方向にずれる。
    // 半径はUpLeftが最大、DownRightが最小
    public static Dictionary<AreaName, Dictionary<GeometryType, Geometry>> Dict { get; } = new() {
        { AreaName.BgMain, new() {
                { GeometryType.LargeUpLeft, new Geometry(0.9, -0.3, -0.3) },
                { GeometryType.LargeUpRight, new Geometry(0.8, 0.3, -0.3) },
                { GeometryType.LargeDownLeft, new Geometry(0.7, -0.3, 0.3) },
                { GeometryType.LargeDownRight, new Geometry(0.6, 0.3, 0.3) },
                { GeometryType.MiddleUpLeft, new Geometry(0.46, -0.2, -0.2) },
                { GeometryType.MiddleUpRight, new Geometry(0.44, 0.2, -0.2) },
                { GeometryType.MiddleDownLeft, new Geometry(0.42, -0.2, 0.2) },
                { GeometryType.MiddleDownRight, new Geometry(0.40, 0.2, 0.2) },
                { GeometryType.SmallUpLeft, new Geometry(0.36, -0.1, -0.1) },
                { GeometryType.SmallUpRight, new Geometry(0.34, 0.1, -0.1) },
                { GeometryType.SmallDownLeft, new Geometry(0.32, -0.1, 0.1) },
                { GeometryType.SmallDownRight, new Geometry(0.30, 0.1, 0.1) },
            }
        },
        // BgSub1の中心位置は、対角線方向から少し下にずれる。
        // 半径はDownRightが最大、UpLeftが最小
        { AreaName.BgSub1, new(){
                { GeometryType.LargeUpLeft, new Geometry(0.24, -0.25, -0.25).Offset(-0.1, -0.08) },
                { GeometryType.LargeUpRight, new Geometry(0.26, 0.25, -0.25).Offset(0.1, -0.08) },
                { GeometryType.LargeDownLeft, new Geometry(0.28, -0.25, 0.25).Offset(-0.1, 0.12) },
                { GeometryType.LargeDownRight, new Geometry(0.3, 0.25, 0.25).Offset(0.1, 0.12) },
                { GeometryType.MiddleUpLeft, new Geometry(0.16, -0.25, -0.25).Offset(-0.1, -0.08) },
                { GeometryType.MiddleUpRight, new Geometry(0.18, 0.25, -0.25).Offset(0.1, -0.08) },
                { GeometryType.MiddleDownLeft, new Geometry(0.20, -0.25, 0.25).Offset(-0.1, 0.12) },
                { GeometryType.MiddleDownRight, new Geometry(0.22, 0.25, 0.25).Offset(0.1, 0.12) },
                { GeometryType.SmallUpLeft, new Geometry(0.06, -0.25, -0.25).Offset(-0.1, -0.08) },
                { GeometryType.SmallUpRight, new Geometry(0.08, 0.25, -0.25).Offset(0.1, -0.08) },
                { GeometryType.SmallDownLeft, new Geometry(0.10, -0.25, 0.25).Offset(-0.1, 0.12) },
                { GeometryType.SmallDownRight, new Geometry(0.12, 0.25, 0.25).Offset(0.1, 0.12) },
            }
        },
        // BgSub1の中心位置は、対角線方向から少し下にずれる。
        // 半径はがUpLeft最大、DownRightが最小
        { AreaName.BgSub2, new(){
                { GeometryType.LargeUpLeft, new Geometry(0.3, -0.25, -0.25).Offset(-0.1, -0.12) },
                { GeometryType.LargeUpRight, new Geometry(0.28, 0.25, -0.25).Offset(0.1, -0.12) },
                { GeometryType.LargeDownLeft, new Geometry(0.26, -0.25, 0.25).Offset(-0.1, 0.08) },
                { GeometryType.LargeDownRight, new Geometry(0.24, 0.25, 0.25).Offset(0.1, 0.08) },
                { GeometryType.MiddleUpLeft, new Geometry(0.22, -0.25, -0.25).Offset(-0.1, -0.12) },
                { GeometryType.MiddleUpRight, new Geometry(0.20, 0.25, -0.25).Offset(0.1, -0.12) },
                { GeometryType.MiddleDownLeft, new Geometry(0.18, -0.25, 0.25).Offset(-0.1, 0.08) },
                { GeometryType.MiddleDownRight, new Geometry(0.16, 0.25, 0.25).Offset(0.1, 0.08) },
                { GeometryType.SmallUpLeft, new Geometry(0.12, -0.25, -0.25).Offset(-0.1, -0.08) },
                { GeometryType.SmallUpRight, new Geometry(0.10, 0.25, -0.25).Offset(0.1, -0.08) },
                { GeometryType.SmallDownLeft, new Geometry(0.08, -0.25, 0.25).Offset(-0.1, 0.08) },
                { GeometryType.SmallDownRight, new Geometry(0.06, 0.25, 0.25).Offset(0.1, 0.08) },
            }
        },
    };


    public static Geometry GenerateGeometryFromHash(AreaName areaName, uint hash)
    {
        var type = (GeometryType)(hash % Enum.GetNames(typeof(GeometryType)).Length);
        return Dict[areaName][type];
    }

    public Geometry Offset(double offsetX, double offsetY)
    {
        return Offset((float)offsetX, (float)offsetY);
    }

    public Geometry Offset(float offsetX, float offsetY)
    {
        CenterX += offsetX;
        CenterY += offsetY;
        return this;
    }

    // SVGライブラリがdoubleではなくfloatなので、floatにしている。

    // 半径。アイコンの縦横サイズに対する倍数 0.1 - 1.0d 
    // 0.5dより大きい場合、はみ出るが、はみ出た部分は表示されない。
    public float Radius { get; set; }
    // 中心からどれだけ離れているか。アイコンの縦横サイズに対する倍数 0.1 - 1.0d 
    public float CenterX { get; set; }
    public float CenterY { get; set; }

    public Geometry(float radius, float centerX, float centerY) {
        Radius = radius;
        CenterX = centerX;
        CenterY = centerY;
    }

    public Geometry(double radius, double centerX, double centerY) {
        Radius = (float)radius;
        CenterX = (float)centerX;
        CenterY = (float)centerY;
    }
}
