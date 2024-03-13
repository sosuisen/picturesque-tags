
using PicturesqueTags;

public class Generator {
    public static string GenerateIcon(int width, int height, string tag)
    {
        var builder = new IconBuilder(width, height);
        return builder.generateBgFromText(tag)
        .addForeground()
        .toSvg();
    }

    public static string GenerateIcon(int width, int height, List<string> tags)
    {
        if (tags.Count > 0)
        {
            return GenerateIcon(width, height, tags[0]);
        }
        else
        {
            var builder = new IconBuilder(width, height);            
            return builder.Default().toSvg();
        }
    }
}