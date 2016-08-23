﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace StyleCopPlus.Refactorings
{
    /// <summary>
    /// Provides duplicate method refactorings.
    /// </summary>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "DuplicateMethod")]
    public class DuplicateMethodRefactoringProvider :
        SyntaxNodeRefactoringProviderBase<MethodDeclarationSyntax>
    {
        protected override IEnumerable<CodeAction> GetActions(
            SyntaxNodeRefactoringContext<MethodDeclarationSyntax> context)
        {
            // Make duplicate method action available only on method identifier
            if (!context.TargetNode.Identifier.Span.IntersectsWith(context.Span))
                yield break;

            yield return CodeAction.Create("Duplicate method", t => DuplicateNode(context));
        }

        private static Task<Document> DuplicateNode(
            SyntaxNodeRefactoringContext<MethodDeclarationSyntax> context)
        {
            SyntaxNode updatedNode = context.TargetNode;
            SyntaxTriviaList trivia = context.TargetNode.GetLeadingTrivia();

            // When necessary add additional empty line to separate new method with original
            if (trivia.Count < 2)
            {
                trivia = trivia.Insert(0, SyntaxFactory.CarriageReturnLineFeed);
                updatedNode = context.TargetNode.WithLeadingTrivia(trivia);
            }

            var newRoot = context.SyntaxRoot.InsertNodesAfter(context.TargetNode, new[] { updatedNode });

            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
        }
    }
}