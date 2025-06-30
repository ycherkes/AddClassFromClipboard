using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Shell;
using System.IO;
using System.Text;
using System.Windows;
using Command = Microsoft.VisualStudio.Extensibility.Commands.Command;

namespace AddClassFromClipboard;

[VisualStudioContribution]
internal class CopyFileContentsCommand : Command
{
    public override CommandConfiguration CommandConfiguration => new($"%{nameof(AddClassFromClipboard)}.{nameof(CopyFileContentsCommand)}.DisplayName%")
    {
        // https://github.com/VsixCommunity/Community.VisualStudio.VSCT/blob/main/src/Community.VisualStudio.VSCT/VSCT/VSGlobals.vsct
        Placements =
        [
            CommandPlacement.VsctParent(new Guid("d309f791-903f-11d0-9efc-00a0c911004f"), 0x02E6, 800),
            CommandPlacement.VsctParent(new Guid("d309f791-903f-11d0-9efc-00a0c911004f"), 0x0266, 800),
            CommandPlacement.VsctParent(new Guid("d309f791-903f-11d0-9efc-00a0c911004f"), 0x0267, 800)
        ],
        Icon = new CommandIconConfiguration(ImageMoniker.KnownValues.Copy, IconSettings.IconAndText)
    };

    public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        var dte = (DTE2)Package.GetGlobalService(typeof(DTE));
        var selectedItems = dte.SelectedItems;

        var filePaths = new List<string>();

        foreach (SelectedItem selectedItem in selectedItems)
        {
            if (selectedItem.ProjectItem is not null)
            {
                CollectFiles(selectedItem.ProjectItem, filePaths);
            }
            else if (selectedItem.Project is not null)
            {
                foreach (ProjectItem rootItem in selectedItem.Project.ProjectItems)
                {
                    CollectFiles(rootItem, filePaths);
                }
            }
        }

        var sb = new StringBuilder();

        foreach (var filePath in filePaths.Distinct())
        {
            if (!File.Exists(filePath))
            {
                continue;
            }

            try
            {
                sb.AppendLine($"// File: {Path.GetFileName(filePath)}");
                sb.AppendLine(File.ReadAllText(filePath));
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                sb.AppendLine($"// Error reading {filePath}: {ex.Message}");
            }
        }

        if (sb.Length > 0)
        {
            Clipboard.SetText(sb.ToString());
        }
    }

    private static void CollectFiles(ProjectItem item, List<string> filePaths)
    {
        ThreadHelper.ThrowIfNotOnUIThread();

        if (item is { Kind: Constants.vsProjectItemKindPhysicalFile, FileCount: > 0 })
        {
            filePaths.Add(item.FileNames[1]);
        }

        if (item.ProjectItems is not { Count: > 0 })
        {
            return;
        }

        foreach (ProjectItem subItem in item.ProjectItems)
        {
            CollectFiles(subItem, filePaths);
        }
    }
}