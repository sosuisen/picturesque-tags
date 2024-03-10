namespace SvgGenerator;

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

public class Geometry(double radius, double centerX, double centerY)
{
    // BgMainDictは、BgMainの配置を決めるための辞書
    // BgMainの中心位置は、対角線方向にずれる。
    public static Dictionary<AreaName, Dictionary<GeometryType, Geometry>> Dict { get; } = new() {
        { AreaName.BgMain, new() {
                { GeometryType.LargeUpLeft, new Geometry(0.9, 0.2, 0.2) },
                { GeometryType.LargeUpRight, new Geometry(0.8, 0.2, 0.2) },
                { GeometryType.LargeDownLeft, new Geometry(0.7, 0.2, 0.2) },
                { GeometryType.LargeDownRight, new Geometry(0.6, 0.2, 0.2) },
                { GeometryType.MiddleUpLeft, new Geometry(0.46, -0.02, -0.02) },
                { GeometryType.MiddleUpRight, new Geometry(0.44, 0.02, -0.02) },
                { GeometryType.MiddleDownLeft, new Geometry(0.42, -0.02, 0.02) },
                { GeometryType.MiddleDownRight, new Geometry(0.40, 0, 0.02) },
                { GeometryType.SmallUpLeft, new Geometry(0.36, 0.1, 0.1) },
                { GeometryType.SmallUpRight, new Geometry(0.34, 0.1, 0.1) },
                { GeometryType.SmallDownLeft, new Geometry(0.32, 0.1, 0.1) },
                { GeometryType.SmallDownRight, new Geometry(0.30, 0.1, 0.1) },
            }
        },
        { AreaName.BgSub1, new(){
                { GeometryType.LargeUpLeft, new Geometry(0.9, 0.2, 0.2) },
                { GeometryType.LargeUpRight, new Geometry(0.8, 0.2, 0.2) },
                { GeometryType.LargeDownLeft, new Geometry(0.7, 0.2, 0.2) },
                { GeometryType.LargeDownRight, new Geometry(0.6, 0.2, 0.2) },
                { GeometryType.MiddleUpLeft, new Geometry(0.46, -0.02, -0.02) },
                { GeometryType.MiddleUpRight, new Geometry(0.44, 0.02, -0.02) },
                { GeometryType.MiddleDownLeft, new Geometry(0.42, -0.02, 0.02) },
                { GeometryType.MiddleDownRight, new Geometry(0.40, 0, 0.02) },
                { GeometryType.SmallUpLeft, new Geometry(0.36, 0.1, 0.1) },
                { GeometryType.SmallUpRight, new Geometry(0.34, 0.1, 0.1) },
                { GeometryType.SmallDownLeft, new Geometry(0.32, 0.1, 0.1) },
                { GeometryType.SmallDownRight, new Geometry(0.30, 0.1, 0.1) },
            }
        },
        { AreaName.BgSub2, new(){
                { GeometryType.LargeUpLeft, new Geometry(0.9, 0.2, 0.2) },
                { GeometryType.LargeUpRight, new Geometry(0.8, 0.2, 0.2) },
                { GeometryType.LargeDownLeft, new Geometry(0.7, 0.2, 0.2) },
                { GeometryType.LargeDownRight, new Geometry(0.6, 0.2, 0.2) },
                { GeometryType.MiddleUpLeft, new Geometry(0.46, -0.02, -0.02) },
                { GeometryType.MiddleUpRight, new Geometry(0.44, 0.02, -0.02) },
                { GeometryType.MiddleDownLeft, new Geometry(0.42, -0.02, 0.02) },
                { GeometryType.MiddleDownRight, new Geometry(0.40, 0, 0.02) },
                { GeometryType.SmallUpLeft, new Geometry(0.36, 0.1, 0.1) },
                { GeometryType.SmallUpRight, new Geometry(0.34, 0.1, 0.1) },
                { GeometryType.SmallDownLeft, new Geometry(0.32, 0.1, 0.1) },
                { GeometryType.SmallDownRight, new Geometry(0.30, 0.1, 0.1) },
            }
        },
    };


    public static Geometry GenerateGeometryFromHash(AreaName areaName, uint hash)
    {
        var type = (GeometryType)(hash % Enum.GetNames(typeof(GeometryType)).Length);
        return Dict[areaName][type];
    }

    // 半径。アイコンの縦横サイズに対する倍数 0.1 - 1.0d 
    // 0.5dより大きい場合、はみ出るが、はみ出た部分は表示されない。
    public double Radius { get; } = radius;
    // 中心からどれだけ離れているか。アイコンの縦横サイズに対する倍数 0.1 - 1.0d 
    public double CenterX { get; } = centerX;
    public double CenterY { get; } = centerY;

}
