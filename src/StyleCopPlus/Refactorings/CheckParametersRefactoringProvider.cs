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
    /// Provides refactoring that adds method parameters validation.
    /// </summary>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "CheckParameters")]
    public class CheckParametersRefactoringProvider :
        SyntaxNodeRefactoringProviderBase<ParameterSyntax>
    {
        protected override IEnumerable<CodeAction> GetActions(
            SyntaxNodeRefactoringContext<ParameterSyntax> context)
        {
            IParameterSymbol symbol = context.SemanticModel.GetDeclaredSymbol(
                context.TargetNode,
                context.CancellationToken);

            if (null == symbol ||
                !symbol.Type.IsReferenceType ||
                symbol.HasExplicitDefaultValue ||
                RefKind.None != symbol.RefKind)
            {
                yield break;
            }

            yield return CodeAction.Create("Validate parameter", t => AddValidation(context, symbol));
        }

        private static Task<Document> AddValidation(
            SyntaxNodeRefactoringContext<ParameterSyntax> context,
            IParameterSymbol parameter)
        {
            SyntaxFactory.ParseStatement(
                $"Verify.ArgumentNotNull({parameter.Name}, nameof(\"{parameter.Name}\"));");

            return Task.FromResult(context.Document);
        }
    }
}