# Death Gate Resource Manager - C# .NET 8 Port

This is a C# .NET 8 port of the original C++ Qt-based Death Gate Resource Manager, designed to explore data files from the Legend Entertainment game Death Gate. The port uses Avalonia UI for cross-platform compatibility on Windows, macOS, and Linux.

## Features

- Cross-platform GUI application using Avalonia UI
- Browse and analyze Death Gate game resource files
- Support for multiple resource types:
  - Images (SCR files)
  - Videos (FLC files)
  - Audio (XMI, WAV files)
  - Text (TXT files)
  - Palettes
- Modern .NET 8 architecture with dependency injection
- MVVM pattern for clean separation of concerns

## Requirements

- .NET 8.0 SDK or later
- Windows, macOS, or Linux

## Building

1. Clone the repository
2. Navigate to the project directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Build the project:
   ```bash
   dotnet build
   ```

## Running

```bash
dotnet run --project DGateResourceManager
```

Or run the executable directly after building:
```bash
./DGateResourceManager/bin/Debug/net8.0/DGateResourceManager
```

## Usage

1. Start the application
2. Use File > Open Directory to select a Death Gate game directory
3. Browse through the detected resources in the left panel
4. Select a resource to view its details in the right panel

## Architecture

The application follows a modern .NET architecture:

- **Models**: Data models for different resource types (Image, Video, Audio, Text, Palette)
- **Services**: Resource management and file parsing services
- **ViewModels**: MVVM pattern with data binding
- **Views**: Avalonia UI XAML views

## Future Enhancements

The current implementation provides a foundation for the full port. Future enhancements could include:

- Complete implementation of the complex text decompression algorithms from the original C++
- FLIC animation playback support
- MIDI audio playback
- Image rendering with palette support
- Enhanced resource preview capabilities

## Original C++ Implementation

This port is based on the original C++ Qt implementation that included:
- Complex Huffman decompression for text resources
- FLIC animation playback using the flic library
- MIDI playback using WildMIDI
- Custom image format parsing
- Qt-based UI with various specialized widgets

The C# port maintains the same core functionality while providing better cross-platform compatibility and modernized architecture.