using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Shell;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace AddClassFromClipboard
{
    /// <summary>
    /// Command1 handler.
    /// </summary>
    [VisualStudioContribution]
    internal class ClassFromClipboardCommand : Microsoft.VisualStudio.Extensibility.Commands.Command
    {
        private readonly TraceSource _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassFromClipboardCommand"/> class.
        /// </summary>
        /// <param name="traceSource">Trace source instance to utilize.</param>
        public ClassFromClipboardCommand(TraceSource traceSource)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this._logger = Requires.NotNull(traceSource, nameof(traceSource));
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new($"%{nameof(AddClassFromClipboard)}.{nameof(ClassFromClipboardCommand)}.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.Class, IconSettings.IconAndText),
            Placements = [
                //https://github.com/trufflesuite/trufflevsix/blob/master/packages/Microsoft.VSSDK.BuildTools.14.3.25407/tools/vssdk/inc/vsshlids.h#L747-L761
                CommandPlacement.VsctParent(new Guid("D309F791-903F-11D0-9EFC-00A0C911004F"), 0x014C, 800), //IDG_VS_PROJ_MISCADD - main menu
                CommandPlacement.VsctParent(new Guid("D309F791-903F-11D0-9EFC-00A0C911004F"), 0x0203, 800), //IDG_VS_CTXT_PROJECT_ADD_ITEMS  - solution explorer
            ],
            EnabledWhen = ActivationConstraint.SolutionState(SolutionState.FullyLoaded),
            Shortcuts = [new CommandShortcutConfiguration(ModifierKey.Control, Key.E, ModifierKey.Control, Key.V)]
        };

        /// <inheritdoc />
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            var dte = (DTE2)Package.GetGlobalService(typeof(DTE));
            if (dte == null) return;

            if (dte.SelectedItems.Count == 0) return;

            var clipboardContent = Clipboard.GetText();
            if (string.IsNullOrWhiteSpace(clipboardContent)) return;

            var newItemName = TryGetClassOrInterfaceOrEnumName(clipboardContent);

            if (string.IsNullOrWhiteSpace(newItemName))
                return;

            var selectedItem = dte.SelectedItems.Item(1);

            var project = selectedItem.Project ?? selectedItem.ProjectItem?.ContainingProject ?? dte.ActiveDocument?.ProjectItem?.ContainingProject;

            if (project == null) return;

            var fileName = selectedItem.Project?.FileName ?? (selectedItem.ProjectItem ?? dte.ActiveDocument?.ProjectItem)?.FileNames[1];

            var folder = Path.GetDirectoryName(fileName);

            if (string.IsNullOrWhiteSpace(folder)) return;

            var newClassPath = Path.Combine(folder, $"{newItemName}.cs");

            using (var writer = new StreamWriter(new FileStream(newClassPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)))
            {
                await writer.WriteAsync(clipboardContent);
            }

            project.ProjectItems.AddFromFile(newClassPath);
        }

        private string? TryGetClassOrInterfaceOrEnumName(string clipboardContent)
        {
            try
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(clipboardContent);
                var root = syntaxTree.GetRoot();

                var suitableNode = root.DescendantNodes()
                    .Select(x => x switch
                        {
                            BaseTypeDeclarationSyntax bts => bts.Identifier,
                            DelegateDeclarationSyntax dds => dds.Identifier,
                            _ => new SyntaxToken()
                        }
                    )
                    .FirstOrDefault(node => !string.IsNullOrWhiteSpace(node.Text));

                return suitableNode.Text;

            }
            catch (Exception e)
            {
                _logger.TraceEvent(TraceEventType.Warning, 1, "Error parsing clipboard text {0}", e);
                return null;
            }
        }
    }
}