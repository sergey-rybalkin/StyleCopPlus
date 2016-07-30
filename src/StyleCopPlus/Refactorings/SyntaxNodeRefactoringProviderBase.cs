using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeRefactorings;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus.Refactorings
{
    /// <summary>
    /// Base class for refactoring providers that target specific syntax nodes.
    /// </summary>
    /// <typeparam name="T">Type of the target syntax nodes.</typeparam>
    public abstract class SyntaxNodeRefactoringProviderBase<T> : CodeRefactoringProvider
        where T : SyntaxNode
    {
        /// <summary>
        /// Computes one or more refactorings for the specified
        /// <see cref="T:Microsoft.CodeAnalysis.CodeRefactorings.CodeRefactoringContext" />.
        /// </summary>
        /// <param name="context">Code context to compute refactorings for.</param>
        public override async Task ComputeRefactoringsAsync(CodeRefactoringContext context)
        {
            Document document = context.Document;
            TextSpan span = context.Span;
            CancellationToken cancellationToken = context.CancellationToken;

            if (WorkspaceKind.MiscellaneousFiles == document.Project.Solution.Workspace.Kind)
                return;

            // Ignore if editor has selected text.
            if (!span.IsEmpty)
                return;

            var model = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
            var root = await model.SyntaxTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
            if (!root.Span.Contains(span))
                return;

            var node = root.FindNode(span, false, true);
            var foundNode = (T)node.AncestorsAndSelf().FirstOrDefault(n => n is T);
            if (null == foundNode)
                return;

            foreach (var action in GetActions(document, model, root, span, foundNode, cancellationToken))
                context.RegisterRefactoring(action);
        }

        protected abstract IEnumerable<CodeAction> GetActions(
            Document document,
            SemanticModel semanticModel,
            SyntaxNode root,
            TextSpan span,
            T node,
            CancellationToken cancellationToken);
    }
}