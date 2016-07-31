using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace StyleCopPlus.Refactorings
{
    /// <summary>
    /// Contains context information for syntax node refactoring.
    /// </summary>
    /// <typeparam name="T">Type of the syntax node to refactor.</typeparam>
    public struct SyntaxNodeRefactoringContext<T>
        where T : SyntaxNode
    {
        public SyntaxNodeRefactoringContext(
            Document document,
            SemanticModel semanticModel,
            SyntaxNode root,
            T target,
            TextSpan span,
            CancellationToken cancellationToken)
        {
            Document = document;
            SemanticModel = semanticModel;
            SyntaxRoot = root;
            TargetNode = target;
            Span = span;
            CancellationToken = cancellationToken;
        }

        public Document Document { get; private set; }

        public SemanticModel SemanticModel { get; private set; }

        public SyntaxNode SyntaxRoot { get; private set; }

        public T TargetNode { get; private set; }

        public TextSpan Span { get; set; }

        public CancellationToken CancellationToken { get; private set; }
    }
}