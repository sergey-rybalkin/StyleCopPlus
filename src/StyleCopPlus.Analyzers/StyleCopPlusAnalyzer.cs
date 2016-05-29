using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// Base class for all analyzers.
    /// </summary>
    public abstract class StyleCopPlusAnalyzer : DiagnosticAnalyzer
    {
        private static readonly SyntaxKind[] MethodSyntaxKinds = new SyntaxKind[]
        {
            SyntaxKind.MethodDeclaration,
            SyntaxKind.ConstructorDeclaration,
            SyntaxKind.DestructorDeclaration,
            SyntaxKind.ConversionOperatorDeclaration,
            SyntaxKind.OperatorDeclaration
        };

        // Performance hack. All values in MethodSyntaxKinds are declared next to each other, so we don't have
        // to compare target value to each item in the array - range comparison is enough. 
        private static readonly ushort MethodSyntaxLowerBound = (ushort)MethodSyntaxKinds.Min();
        private static readonly ushort MethodSyntaxUppderBound = (ushort)MethodSyntaxKinds.Max();

        /// <summary>
        /// Checks whether specified code block is inside a method-like declaration.
        /// </summary>
        /// <param name="context">Analysis context for the block to check.</param>
        protected static bool IsInsideMethod(CodeBlockAnalysisContext context)
        {
            if (context.OwningSymbol.Kind != SymbolKind.Method)
                return false;

            SyntaxNode block = context.CodeBlock;
            ushort blockKind = (ushort)block.RawKind;

            return blockKind >= MethodSyntaxLowerBound && blockKind <= MethodSyntaxUppderBound;
        }
    }
}