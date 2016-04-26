﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Base class for all code formatters.
    /// </summary>
    public abstract class CodeFormatterBase
    {
        protected static readonly SyntaxTrivia Indent = SyntaxFactory.Whitespace("    ");

        protected SyntaxNode SplitArgumentsList(SyntaxNode node, SyntaxTriviaList baseIndentation)
        {
            SyntaxTriviaList newCommaTrivia = SyntaxTriviaList.Create(SyntaxFactory.CarriageReturnLineFeed);
            newCommaTrivia = newCommaTrivia.AddRange(baseIndentation);
            newCommaTrivia = newCommaTrivia.Add(Indent);

            var commas = node.DescendantTokens()
                             .Where(token => token.IsKind(SyntaxKind.CommaToken))
                             .ToList();

            SyntaxToken openParen = node.DescendantTokens()
                                        .Where(token => token.IsKind(SyntaxKind.OpenParenToken))
                                        .First();

            Dictionary<SyntaxToken, SyntaxToken> replacements =
                new Dictionary<SyntaxToken, SyntaxToken>(commas.Count + 1);

            foreach (var comma in commas)
                replacements[comma] = comma.WithTrailingTrivia(newCommaTrivia);

            replacements[openParen] = openParen.WithTrailingTrivia(newCommaTrivia);

            var updatedNode = node.ReplaceTokens(
                replacements.Keys,
                (original, mayRewrite) => replacements[original]);

            return updatedNode;
        }
    }
}