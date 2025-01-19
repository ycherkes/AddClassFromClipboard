[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/banner2-direct.svg)](https://stand-with-ukraine.pp.ua)

# Add Class From Clipboard - Visual Studio Extension

## Overview

The **Add Class From Clipboard** Visual Studio extension enables developers to quickly create a new C# class (interface or enum) file from the clipboard text content.

---

## Features

- **Contextual Placement**:
  - Available in the **Project** menu
  - Available in the **Solution Explorer** context menu under "Add."

- **Automatic Class Naming**:
  - Extracts the class name from the clipboard content to name the new file appropriately.

- **Keyboard Shortcut**:
  - Quickly invoke the command with `Ctrl + Shift + Alt + C`.

---

## How to Use

1. Copy valid C# code containing a class declaration to your clipboard.
2. In Visual Studio:
   - Right-click a folder in **Solution Explorer**, navigate to **Add**, and select "Add Class From Clipboard."
   - Or, open the **Project** menu and select "Add Class From Clipboard."
3. The extension will:
   - Parse the clipboard content.
   - Extract the class name.
   - Create a `.cs` file named after the class in the selected folder.
4. If a valid class name cannot be extracted, no file will be created.

---

## Keyboard Shortcut

- **Default Shortcut**: `Ctrl + Shift + Alt + C`
- To customize the shortcut:
  1. Go to **Tools -> Options -> Keyboard**.
  2. Search for the command by its display name: `Add Class From Clipboard`.
  3. Assign a new shortcut if desired.

## Contributing

Contributions are welcome! To contribute:
1. Clone the repository.
2. Make changes or add new features.
3. Submit a pull request for review.

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Contact

For issues or feature requests, please open an issue on the [GitHub repository](https://github.com/ycherkes/AddClassFromClipboard).
