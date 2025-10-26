# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Sefinek Blocklists is a WPF desktop application that provides a WebView2-based interface for the Sefinek blocklist generator (https://sefinek.net/blocklist-generator). The application wraps a web interface in a native Windows application with persistent URL state management.

## Build and Run Commands

### Building the solution
```bash
# Build all projects (Debug configuration for x64)
dotnet build "Blocklists by Sefinek.sln" -c Debug -p:Platform=x64

# Build for specific platforms
dotnet build "Blocklists by Sefinek.sln" -c Debug -p:Platform=x86
dotnet build "Blocklists by Sefinek.sln" -c Debug -p:Platform=ARM64

# Release builds
dotnet build "Blocklists by Sefinek.sln" -c Release -p:Platform=x64
```

### Building the main application only
```bash
dotnet build "SefinekBlocklists/Sefinek Blocklists.csproj" -c Debug -p:Platform=x64
```

### Running the application
```bash
# Run from project directory
dotnet run --project "SefinekBlocklists/Sefinek Blocklists.csproj"
```

### Cleaning build artifacts
```bash
dotnet clean "Blocklists by Sefinek.sln"
```

## Solution Structure

The solution consists of two projects:

1. **SefinekBlocklists** (`SefinekBlocklists/Sefinek Blocklists.csproj`)
   - Main WPF application project
   - .NET 8.0 Windows target framework
   - Entry point: `SefinekBlocklists.App`

2. **WinPackagingProject** (`WinPackagingProject/WinPackagingProject.wapproj`)
   - Windows Application Packaging Project for Microsoft Store deployment
   - References the main application project
   - Contains app manifest and asset images
   - Targets Windows 10 version 10.0.22621.0 (minimum: 10.0.14393.0)

## Architecture

### Application Flow
1. **App startup** (`App.xaml.cs`): Minimal WPF application entry point
2. **MainWindow initialization** (`MainWindow.xaml.cs`):
   - Creates WebView2 control with custom user agent
   - Loads last visited URL from settings (defaults to https://sefinek.net/blocklist-generator)
   - Persists current URL on successful navigation
   - Settings stored in `%APPDATA%/Sefinek Blocklists/settings.json`

### Key Components

**MainWindow.xaml.cs** - Main window logic
- `InitializeWebView()`: Sets up WebView2 environment and applies security settings
- `OnWebViewInitialized()`: Loads URL from settings or default
- `OnNavigationCompleted()`: Persists current URL on successful navigation
- `LoadUrlFromSettings()`: Deserializes settings from JSON
- `SaveUrlToSettings()`: Serializes current URL to JSON settings file

**Models/Settings.cs** - Settings data model
- Simple POCO with `CurrentUrl` property for JSON serialization

**Scripts/Utils.cs** - Utility functions
- `AppFileVersion`: Gets application version from assembly metadata
- `ShowErrorMessage()`: Displays WPF error message boxes

### WebView2 Configuration

The WebView2 control is configured with the following security and UX settings (MainWindow.xaml.cs:32-37):
- Custom user agent suffix: `SefinekBlocklists/{version}`
- DevTools disabled in production
- Status bar disabled
- Host objects disallowed
- Password autosave disabled
- User data folder: `%APPDATA%/Sefinek Blocklists`

## Dependencies

- **Microsoft.Web.WebView2** (v1.0.3537.50): Embedded Chromium browser control
- **Newtonsoft.Json** (v13.0.4): JSON serialization for settings
- **Microsoft.Windows.SDK.BuildTools** (v10.0.26100.6901): Windows SDK for packaging project

## Platform Support

The application supports three architectures:
- x64 (64-bit Intel/AMD)
- x86 (32-bit Intel/AMD)
- ARM64 (ARM 64-bit)

Runtime identifiers: `win-x64`, `win-x86`, `win-arm64`

## File Locations

- **Application data**: `%APPDATA%/Sefinek Blocklists/`
- **Settings file**: `%APPDATA%/Sefinek Blocklists/settings.json`
- **WebView2 cache**: `%APPDATA%/Sefinek Blocklists/` (managed by CoreWebView2Environment)