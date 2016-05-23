using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
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
        /// Splits code line by placing each fluent API call on its own line.
        /// </summary>
        /// <param name="editor">Editor for the target document.</param>
        public void SplitCodeLine(DocumentEditor editor)
        {
            SyntaxTriviaList newDotTrivia = SyntaxTriviaList.Create(SyntaxFactory.CarriageReturnLineFeed);
            newDotTrivia = newDotTrivia.AddRange(_parentNode.GetLeadingTrivia());
            newDotTrivia = newDotTrivia.Add(Indent);

            Dictionary<SyntaxToken, SyntaxToken> replacements =
                new Dictionary<SyntaxToken, SyntaxToken>(_fluentCalls.Count - 1);

            for (int index = 1; index < _fluentCalls.Count; index++)
            {
                SyntaxToken dot = _fluentCalls[index].ChildTokens()
                                                     .First(token => token.IsKind(SyntaxKind.DotToken));

                replacements[dot] = dot.WithTrailingTrivia(newDotTrivia);
            }

            var updatedNode = _parentNode.ReplaceTokens(
                replacements.Keys,
                (original, mayRewrite) => replacements[original]);

            editor.ReplaceNode(_parentNode, updatedNode);
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