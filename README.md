# ConsoleifyCLI

ConsoleifyCLI is a Windows configuration utility that transforms a standard PC into a dedicated living-room gaming console. It removes the friction of mice, keyboards, and TV remotes, putting nothing but a controller between you and your Steam library.

---

### Disclaimer ⚠️
* **Registry:** Frequent registry edits are used to facilitate system changes. Last verified build: **Windows 11 Home 26200.7840**. Use at your own risk on unverified builds.
* **Downloads:** This tool fetches remote binaries for background services. Ensure all distribution URLs are trusted before use.

---

## Features

### Startup and Automation
* **Secure Auto-Logon:** Uses Microsoft Sysinternals to bypass the Windows login screen, moving directly from boot to Steam without a keyboard.
* **Steam Big Picture:** Configures Steam to launch on startup in full-screen mode for immediate controller navigation.
* **Power Management:** Re-maps the PC power button to Sleep mode, replicating the standby-and-resume behavior of modern consoles using only a controller.

### Minimalist Visuals
* **Desktop Icon Remover:** A background service monitors the desktop and instantly deletes shortcuts or installer icons to ensure seamless visual transitions between the BIOS and Steam.
* **Taskbar Suppression:** Forces the Windows taskbar into auto-hide mode to remove UI distractions.

### Hardware Orchestration
* **TV CEC Control:** Utilizes ADB (Android Debug Bridge) to automatically power on Google TVs and switch inputs upon controller input, mimicking the CEC functionality found in modern consoles.
* **Audio Device Switcher [Coming Soon]:** Provides a one-click audio switcher for the system tray, allowing for instant output redirection between home theater speakers and gaming headsets.

---

## Usage
1. Download and run `ConsoleifyCLI.exe`.
2. Toggle desired features using the number keys.
3. Press **Enter** to install or press **U** to switch to Uninstall mode to revert changes. 
