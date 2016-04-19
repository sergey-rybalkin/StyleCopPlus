using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;

namespace StyleCopPlus.Analyzers
{
    /// <summary>
    /// Base class for all code fix providers that contains common functionality and helper members;
    /// </summary>
    public abstract class StyleCopPlusCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// Custom implementation for Task.CompletedTask functionality that does not exist in .NET 4.5.
        /// </summary>
        protected static readonly Task CompletedTask = Task.FromResult(false);
    }
}