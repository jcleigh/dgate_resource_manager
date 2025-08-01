using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using DGateResourceManager.Models;
using DGateResourceManager.Services;

namespace DGateResourceManager;

/// <summary>
/// Interface for managing Death Gate resource files. Provides methods to discover, 
/// load, and validate Death Gate game directories and their associated resource files.
/// </summary>
public interface IResourceManager
{
    /// <summary>
    /// Asynchronously loads and analyzes all Death Gate resources from the specified directory.
    /// </summary>
    /// <param name="directoryPath">Path to the directory containing Death Gate game files</param>
    /// <returns>A list of discovered and parsed resource models</returns>
    Task<List<IResourceModel>> LoadDirectoryAsync(string directoryPath);
    
    /// <summary>
    /// Asynchronously loads the raw binary data from a resource file.
    /// </summary>
    /// <param name="filePath">Full path to the resource file</param>
    /// <returns>Byte array containing the file's binary data</returns>
    Task<byte[]> LoadResourceDataAsync(string filePath);
    
    /// <summary>
    /// Validates whether a directory contains recognizable Death Gate resource files.
    /// </summary>
    /// <param name="directoryPath">Path to the directory to validate</param>
    /// <returns>True if the directory contains known Death Gate files, false otherwise</returns>
    bool IsValidResourceDirectory(string directoryPath);
}

/// <summary>
/// Core service for discovering, loading, and parsing Death Gate game resource files.
/// Recognizes and processes images (.PIC, .SCR), videos (.FLI, .FLC), audio (.XMI, .WAV), 
/// and text (.TXT, .DAT) resources from the classic 1994 adventure game.
/// </summary>
public class ResourceManager : IResourceManager
{
    /// <summary>
    /// Known Death Gate image files including screenshots, backgrounds, and UI elements.
    /// These files typically use custom image formats with 8-bit color palettes.
    /// </summary>
    private readonly HashSet<string> _knownImageFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "DGLOGO.SCR", "EAPMLOGO.SCR", "LSLOGO.SCR", "TITLE.SCR",
        "WNDWS.SCR", "ITEMS.SCR", "SPELLS.SCR", "MOUSE.SCR",
        "CHARGEN.SCR", "ENDGAME.SCR", "DEATH.SCR", "PAPER.SCR"
    };

    /// <summary>
    /// Known Death Gate video files including FLIC animations and cutscenes.
    /// FLIC format was popular for game animations in the 1990s.
    /// </summary>
    private readonly HashSet<string> _knownVideoFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "INTRO.FLC", "ENDING.FLC", "DEATH.FLC"
    };

    /// <summary>
    /// Known Death Gate audio files including XMIDI music and WAV sound effects.
    /// XMIDI is an extended MIDI format with enhanced timing and controller data.
    /// </summary>
    private readonly HashSet<string> _knownAudioFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "MUSIC.XMI", "SOUND.XMI", "VOICE.WAV"
    };

    /// <summary>
    /// Known Death Gate text files containing game dialog, strings, and narrative text.
    /// These files often use sophisticated Huffman compression algorithms.
    /// </summary>
    private readonly HashSet<string> _knownTextFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "STRINGS.TXT", "DIALOG.TXT", "ITEMS.TXT"
    };

    public bool IsValidResourceDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            return false;

        var files = Directory.GetFiles(directoryPath);
        var fileNames = files.Select(Path.GetFileName).ToHashSet(StringComparer.OrdinalIgnoreCase);

        // Check if at least some known files exist
        return _knownImageFiles.Any(fileNames.Contains) ||
               _knownVideoFiles.Any(fileNames.Contains) ||
               _knownAudioFiles.Any(fileNames.Contains) ||
               _knownTextFiles.Any(fileNames.Contains);
    }

    public async Task<List<IResourceModel>> LoadDirectoryAsync(string directoryPath)
    {
        var resources = new List<IResourceModel>();

        if (!Directory.Exists(directoryPath))
            return resources;

        var files = Directory.GetFiles(directoryPath);

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);
            var fileInfo = new FileInfo(file);

            if (_knownImageFiles.Contains(fileName))
            {
                var imageResource = new ImageResource
                {
                    Name = fileName,
                    FilePath = file,
                    Size = fileInfo.Length
                };
                
                await LoadImageDetailsAsync(imageResource);
                resources.Add(imageResource);
            }
            else if (_knownVideoFiles.Contains(fileName))
            {
                var videoResource = new VideoResource
                {
                    Name = fileName,
                    FilePath = file,
                    Size = fileInfo.Length
                };
                
                await LoadVideoDetailsAsync(videoResource);
                resources.Add(videoResource);
            }
            else if (_knownAudioFiles.Contains(fileName))
            {
                var audioResource = new AudioResource
                {
                    Name = fileName,
                    FilePath = file,
                    Size = fileInfo.Length
                };
                
                await LoadAudioDetailsAsync(audioResource);
                resources.Add(audioResource);
            }
            else if (_knownTextFiles.Contains(fileName))
            {
                var textResource = new TextResource
                {
                    Name = fileName,
                    FilePath = file,
                    Size = fileInfo.Length
                };
                
                await LoadTextDetailsAsync(textResource);
                resources.Add(textResource);
            }
        }

        return resources;
    }

    public async Task<byte[]> LoadResourceDataAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return Array.Empty<byte>();

        return await File.ReadAllBytesAsync(filePath);
    }

    private async Task LoadImageDetailsAsync(ImageResource resource)
    {
        try
        {
            // Basic implementation - in a full port, this would parse the actual image format
            var data = await File.ReadAllBytesAsync(resource.FilePath);
            
            // Placeholder values - real implementation would parse the header
            resource.Width = 320;
            resource.Height = 200;
            resource.ColorDepth = 8;
            resource.HasPalette = true;
        }
        catch
        {
            // Error handling
        }
    }

    private async Task LoadVideoDetailsAsync(VideoResource resource)
    {
        try
        {
            // Basic implementation - in a full port, this would parse FLIC format
            var data = await File.ReadAllBytesAsync(resource.FilePath);
            
            // Placeholder values
            resource.FrameCount = 100;
            resource.Duration = 5000; // ms
            resource.FrameRate = 20;
        }
        catch
        {
            // Error handling
        }
    }

    private async Task LoadAudioDetailsAsync(AudioResource resource)
    {
        try
        {
            // Basic implementation - would parse XMI/WAV headers
            var data = await File.ReadAllBytesAsync(resource.FilePath);
            
            // Placeholder values
            resource.Duration = 30000; // ms
            resource.SampleRate = 22050;
            resource.Channels = 1;
        }
        catch
        {
            // Error handling
        }
    }

    private async Task LoadTextDetailsAsync(TextResource resource)
    {
        try
        {
            var data = await File.ReadAllBytesAsync(resource.FilePath);
            
            // Use the new TextParser to analyze the data
            resource.TextLines = TextParser.ParseTextData(data);
        }
        catch (Exception ex)
        {
            resource.TextLines.Add($"Error loading text resource: {ex.Message}");
        }
    }
}