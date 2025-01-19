using System.Diagnostics;
using System.IO;
using System.Windows;
using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Shell;

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
        public override CommandConfiguration CommandConfiguration => new("%AddNewClassFromClipboard.ClassFromClipboardCommand.DisplayName%")
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
            Shortcuts = [new CommandShortcutConfiguration(ModifierKey.ControlShiftLeftAlt, Key.C)]
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

            var project = dte.SelectedItems.Item(1).Project ?? dte.ActiveDocument?.ProjectItem.ContainingProject;

            var projectItem = dte.SelectedItems.Item(1).ProjectItem ?? dte.ActiveDocument?.ProjectItem;

            project ??= projectItem?.ContainingProject;

            if (project == null) return;

            var folder = Path.GetDirectoryName(projectItem != null ?  projectItem.FileNames[1] : project.FileName);

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

                var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
                
                var className = classDeclaration?.Identifier.Text;

                if (className != null)
                {
                    return className;
                }

                var interfaceDeclaration = root.DescendantNodes().OfType<InterfaceDeclarationSyntax>().FirstOrDefault();

                var interfaceName = interfaceDeclaration?.Identifier.Text;

                if (interfaceName != null)
                {
                    return interfaceName;
                }

                var enumDeclaration = root.DescendantNodes().OfType<EnumDeclarationSyntax>().FirstOrDefault();

                return enumDeclaration?.Identifier.Text;

            }
            catch (Exception e)
            {
                _logger.TraceEvent(TraceEventType.Warning, 1, "Error parsing clipboard text {0}", e);
                return null;
            }
        }
    }
}