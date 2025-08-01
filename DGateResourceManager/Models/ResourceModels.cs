using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DGateResourceManager.Models;

public interface IResourceModel
{
    string Name { get; }
    string Type { get; }
    long Size { get; }
    string FilePath { get; }
}

public class ResourceModel : IResourceModel
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
    public string FilePath { get; set; } = string.Empty;
}

public class ImageResource : ResourceModel
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int ColorDepth { get; set; }
    public bool HasPalette { get; set; }
    
    public ImageResource()
    {
        Type = "Image";
    }
}

public class VideoResource : ResourceModel
{
    public int FrameCount { get; set; }
    public int Duration { get; set; }
    public int FrameRate { get; set; }
    
    public VideoResource()
    {
        Type = "Video";
    }
}

public class AudioResource : ResourceModel
{
    public int Duration { get; set; }
    public int SampleRate { get; set; }
    public int Channels { get; set; }
    
    public AudioResource()
    {
        Type = "Audio";
    }
}

public class TextResource : ResourceModel
{
    public List<string> TextLines { get; set; } = new();
    
    public TextResource()
    {
        Type = "Text";
    }
}

public class PaletteResource : ResourceModel
{
    public List<Color> Colors { get; set; } = new();
    
    public PaletteResource()
    {
        Type = "Palette";
    }
}

public struct Color
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
    public byte A { get; set; }
    
    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}