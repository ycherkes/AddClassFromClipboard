[![Stand With Ukraine](https://raw.githubusercontent.com/vshymanskyy/StandWithUkraine/main/banner2-direct.svg)](https://stand-with-ukraine.pp.ua)

# [<img src="https://upload.wikimedia.org/wikipedia/commons/4/44/Microsoft_logo.svg" alt="Add Class From Clipboard Marketplace" width="75"/>](https://marketplace.visualstudio.com/items?itemName=YevhenCherkes.AddClassFromClipboard) [Add Class From Clipboard - Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=YevhenCherkes.AddClassFromClipboard)


## Overview

This Visual Studio extension lets developers quickly create a new C# class (interface or enum) file from the clipboard text content.

---

## Features

- **Placement**:
  - In the **Project** menu
    ![image](https://github.com/user-attachments/assets/144888a6-6f3c-4edb-8109-645828916a48)
  - In the **Solution Explorer** context menu under "Add."
    ![image](https://github.com/user-attachments/assets/b5bf8c35-1a11-4eb7-a1da-b043ec070a4f)

- **Automatic Class Naming**:
  - Extracts the class name from the clipboard content to name the new file appropriately.

- **Keyboard Shortcut**:
  - Quickly invoke the command with `Ctrl + Shift + E`.

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
4. No file will be created if a valid class name cannot be extracted.

---

## Keyboard Shortcut

- **Default Shortcut**: `Ctrl + Shift + E`
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
