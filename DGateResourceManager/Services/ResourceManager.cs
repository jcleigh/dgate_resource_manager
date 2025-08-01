using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using DGateResourceManager.Models;
using DGateResourceManager.Services;

namespace DGateResourceManager;

public interface IResourceManager
{
    Task<List<IResourceModel>> LoadDirectoryAsync(string directoryPath);
    Task<byte[]> LoadResourceDataAsync(string filePath);
    bool IsValidResourceDirectory(string directoryPath);
}

public class ResourceManager : IResourceManager
{
    private readonly HashSet<string> _knownImageFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "DGLOGO.SCR", "EAPMLOGO.SCR", "LSLOGO.SCR", "TITLE.SCR",
        "WNDWS.SCR", "ITEMS.SCR", "SPELLS.SCR", "MOUSE.SCR",
        "CHARGEN.SCR", "ENDGAME.SCR", "DEATH.SCR", "PAPER.SCR"
    };

    private readonly HashSet<string> _knownVideoFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "INTRO.FLC", "ENDING.FLC", "DEATH.FLC"
    };

    private readonly HashSet<string> _knownAudioFiles = new(StringComparer.OrdinalIgnoreCase)
    {
        "MUSIC.XMI", "SOUND.XMI", "VOICE.WAV"
    };

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