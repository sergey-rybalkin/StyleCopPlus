using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Provides functionality for splitting long constructor call into several lines.
    /// </summary>
    internal class ConstructorInvocationSplitter : CodeFormatterBase, ILongLineSplitter
    {
        private readonly DocumentEditor _editor;

        private readonly ObjectCreationExpressionSyntax _node;

        private readonly SyntaxNode _parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInvocationSplitter"/> class.
        /// </summary>
        /// <param name="editor">Editor for the target document.</param>
        /// <param name="parent">Parent node that contains constructor call.</param>
        /// <param name="node">Constructor call to split.</param>
        internal ConstructorInvocationSplitter(
            DocumentEditor editor,
            SyntaxNode parent,
            ObjectCreationExpressionSyntax node)
        {
            _node = node;
            _editor = editor;
            _parent = parent;
        }

        /// <summary>
        /// Splits long code line into several smaller lines in order to match max line length requirement.
        /// </summary>
        public void SplitCodeLine()
        {
            // We can move constructor parameters to the new line only if there is at least one.
            if (_node.ArgumentList.Arguments.Count == 0)
                return;

            SyntaxNode updatedNode = SplitArgumentsList(_node, _parent.GetLeadingTrivia());
            _editor.ReplaceNode(_node, updatedNode);
        }
    }
}