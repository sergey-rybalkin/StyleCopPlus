using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Provides functionality for splitting long line with several invocation expressions by placing each
    /// of them on its own line.
    /// </summary>
    public class FluentApiCallsSplitter : CodeFormatterBase, ILongLineSplitter
    {
        private readonly SyntaxNode _parentNode;

        private readonly List<InvocationExpressionSyntax> _fluentCalls;

        private FluentApiCallsSplitter(SyntaxNode parent, List<InvocationExpressionSyntax> fluentCalls)
        {
            _parentNode = parent;
            _fluentCalls = fluentCalls;
        }

        /// <summary>
        /// Splits code line.
        /// </summary>
        /// <param name="editor">Editor for the target document.</param>
        public void SplitCodeLine(DocumentEditor editor)
        {
            
        }

        /// <summary>
        /// Attempts to create splitter for the specified node.
        /// </summary>
        /// <param name="node">Syntax node containing arguments list.</param>
        /// <param name="splitter">[out] Created splitter or null reference.</param>
        internal static bool TryCreateSplitterFor(SyntaxNode node, out ILongLineSplitter splitter)
        {
            var invocations = node.DescendantNodes().OfType<InvocationExpressionSyntax>().ToList();

            if (invocations.Count > 1)
            {
                splitter = new FluentApiCallsSplitter(node, invocations);
                return true;
            }

            splitter = null;

            return false;
        }
    }
}