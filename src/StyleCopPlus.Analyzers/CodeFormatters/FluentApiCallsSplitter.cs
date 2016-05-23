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

        private readonly List<ExpressionSyntax> _fluentCalls;

        private FluentApiCallsSplitter(SyntaxNode parent, List<ExpressionSyntax> fluentCalls)
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
            // Build a trivia with proper indentation level that will put all dots in one column.
            SyntaxTriviaList indentedNewLine = GetIndentationTrivia();

            Dictionary<SyntaxToken, SyntaxToken> replacements =
                new Dictionary<SyntaxToken, SyntaxToken>(_fluentCalls.Count - 1);

            for (int index = 1; index < _fluentCalls.Count - 1; index++)
            {
                SyntaxToken dot = _fluentCalls[index].ChildTokens()
                                                     .FirstOrDefault(t => t.IsKind(SyntaxKind.DotToken));

                if (dot.IsKind(SyntaxKind.None))
                    continue;

                replacements[dot] = dot.WithLeadingTrivia(indentedNewLine);
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
            List<ExpressionSyntax> invocations = new List<ExpressionSyntax>(16);
            BuildFluentInvocationsTree(node, invocations);

            if (invocations.Count > 3)
            {
                splitter = new FluentApiCallsSplitter(node, invocations);
                return true;
            }

            splitter = null;

            return false;
        }

        private static void BuildFluentInvocationsTree(SyntaxNode node, List<ExpressionSyntax> container)
        {
            SyntaxNode child = node.ChildNodes()
                                   .FirstOrDefault(c => c is InvocationExpressionSyntax ||
                                                        c is MemberAccessExpressionSyntax);
            if (null != child)
            {
                container.Add((ExpressionSyntax)child);
                BuildFluentInvocationsTree(child, container);
            }
        }

        private SyntaxTriviaList GetIndentationTrivia()
        {
            SyntaxToken firstDot = _parentNode.DescendantTokens().First(t => t.IsKind(SyntaxKind.DotToken));
            int indentationLevel = firstDot.Span.Start - _parentNode.FullSpan.Start;
            SyntaxTrivia indentation = SyntaxFactory.Whitespace(new string(' ', indentationLevel));

            SyntaxTriviaList indentedNewLine = SyntaxTriviaList.Create(SyntaxFactory.CarriageReturnLineFeed);

            return indentedNewLine.Add(indentation);
        }
    }
}