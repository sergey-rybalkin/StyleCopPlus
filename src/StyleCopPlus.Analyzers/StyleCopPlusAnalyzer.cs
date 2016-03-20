using System;
using Microsoft.CodeAnalysis.Diagnostics;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// Base class for all analyzers.
    /// </summary>
    public abstract class StyleCopPlusAnalyzer : DiagnosticAnalyzer
    {
        protected static bool IsGeneratedCode(SyntaxTreeAnalysisContext context)
        {
            return context.Tree.FilePath.EndsWith("Generated.cs", StringComparison.Ordinal);
        }
    }
}