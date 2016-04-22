using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Editing;

namespace StyleCopPlus.Analyzers.CodeFormatters
{
    /// <summary>
    /// Provides functionality for splitting long constructor call into several lines.
    /// </summary>
    internal class ConstructorInvocationSplitter : ILongLineSplitter
    {
        private readonly DocumentEditor _editor;

        private readonly ObjectCreationExpressionSyntax _node;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorInvocationSplitter"/> class.
        /// </summary>
        /// <param name="editor">Editor for the target document.</param>
        /// <param name="node">Constructor call to split.</param>
        internal ConstructorInvocationSplitter(DocumentEditor editor, ObjectCreationExpressionSyntax node)
        {
            _node = node;
            _editor = editor;
        }

        public void SplitCodeLine()
        {
            // We can move constructor parameters to the new line only if there is at least one.
            if (_node.ArgumentList.Arguments.Count == 0)
                return;


        }
    }
}