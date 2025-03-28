﻿[![Stand With Ukraine](https://github.com/user-attachments/assets/f9a7bb0c-5cec-45da-93d8-8eb9f17c51e7)](https://stand-with-ukraine.pp.ua)


[![Add Class From Clipboard](https://YevhenCherkes.gallerycdn.vsassets.io/extensions/yevhencherkes/addclassfromclipboard/0.0.2/1737809682937/Microsoft.VisualStudio.Services.Icons.Default)](https://marketplace.visualstudio.com/items?itemName=YevhenCherkes.AddClassFromClipboard)

# [Add Class From Clipboard - Visual Studio Extension](https://marketplace.visualstudio.com/items?itemName=YevhenCherkes.AddClassFromClipboard)

## Overview

This Visual Studio extension lets developers quickly create a new C# class (interface, enum, or delegate) file from the clipboard text content.

---

## Features

- **Placement**:
  - In the **Project** menu
    ![image](https://github.com/user-attachments/assets/c33ac5a3-168b-4f35-a3b9-23797f5cb583)
  - In the **Solution Explorer** context menu under "Add."
    ![image](https://github.com/user-attachments/assets/ce6e1908-6287-467f-a8cd-c0e58e44e6e9)

- **Automatic Class Naming**:
  - Extracts the class name from the clipboard content to name the new file appropriately.

- **Keyboard Shortcut**:
  - Quickly invoke the command with `Ctrl + E, Ctrl + V`.

---

## How to Use

1. Copy valid C# code containing a class (interface, enum, delegate) declaration to your clipboard.
2. In Visual Studio:
   - Right-click a folder in **Solution Explorer**, navigate to **Add**, and select "Add Class From Clipboard."
   - Or, open the **Project** menu and select "Add Class From Clipboard."
3. The extension will:
   - Parse the clipboard content.
   - Extract the class name.
   - Create a `.cs` file named after the type in the selected folder.
4. No file will be created if a valid type name cannot be extracted.

---
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
