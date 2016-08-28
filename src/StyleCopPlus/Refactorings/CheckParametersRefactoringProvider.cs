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

            StatementSyntax validationStatement = ExpressionStatement(
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

            // Supports methods, constructors, operators etc. except for indexers.
            BaseMethodDeclarationSyntax method =
                context.TargetNode.Ancestors().OfType<BaseMethodDeclarationSyntax>().FirstOrDefault();

            if (null == method)
                return Task.FromResult(context.Document);

            var firstNode = method.Body.ChildNodes().FirstOrDefault();
            var newBody = null != firstNode ?
                method.Body.InsertNodesBefore(firstNode, new[] { validationStatement }) :
                Block(validationStatement);
            var newRoot = context.SyntaxRoot.ReplaceNode(method.Body, newBody);

            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
        }
    }
}