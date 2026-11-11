<h1 align="center">Tree — MIDI Visualizer</h1>

<p align="center">
    <img src="Kiva-MIDI/logo.png" width="128" alt="Tree logo"/>
    <br />
    <strong>Fast, Easy, Modern</strong>
</p>

Tree is a branded fork of [Kiva](https://github.com/arduano/Kiva), an optimized Windows MIDI player and visualizer.

## Features
- Extremely fast multithreaded rendering, best optimized for medium to high end systems.
- Support for switching between KDMAPI and WinMM in the settings.
- Support for Zenith color palettes and MIDI color events.
- Transparent or semi-transparent backgrounds.
- 256-key support and dynamic key range options.
- Two keyboard designs, with an option to disable the keyboard.
- Render MIDI visualizations to video with FFmpeg and an audio render.

## Build
Tree retains the original Visual Studio solution and targets .NET Framework 4.6.1. Open `Kiva.sln` in Visual Studio and build the `Kiva-MIDI` and `KivaInstaller` projects for x64 Release. The original `build.bat` also packages both outputs and requires Visual Studio MSBuild.

## Installation and updates
The original installer and automatic updater are configured to read release assets from the upstream Kiva repository. Until Tree has its own published release feed, use locally built binaries and do not rely on the upstream auto-updater for Tree releases. See the [original Kiva releases](https://github.com/arduano/Kiva/releases) for upstream downloads. DirectX 11 is required.

## Upstream project and license
Tree preserves the original project's features and license. Kiva was created by Arduano; its source and history are available at [arduano/Kiva](https://github.com/arduano/Kiva). This fork is licensed under the terms of the [Don't Be a Dick Public License](LICENSE). Copyright and license notices are retained.

## Screenshots
Original project screenshots: [Kiva README](https://github.com/arduano/Kiva#screenshots).
