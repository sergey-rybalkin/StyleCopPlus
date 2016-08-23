using System;
using System.Collections.Generic;
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
                yield break;

            string type = GetVariableType(context);
            if ("void" == type)
                yield break;

            yield return CodeAction.Create("Create implicit variable", t => CreateVariable(context, "var"));
            yield return CodeAction.Create("Create explicit variable", t => CreateVariable(context, type));
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
            ITypeSymbol type = null;

            IMethodSymbol method = symbolInfo.Symbol as IMethodSymbol;
            if (null != method && MethodKind.Constructor == method.MethodKind)
                type = method.ContainingType;
            else if (null != method && MethodKind.Constructor != method.MethodKind)
                type = method.ReturnType;

            IPropertySymbol property = symbolInfo.Symbol as IPropertySymbol;
            if (null != property)
                type = property.Type;

            if (null != type)
            {
                return type.ToMinimalDisplayString(
                    context.SemanticModel,
                    context.Span.Start,
                    SymbolDisplayFormat.MinimallyQualifiedFormat);
            }

            return "var";
        }
    }
}