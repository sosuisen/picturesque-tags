using System.Runtime.InteropServices.JavaScript;

namespace PicturesqueTags;

public partial class Program
{
    #if WASM
    [JSExport]
    #endif
    internal static string GeneratePtag(string tag, int width, int height) {
        var builder = new IconBuilder(width, height);
        return builder.generateBgFromText(tag)
//        .addForeground()
        .toSvg();
    }

    static void Main(string[] args)
    {
        string svg = GeneratePtag("A", 100, 100);
        Console.WriteLine(svg);
    }
}
