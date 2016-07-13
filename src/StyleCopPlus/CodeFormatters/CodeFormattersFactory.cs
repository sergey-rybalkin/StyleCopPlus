using Microsoft.CodeAnalysis;

namespace StyleCopPlus.CodeFormatters
{
    /// <summary>
    /// Factory method pattern implementation for code formatters.
    /// </summary>
    internal static class CodeFormattersFactory
    {
        /// <summary>
        /// Gets line splitter for the specified syntax node.
        /// </summary>
        /// <param name="node">Syntax node containing long code line.</param>
        internal static ILongLineSplitter CreateLineSplitter(SyntaxNode node)
        {
            ILongLineSplitter retVal;

            if (FluentApiCallsSplitter.TryCreateSplitterFor(node, out retVal))
                return retVal;
            else if (ArgumentsListSplitter.TryCreateSplitterFor(node, out retVal))
                return retVal;

            return null;
        }
    }
}