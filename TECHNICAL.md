# Death Gate Resource Manager - Technical Documentation

This document provides technical details about the .NET 8 port and implementation specifics.

## Architecture Overview

### Migration Strategy
The application was migrated from C++ Qt to C# .NET 8 with the following key changes:

| Aspect | Original C++ | Current .NET 8 |
|--------|-------------|----------------|
| **UI Framework** | Qt 5.7+ | Avalonia UI 11.0 |
| **Platform Support** | Linux (Sabayon) only | Windows, macOS, Linux |
| **Language** | C++ | C# |
| **Pattern** | Traditional MVC | MVVM with data binding |
| **Dependencies** | Qt, WildMIDI, FLIC library | .NET 8, Avalonia, Microsoft.Extensions |

### Project Structure

```
DGateResourceManager/
├── Models/
│   └── ResourceModels.cs          # Data models for all resource types
├── Services/
│   ├── ResourceManager.cs         # Core resource discovery and loading
│   └── TextParser.cs              # Specialized text format parser
├── ViewModels/
│   └── MainWindowViewModel.cs     # MVVM implementation
├── Views/
│   ├── MainWindow.axaml           # Main UI layout
│   └── MainWindow.axaml.cs        # UI code-behind
├── App.axaml                      # Application styling
├── App.axaml.cs                   # Application entry point
└── Program.cs                     # Main program entry
```

## Key Implementation Details

### Resource Models Hierarchy

```csharp
IResourceModel (interface)
├── ResourceModel (base class)
    ├── ImageResource    - Width, Height, ColorDepth, HasPalette
    ├── VideoResource    - FrameCount, Duration, FrameRate  
    ├── AudioResource    - Duration, SampleRate, Channels
    ├── TextResource     - TextLines collection
    └── PaletteResource  - Colors collection
```

### File Format Recognition

The `ResourceManager` uses predefined sets of known filenames to identify resource types:

```csharp
// Image files - backgrounds, UI elements, character art
"DGLOGO.SCR", "TITLE.SCR", "DGATE000.PIC" through "DGATE086.PIC"

// Video files - animations and cutscenes  
"INTRO.FLC", "ENDING.FLC", character-specific animations

// Audio files - music and sound effects
"MUSIC.XMI", "SOUND.XMI", "VOICE.WAV"

// Text files - dialog and game strings
"STRINGS.TXT", "DIALOG.TXT", compressed .DAT files
```

### Text Parser Implementation

The `TextParser` service handles Death Gate's complex text format:

```csharp
public struct TextHeader
{
    public ushort StringCount;   // Number of text strings
    public ushort StreamSize;    // Size of compressed data  
    public ushort Flags;         // Compression type/version
}
```

**Current Status**: Basic header parsing and string extraction
**Future**: Full Huffman decompression (versions 2, 3, 4)

### MVVM Pattern Implementation

- **Model**: Resource data models with strongly-typed properties
- **View**: Avalonia XAML with data binding and templates
- **ViewModel**: `MainWindowViewModel` handles UI logic and commands

Key features:
- `INotifyPropertyChanged` for automatic UI updates
- `ObservableCollection<IResourceModel>` for dynamic resource lists  
- `RelayCommand` for menu actions
- Data templates for resource-specific UI rendering

## File Format Details

### Death Gate Image Format (.PIC)
- **Resolution**: Typically 320x200 pixels
- **Color Depth**: 8-bit (256 colors)
- **Palette**: Separate palette data required for rendering
- **Compression**: Custom RLE-style compression

### FLIC Animation Format (.FLI, .FLC)
- **Standard**: Autodesk Animator format
- **Features**: Frame-based animation with delta compression
- **Typical Use**: Character animations, cutscenes

### XMIDI Audio Format (.XMI)  
- **Base**: Extended MIDI standard
- **Enhancements**: Additional controller data, improved timing
- **Playback**: Originally required WildMIDI library

### Text Compression
Death Gate used sophisticated Huffman compression:

```
Version 2: Basic Huffman with static dictionary
Version 3: Enhanced compression with dynamic dictionaries  
Version 4: Most advanced compression (used in later games)
```

## Development Guidelines

### Adding New Resource Types
1. Create new model class inheriting from `ResourceModel`
2. Add recognition logic to `ResourceManager.LoadDirectoryAsync()`
3. Implement parsing method (e.g., `LoadNewResourceDetailsAsync()`)
4. Add XAML DataTemplate in `MainWindow.axaml`

### Parser Implementation
For complex formats, follow the `TextParser` pattern:
- Create service class in `Services/` directory
- Implement static parsing methods
- Handle binary data with `BinaryReader`
- Provide meaningful error messages

### UI Extensions
- Use Avalonia data binding for automatic updates
- Create resource-specific DataTemplates
- Follow MVVM pattern for testability
- Implement commands for user actions

## Performance Considerations

### Current Optimizations
- Async file loading prevents UI blocking
- Lazy loading of resource details
- Efficient string handling in text parser

### Future Optimizations  
- Caching of parsed resource data
- Background thumbnail generation
- Virtualized lists for large directories

## Testing Strategy

### Unit Tests (Future)
```csharp
// Resource parsing tests
[Test] public void TextParser_ParseHeader_ValidInput()
[Test] public void ResourceManager_LoadDirectory_ValidPath()

// Model validation tests  
[Test] public void ImageResource_Properties_SetCorrectly()
```

### Integration Tests (Future)
- Full directory loading scenarios
- UI interaction testing with Avalonia test framework

## Known Limitations

### Simplified Implementations
1. **Text Decompression**: Only basic parsing, no full Huffman decompression
2. **Image Rendering**: Metadata only, no actual image display
3. **Audio Playback**: No MIDI or WAV playback capabilities
4. **Video Playback**: No FLIC animation support

### Platform Differences
- File path handling uses .NET standards for cross-platform compatibility
- No platform-specific audio/video libraries integrated yet

## Contributing Guidelines

### Code Style
- Follow standard C# conventions
- Use meaningful variable and method names
- Add XML documentation for public APIs
- Implement proper error handling

### Pull Request Process
1. Focus on single feature/fix per PR
2. Include unit tests for new functionality
3. Update documentation for user-facing changes
4. Ensure cross-platform compatibility

This technical documentation serves as a guide for developers working on the Death Gate Resource Manager .NET port.