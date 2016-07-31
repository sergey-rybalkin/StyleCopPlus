using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;

namespace StyleCopPlus.CodeFixes
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

        /// <summary>
        /// Gets an optional <see cref="T:Microsoft.CodeAnalysis.CodeFixes.FixAllProvider" /> that can fix
        /// all/multiple occurrences of diagnostics fixed by this code fix provider. Return null if the
        /// provider doesn't support fix all/multiple occurrences. Otherwise, you can return any of the well
        /// known fix all providers from
        /// <see cref="T:Microsoft.CodeAnalysis.CodeFixes.WellKnownFixAllProviders" /> or implement your own
        /// fix all provider.
        /// </summary>
        public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;
    }
}