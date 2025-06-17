# Cross-Platform Publishing Commands

## Windows (x64)
```bash
dotnet publish DGateResourceManager/DGateResourceManager.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

## macOS (x64)
```bash
dotnet publish DGateResourceManager/DGateResourceManager.csproj -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true
```

## macOS (ARM64)
```bash
dotnet publish DGateResourceManager/DGateResourceManager.csproj -c Release -r osx-arm64 --self-contained true -p:PublishSingleFile=true
```

## Linux (x64)
```bash
dotnet publish DGateResourceManager/DGateResourceManager.csproj -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true
```

## Linux (ARM64)
```bash
dotnet publish DGateResourceManager/DGateResourceManager.csproj -c Release -r linux-arm64 --self-contained true -p:PublishSingleFile=true
```

The published executables will be located in:
`DGateResourceManager/bin/Release/net8.0/{runtime}/publish/`

## Development
For development, simply run:
```bash
dotnet run --project DGateResourceManager
```