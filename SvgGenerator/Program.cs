using System.Runtime.InteropServices.JavaScript;

namespace SvgGenerator;

public partial class Program
{
    [JSExport]
    internal static string GeneratePtag(string tag, int width, int height) {
        var builder = new IconBuilder(width, height);
        return builder.generateBgFromText(tag)
//        .addForeground()
        .toSvg();
    }

    static void Main(string[] args)
    {
    }
}
