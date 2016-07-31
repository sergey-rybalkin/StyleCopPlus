using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StyleCopPlus.Refactorings
{
    /// <summary>
    /// Provides functionality to save invocation expression result into a local variable.
    /// </summary>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "CreateVariableFromInvocation")]
    public class CreateVariableFromInvocationRefactoringProvider :
        SyntaxNodeRefactoringProviderBase<ExpressionStatementSyntax>
    {
        private static readonly HashSet<Type> _supportedExpressionTypes = new HashSet<Type>
        {
            typeof(ObjectCreationExpressionSyntax),
            typeof(InvocationExpressionSyntax),
            typeof(MemberAccessExpressionSyntax)
        };

        protected override IEnumerable<CodeAction> GetActions(
            SyntaxNodeRefactoringContext<ExpressionStatementSyntax> context)
        {
            ExpressionStatementSyntax node = context.TargetNode;

            if (!_supportedExpressionTypes.Contains(node.Expression.GetType()))
                return Enumerable.Empty<CodeAction>();

            string type = GetVariableType(context);
            if ("void" == type)
                return Enumerable.Empty<CodeAction>();

            CodeAction implicitVariableAction = CodeAction.Create(
                "Create implicit variable",
                t => CreateVariable(context, "var"));

            CodeAction explicitVariableAction = CodeAction.Create(
                "Create explicit variable",
                t => CreateVariable(context, type));

            return new[] { implicitVariableAction, explicitVariableAction };
        }

        private static Task<Document> CreateVariable(
            SyntaxNodeRefactoringContext<ExpressionStatementSyntax> context,
            string variableType)
        {
            ExpressionSyntax node = context.TargetNode.Expression;
            SymbolInfo symbol = context.SemanticModel.GetSymbolInfo(node);

            SyntaxNode newNode =
                LocalDeclarationStatement(
                    VariableDeclaration(
                        IdentifierName(variableType))
                    .WithVariables(
                        SingletonSeparatedList(
                            VariableDeclarator(
                                Identifier("v"))
                            .WithInitializer(
                                EqualsValueClause(
                                    node.WithoutTrivia())))))
                .WithLeadingTrivia(node.Parent.GetLeadingTrivia());

            SyntaxNode newRoot = context.SyntaxRoot.ReplaceNode(node.Parent, newNode);

            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
        }

        private static string GetVariableType(SyntaxNodeRefactoringContext<ExpressionStatementSyntax> context)
        {
            ExpressionSyntax node = context.TargetNode.Expression;
            SymbolInfo symbolInfo = context.SemanticModel.GetSymbolInfo(node);

            IMethodSymbol method = symbolInfo.Symbol as IMethodSymbol;
            if (null != method && MethodKind.Constructor == method.MethodKind)
            {
                return method.ContainingType.ToMinimalDisplayString(
                    context.SemanticModel,
                    context.Span.Start,
                    SymbolDisplayFormat.MinimallyQualifiedFormat);
            }
            else if (null != method && MethodKind.Constructor != method.MethodKind)
            {
                return method.ReturnType.ToMinimalDisplayString(
                    context.SemanticModel,
                    context.Span.Start,
                    SymbolDisplayFormat.MinimallyQualifiedFormat);
            }

            IPropertySymbol property = symbolInfo.Symbol as IPropertySymbol;
            if (null != property)
            {
                return property.Type.ToMinimalDisplayString(
                    context.SemanticModel,
                    context.Span.Start,
                    SymbolDisplayFormat.MinimallyQualifiedFormat);
            }

            return "var";
        }
    }
}