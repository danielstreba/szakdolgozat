using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Simplification;

namespace HelloCode.Environment.Analyzer.CSharp.Syntax.Extensions
{
    internal static class DocumentExtensions
    {
        public static SyntaxNode GetReducedSyntaxRoot(this Document document)
        {
            var syntaxRoot = document.GetSyntaxRootAsync().GetAwaiter().GetResult();
            var documentToSimplify = document.WithSyntaxRoot(syntaxRoot.WithAdditionalAnnotations(Simplifier.Annotation));
            var reducedDocument = Simplifier.ReduceAsync(documentToSimplify).GetAwaiter().GetResult();
            var reducedSyntaxRoot = reducedDocument.GetSyntaxRootAsync().GetAwaiter().GetResult();

            return reducedSyntaxRoot.Simplify();
        }
    }
}