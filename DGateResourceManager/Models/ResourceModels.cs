using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DGateResourceManager.Models;

/// <summary>
/// Base interface for all Death Gate resource models. Provides common properties
/// shared by all resource types discovered in Death Gate game directories.
/// </summary>
public interface IResourceModel
{
    /// <summary>File name of the resource</summary>
    string Name { get; }
    
    /// <summary>Type of resource (Image, Video, Audio, Text, Palette)</summary>
    string Type { get; }
    
    /// <summary>File size in bytes</summary>
    long Size { get; }
    
    /// <summary>Full file path to the resource</summary>
    string FilePath { get; }
}

/// <summary>
/// Base implementation for Death Gate resource models. Provides common properties
/// and serves as the foundation for specialized resource types.
/// </summary>
public class ResourceModel : IResourceModel
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long Size { get; set; }
    public string FilePath { get; set; } = string.Empty;
}

/// <summary>
/// Represents a Death Gate image resource (.PIC, .SCR files).
/// Death Gate images typically use 8-bit color with custom palettes and compression.
/// </summary>
public class ImageResource : ResourceModel
{
    /// <summary>Image width in pixels (typically 320 for Death Gate)</summary>
    public int Width { get; set; }
    
    /// <summary>Image height in pixels (typically 200 for Death Gate)</summary>
    public int Height { get; set; }
    
    /// <summary>Color depth in bits (typically 8-bit for Death Gate)</summary>
    public int ColorDepth { get; set; }
    
    /// <summary>Whether this image requires an external palette file</summary>
    public bool HasPalette { get; set; }
    
    public ImageResource()
    {
        Type = "Image";
    }
}

/// <summary>
/// Represents a Death Gate video resource (.FLI, .FLC files).
/// Death Gate uses FLIC format animations for cutscenes and character animations.
/// </summary>
public class VideoResource : ResourceModel
{
    /// <summary>Total number of animation frames</summary>
    public int FrameCount { get; set; }
    
    /// <summary>Total animation duration in milliseconds</summary>
    public int Duration { get; set; }
    
    /// <summary>Animation frame rate (frames per second)</summary>
    public int FrameRate { get; set; }
    
    public VideoResource()
    {
        Type = "Video";
    }
}

/// <summary>
/// Represents a Death Gate audio resource (.XMI, .WAV files).
/// Death Gate uses XMIDI for background music and WAV for voice/sound effects.
/// </summary>
public class AudioResource : ResourceModel
{
    /// <summary>Audio duration in milliseconds</summary>
    public int Duration { get; set; }
    
    /// <summary>Sample rate in Hz (typically 22050 for Death Gate)</summary>
    public int SampleRate { get; set; }
    
    /// <summary>Number of audio channels (1 for mono, 2 for stereo)</summary>
    public int Channels { get; set; }
    
    public AudioResource()
    {
        Type = "Audio";
    }
}

/// <summary>
/// Represents a Death Gate text resource (.TXT, .DAT files).
/// Death Gate text files often use Huffman compression for game dialog and strings.
/// </summary>
public class TextResource : ResourceModel
{
    /// <summary>Parsed text lines from the resource, including analysis of compressed data</summary>
    public List<string> TextLines { get; set; } = new();
    
    public TextResource()
    {
        Type = "Text";
    }
}

/// <summary>
/// Represents a Death Gate color palette resource.
/// Death Gate uses 256-color palettes for its 8-bit graphics.
/// </summary>
public class PaletteResource : ResourceModel
{
    /// <summary>Collection of colors in this palette (typically 256 colors)</summary>
    public List<Color> Colors { get; set; } = new();
    
    public PaletteResource()
    {
        Type = "Palette";
    }
}

/// <summary>
/// Represents an RGBA color value used in Death Gate palettes.
/// </summary>
public struct Color
{
    /// <summary>Red component (0-255)</summary>
    public byte R { get; set; }
    
    /// <summary>Green component (0-255)</summary>
    public byte G { get; set; }
    
    /// <summary>Blue component (0-255)</summary>
    public byte B { get; set; }
    
    /// <summary>Alpha/transparency component (0-255, typically 255 for opaque)</summary>
    public byte A { get; set; }
    
    public Color(byte r, byte g, byte b, byte a = 255)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}