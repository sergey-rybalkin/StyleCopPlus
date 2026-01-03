using System.Collections.Generic;
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
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "DeleteMethod")]
    public class DeleteMethodRefactoringProvider :
        SyntaxNodeRefactoringProviderBase<MethodDeclarationSyntax>
    {
        protected override IEnumerable<CodeAction> GetActions(
            SyntaxNodeRefactoringContext<MethodDeclarationSyntax> context)
        {
            // Make action available only on method identifier or parameters list
            if (!context.TargetNode.Identifier.Span.IntersectsWith(context.Span) &&
                !context.TargetNode.ParameterList.Span.IntersectsWith(context.Span))
            {
                yield break;
            }

            yield return CodeAction.Create("Delete method", t => DeleteNode(context));
        }

        private static Task<Document> DeleteNode(SyntaxNodeRefactoringContext<MethodDeclarationSyntax> ctx)
        {
            SyntaxNode newRoot = ctx.SyntaxRoot.RemoveNode(ctx.TargetNode, SyntaxRemoveOptions.KeepNoTrivia);

            return Task.FromResult(ctx.Document.WithSyntaxRoot(newRoot));
        }
    }
}
