using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StyleCopPlus.Refactorings
{
    /// <summary>
    /// Provides functionality to save invocation expression result into a local variable.
    /// </summary>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "CreateVariableFromInvocation")]
    public class CreateVariableFromInvocationRefactoringProvider :
        SyntaxNodeRefactoringProviderBase<InvocationExpressionSyntax>
    {
        protected override IEnumerable<CodeAction> GetActions(
            Document document,
            SemanticModel semanticModel,
            SyntaxNode root,
            TextSpan span,
            InvocationExpressionSyntax node,
            CancellationToken cancellationToken)
        {
            // Ensure that target invocation expression result is not assigned yet.
            if (node.Parent is EqualsValueClauseSyntax)
                return Enumerable.Empty<CodeAction>();

            CodeAction retVal = CodeAction.Create(
                "Create variable",
                t => CreateVariable(node, root, document));

            return new[] { retVal };
        }

        private static Task<Document> CreateVariable(
            InvocationExpressionSyntax node,
            SyntaxNode root,
            Document document)
        {
            var newNode =
                LocalDeclarationStatement(
                    VariableDeclaration(
                        IdentifierName("var"))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                Identifier("v"))
                            .WithInitializer(
                                EqualsValueClause(
                                    node.WithoutTrivia())))))
                .WithLeadingTrivia(node.Parent.GetLeadingTrivia());

            var newRoot = root.ReplaceNode(node.Parent, newNode);

            return Task.FromResult(document.WithSyntaxRoot(newRoot));
        }
    }
}