using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
            string validationMethod =
                "String" == parameter.Type.Name ? "ArgumentNotEmpty" : "ArgumentNotNull";

            StatementSyntax newStatement = ExpressionStatement(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(@"Verify"),
                        IdentifierName(validationMethod)))
                .WithArgumentList(
                    ArgumentList(
                        SeparatedList<ArgumentSyntax>(
                            new SyntaxNodeOrToken[]
                            {
                                Argument(
                                    IdentifierName(parameter.Name)),
                                Token(SyntaxKind.CommaToken),
                                Argument(
                                    InvocationExpression(
                                        IdentifierName(@"nameof"))
                                    .WithArgumentList(
                                        ArgumentList(
                                            SingletonSeparatedList(
                                                Argument(
                                                    IdentifierName(parameter.Name))))))
                            }))));

            MethodDeclarationSyntax method =
                context.TargetNode.Ancestors().OfType<MethodDeclarationSyntax>().First();

            var firstNode = method.Body.ChildNodes().FirstOrDefault();
            var newBody = null != firstNode ?
                method.Body.InsertNodesBefore(firstNode, new[] { newStatement }) :
                Block(newStatement);
            var newRoot = context.SyntaxRoot.ReplaceNode(method.Body, newBody);

            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
        }
    }
}