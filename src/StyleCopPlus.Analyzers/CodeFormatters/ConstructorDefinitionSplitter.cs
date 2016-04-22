using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Provides functionality for splitting long constructor definition into several lines.
    /// </summary>
    internal class ConstructorDefinitionSplitter : ILongLineSplitter
    {
        private readonly DocumentEditor _editor;

        private ConstructorDeclarationSyntax _node;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorDefinitionSplitter"/> class.
        /// </summary>
        /// <param name="editor">Editor for the document that contains target constructor definition.</param>
        /// <param name="node">Constructor definition syntax node.</param>
        internal ConstructorDefinitionSplitter(DocumentEditor editor, ConstructorDeclarationSyntax node)
        {
            _node = node;
            _editor = editor;
        }

        /// <summary>
        /// Splits long code line into several smaller lines in order to match max line length requirement.
        /// </summary>
        public void SplitCodeLine()
        {
            // We can move constructor parameters to the new line only if it has them.
            if (_node.ParameterList.Parameters.Count == 0)
                return;

            SyntaxTriviaList newCommaTrivia = SyntaxTriviaList.Create(SyntaxFactory.CarriageReturnLineFeed);
            newCommaTrivia = newCommaTrivia.AddRange(_node.GetLeadingTrivia());
            newCommaTrivia = newCommaTrivia.Add(SyntaxFactory.Whitespace("    "));

            var commas = _node.DescendantTokens()
                              .Where(token => token.IsKind(SyntaxKind.CommaToken))
                              .ToList();

            SyntaxToken openParen = _node.ParameterList
                                         .DescendantTokens()
                                         .Where(token => token.IsKind(SyntaxKind.OpenParenToken))
                                         .First();

            Dictionary<SyntaxToken, SyntaxToken> replacements =
                new Dictionary<SyntaxToken, SyntaxToken>(commas.Count + 1);

            foreach (var comma in commas)
                replacements[comma] = comma.WithTrailingTrivia(newCommaTrivia);

            replacements[openParen] = openParen.WithTrailingTrivia(newCommaTrivia);

            var updatedNode = _node.ReplaceTokens(
                replacements.Keys,
                (original, mayRewrite) => replacements[original]);

            _editor.ReplaceNode(_node, updatedNode);
        }
    }
}