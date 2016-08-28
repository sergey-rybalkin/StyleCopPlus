using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace StyleCopPlus.Refactorings
{
    /// <summary>
    /// Provides refactoring that creates a class field from constructor parameter.
    /// </summary>
    [ExportCodeRefactoringProvider(LanguageNames.CSharp, Name = "IntroduceField")]
    public class IntroduceFieldRefactoringProvider :
            SyntaxNodeRefactoringProviderBase<ParameterSyntax>
    {
        protected override IEnumerable<CodeAction> GetActions(
            SyntaxNodeRefactoringContext<ParameterSyntax> context)
        {
            ConstructorDeclarationSyntax ctor =
                context.TargetNode.Ancestors().OfType<ConstructorDeclarationSyntax>().FirstOrDefault();

            if (null == ctor)
                yield break;

            yield return CodeAction.Create("Introduce field", t => AddField(context, ctor));
            yield return CodeAction.Create("Introduce readonly field", t => AddField(context, ctor, true));
        }

        private Task<Document> AddField(
            SyntaxNodeRefactoringContext<ParameterSyntax> context,
            ConstructorDeclarationSyntax constructor,
            bool isReadonly = false)
        {
            IParameterSymbol symbol = context.SemanticModel.GetDeclaredSymbol(
                context.TargetNode,
                context.CancellationToken);

            StatementSyntax assignmentStatement =
                ExpressionStatement(
                    AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        IdentifierName("_" + symbol.Name),
                        IdentifierName(symbol.Name)));
            ConstructorDeclarationSyntax newConstructor = constructor.AddBodyStatements(assignmentStatement);
            ClassDeclarationSyntax oldClass = constructor.FirstAncestorOrSelf<ClassDeclarationSyntax>();
            if (null == oldClass)
                return Task.FromResult(context.Document);

            ClassDeclarationSyntax newClass = oldClass.ReplaceNode(constructor, newConstructor);
            string typeName = symbol.Type.ToMinimalDisplayString(context.SemanticModel, context.Span.Start);
            FieldDeclarationSyntax fieldDeclaration;
            if (isReadonly)
                fieldDeclaration = GenerateReadonlyFieldDeclaration(symbol.Name, typeName);
            else
                fieldDeclaration = GenerateFieldDeclaration(symbol.Name, typeName);

            newClass = newClass.WithMembers(newClass.Members.Insert(0, fieldDeclaration))
                               .WithAdditionalAnnotations(Formatter.Annotation);
            var newRoot = context.SyntaxRoot.ReplaceNode(oldClass, newClass);

            return Task.FromResult(context.Document.WithSyntaxRoot(newRoot));
        }

        private FieldDeclarationSyntax GenerateReadonlyFieldDeclaration(string name, string type)
        {
            return FieldDeclaration(
                       VariableDeclaration(
                           IdentifierName(type))
                       .WithVariables(
                           SingletonSeparatedList(
                               VariableDeclarator(
                                   Identifier("_" + name)))))
                       .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PrivateKeyword),
                                Token(SyntaxKind.ReadOnlyKeyword)));
        }

        private FieldDeclarationSyntax GenerateFieldDeclaration(string name, string type)
        {
            return FieldDeclaration(
                       VariableDeclaration(
                           IdentifierName(type))
                       .WithVariables(
                           SingletonSeparatedList(
                               VariableDeclarator(
                                   Identifier("_" + name)))))
                       .WithModifiers(
                            TokenList(
                                Token(SyntaxKind.PrivateKeyword)));
        }
    }
}