using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Provides functionality for splitting long lines by placing arguments or parameters on individual line.
    /// </summary>
    internal class ArgumentsListSplitter : CodeFormatterBase, ILongLineSplitter
    {
        private readonly SyntaxNode _targetNode;

        private readonly SyntaxNode _parentNode;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentsListSplitter"/> class.
        /// </summary>
        /// <param name="parent">Syntax node containing arguments list.</param>
        /// <param name="target">Arguments list node.</param>
        private ArgumentsListSplitter(SyntaxNode parent, SyntaxNode target)
        {
            _parentNode = parent;
            _targetNode = target;
        }

        /// <summary>
        /// Splits long code line into several smaller lines in order to match max line length requirement.
        /// </summary>
        /// <param name="editor">Editor for the target document.</param>
        public void SplitCodeLine(DocumentEditor editor)
        {
            SyntaxNode updatedNode = SplitCommaSeparatedList(_targetNode, _parentNode.GetLeadingTrivia());
            editor.ReplaceNode(_targetNode, updatedNode);
        }

        /// <summary>
        /// Attempts to create splitter for the specified node.
        /// </summary>
        /// <param name="node">Syntax node containing arguments list.</param>
        /// <param name="splitter">[out] Created splitter or null reference.</param>
        internal static bool TryCreateSplitterFor(SyntaxNode node, out ILongLineSplitter splitter)
        {
            var childNodes = node.DescendantNodes();

            foreach (var child in childNodes)
            {
                if (child is ArgumentListSyntax || child is ParameterListSyntax)
                {
                    splitter = new ArgumentsListSplitter(node, child);
                    return true;
                }
            }

            splitter = null;

            return false;
        }
    }
}