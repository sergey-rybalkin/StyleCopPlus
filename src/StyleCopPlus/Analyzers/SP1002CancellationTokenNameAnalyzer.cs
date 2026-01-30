using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// SP1002 rule analyzer - validates that cancellation token parameters have proper names.
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SP1002CancellationTokenNameAnalyzer : StyleCopPlusAnalyzer
    {
        public const string DiagnosticId = "SP1002";

        public const string Category = "Readability";

        public const string Title = "Follow cancellation token naming convention";

        public const string MessageFormat = "Change CancellationToken parameter name from {0} to {1}";

        public const string Description = "Method parameters that have a type of CancellationToken should " +
            "follow consistent predefined naming convention.";

        public const string TargetParameterName = @"ct";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(
            DiagnosticId,
            Title,
            MessageFormat,
            Category,
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: Description);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(Rule);

        /// <inheritdoc/>
        protected override void Register(CompilationStartAnalysisContext context, Settings settings)
        {
            context.RegisterSyntaxNodeAction(HandleSyntaxNode, SyntaxKind.Parameter);
        }

        /// <summary>
        /// Look for method parameters of type CancellationToken and make sure that they are properly named.
        /// </summary>
        /// <param name="context">Syntax node context for parameters.</param>
        private static void HandleSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is ParameterSyntax parameter)
            {
                if (parameter.Type?.ToString() != nameof(CancellationToken))
                    return;

                if (parameter.Identifier.ToString() is TargetParameterName)
                    return;

                // Check if parameter name is predefined by an overridden method or interface implementation.
                // Ignore those cases as they will trigger other diagnostics.
                if (IsOverrideOrInterfaceImplementation(context, parameter))
                    return;

                context.ReportDiagnostic(
                    Diagnostic.Create(
                        Rule,
                        parameter.Identifier.GetLocation(),
                        parameter.Identifier.Text.ToString(),
                        TargetParameterName));
            }
        }

        /// <summary>
        /// Checks if the parameter belongs to a method that overrides a base method or implements an
        /// interface.
        /// </summary>
        /// <param name="context">Syntax node context.</param>
        /// <param name="parameter">Parameter syntax node.</param>
        private static bool IsOverrideOrInterfaceImplementation(SyntaxNodeAnalysisContext context, ParameterSyntax parameter)
        {
            var methodDeclaration = parameter.Parent?.Parent as BaseMethodDeclarationSyntax;
            if (methodDeclaration == null)
                return false;

            IMethodSymbol methodSymbol = context.SemanticModel.GetDeclaredSymbol(methodDeclaration);
            if (methodSymbol == null)
                return false;

            if (methodSymbol.IsOverride)
                return true;

            // Check for explicit interface implementation
            if (methodSymbol.ExplicitInterfaceImplementations.Length > 0)
                return true;

            // Check for implicit interface implementation
            INamedTypeSymbol containingType = methodSymbol.ContainingType;
            foreach (INamedTypeSymbol interfaceType in containingType.AllInterfaces)
            {
                ISymbol implementation = containingType.FindImplementationForInterfaceMember(
                    interfaceType.GetMembers(methodSymbol.Name).FirstOrDefault());
                if (SymbolEqualityComparer.Default.Equals(implementation, methodSymbol))
                    return true;
            }

            return false;
        }
    }
}
