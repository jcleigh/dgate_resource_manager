# Death Gate Resource Manager

A cross-platform tool to explore and analyze data files from the classic adventure game **Death Gate** (1994) by Legend Entertainment. This application has been modernized from the original C++ Qt implementation to a .NET 8 application using Avalonia UI for cross-platform compatibility.

## About Death Gate

Death Gate was a graphic adventure game released in 1994 by Legend Entertainment, based on the Dragonlance novel series "The Death Gate Cycle" by Margaret Weis and Tracy Hickman. The game featured rich storytelling, detailed graphics, and atmospheric music, making it a classic of the adventure game genre.

The game used proprietary file formats for its various resources including images, animations, audio, and text. This tool allows you to explore and analyze these resource files, providing insight into the game's structure and content.

## What This Program Does

Death Gate Resource Manager is a specialized file browser and analyzer that:

### 🔍 **Resource Discovery & Analysis**
- Automatically scans directories for Death Gate game files
- Identifies and categorizes different resource types
- Displays detailed metadata about each resource

### 📁 **Supported Resource Types**

#### **Images** (`.PIC`, `.SCR` files)
- Game backgrounds and scenes
- User interface elements  
- Character portraits and artwork
- Screenshots and title screens
- **Examples**: `DGLOGO.SCR`, `TITLE.SCR`, `DGATE000.PIC` through `DGATE086.PIC`

#### **Videos** (`.FLI`, `.FLC`, `.Q` files)  
- FLIC format animations and cutscenes
- Character animations and special effects
- **Examples**: `INTRO.FLC`, `ENDING.FLC`, specific character animations like `BALTAZAR.FLI`, `ZIFNAB.FLI`

#### **Audio** (`.XMI`, `.WAV` files)
- XMIDI music files for background music
- WAV audio for voice acting and sound effects  
- **Examples**: `MUSIC.XMI`, `SOUND.XMI`, `VOICE.WAV`

#### **Text** (`.TXT`, `.DAT` files)
- Game dialog and narrative text
- String resources and localization data
- Often compressed using Huffman coding algorithms
- **Examples**: `STRINGS.TXT`, `DIALOG.TXT`, string files like `DGATE???STR.DAT`

#### **Palettes**
- Color palette data for the game's 8-bit graphics
- Essential for proper image rendering

### 🎯 **Key Features**
- **Cross-platform**: Runs on Windows, macOS, and Linux
- **Modern UI**: Built with Avalonia UI using MVVM architecture
- **Resource Preview**: View detailed information about each resource type
- **File Analysis**: Parse and display metadata from proprietary game formats
- **Legacy Support**: Maintains compatibility with original Death Gate file structures

## Requirements

- **.NET 8.0 SDK** or later
- **Operating System**: Windows, macOS, or Linux

## Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/jcleigh/dgate_resource_manager.git
cd dgate_resource_manager
```

### 2. Build the Application
```bash
dotnet restore
dotnet build
```

### 3. Run the Application
```bash
dotnet run --project DGateResourceManager
```

Alternatively, run the compiled executable:
```bash
./DGateResourceManager/bin/Debug/net8.0/DGateResourceManager
```

## Usage

1. **Launch the application**
2. **Open a Death Gate directory**: Use `File > Open Directory` (Ctrl+O) to select a folder containing Death Gate game files
3. **Browse resources**: The left panel will populate with discovered resources categorized by type
4. **View details**: Select any resource to see detailed information in the right panel, including:
   - File size and path
   - Resource-specific metadata (dimensions for images, duration for audio/video, etc.)
   - Content preview (especially useful for text resources)

## Architecture

The application follows modern .NET practices:

### **Migration from C++ to .NET 8**
- **Original**: C++ with Qt GUI framework (Linux-specific)  
- **Current**: C# .NET 8 with Avalonia UI (cross-platform)
- **Benefits**: Better maintainability, cross-platform support, modern development ecosystem

### **Project Structure**
```
DGateResourceManager/
├── Models/           # Data models for different resource types
├── Services/         # Resource management and file parsing services  
├── ViewModels/       # MVVM pattern implementation
├── Views/           # Avalonia UI XAML views
└── Program.cs       # Application entry point
```

### **Key Components**
- **ResourceManager**: Core service for discovering and loading Death Gate files
- **TextParser**: Specialized parser for Death Gate's compressed text format
- **Resource Models**: Strongly-typed models for Images, Videos, Audio, Text, and Palettes
- **MainWindowViewModel**: Primary UI logic using MVVM pattern

## File Format Details

Death Gate used several proprietary and specialized formats:

### **Image Formats**
- Custom `.PIC` format with palette support
- `.SCR` files for screenshots and backgrounds  
- Typically 320x200 resolution with 8-bit color depth
- Associated palette files for color information

### **Video Formats**
- **FLIC** (`.FLI`, `.FLC`) - Animation format popular in 1990s games
- **Q files** - Custom video format used by Legend Entertainment
- Frame-based animations with variable frame rates

### **Audio Formats**  
- **XMIDI** (`.XMI`) - Extended MIDI format with enhanced features
- **WAV** - Standard PCM audio for voice and sound effects
- Often mono, 22.05kHz for voice samples

### **Text Formats**
- **Huffman Compressed**: Multiple versions (2, 3, 4) of compression algorithms
- **Dictionary-based**: Uses lookup tables for common words/phrases
- **Binary headers**: Contain metadata about string count, compression type, and stream size

## Development Status

### **Current Implementation**
✅ Cross-platform .NET 8 application  
✅ Resource discovery and categorization  
✅ Basic file format parsing  
✅ Modern MVVM UI architecture  
✅ Detailed metadata display  

### **Future Enhancements**
🔄 Complete Huffman text decompression (currently simplified)  
🔄 FLIC animation playback support  
🔄 XMIDI audio playback capabilities  
🔄 Full image rendering with palette support  
🔄 Resource export functionality  

### **Legacy C++ Features**
The original C++ implementation included:
- Complex Huffman decompression with dictionary support
- FLIC animation playback using external libraries
- MIDI playback using WildMIDI
- Custom Qt widgets for specialized data display

## Publishing & Distribution

See [PUBLISH.md](PUBLISH.md) for detailed instructions on building distributable executables for different platforms.

## Contributing

This project preserves an important piece of gaming history. Contributions are welcome, especially:
- Implementing remaining format parsers
- Adding resource export capabilities  
- Improving cross-platform compatibility
- Enhancing the user interface

## License & Acknowledgments

This tool is for educational and preservation purposes. Death Gate and its assets are property of their respective copyright holders.

Special thanks to the original C++ implementation contributors and the adventure gaming preservation community.
