using System.IO;
using HelloCode.Environment.Analyzer.CSharp.Models;
using HelloCode.Environment.Analyzer.CSharp.Syntax.Extensions;
using Humanizer;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Solution = HelloCode.Environment.Analyzer.CSharp.Models.Solution;

namespace HelloCode.Environment.Analyzer.CSharp
{
    internal static class SolutionParser
    {
        public static Solution Parse(Options options) =>
            new Solution(options.Name, GetSolutionName(options), ParseSyntaxRoot(options));

        private static string GetSolutionName(Options options) =>
            options.Name.Dehumanize().Pascalize();

        private static SyntaxNode ParseSyntaxRoot(Options options)
        {
            var implementationFile = GetImplementationFile(options);
            if (!implementationFile.Exists)
                return null;

            using var fileStream = implementationFile.OpenRead();
            var workspace = new AdhocWorkspace();
            var project = workspace.AddProject(implementationFile.DirectoryName, LanguageNames.CSharp);

            var sourceText = SourceText.From(fileStream);
            var document = project.AddDocument(implementationFile.Name, sourceText);
            return document.GetReducedSyntaxRoot();
        }

        private static FileInfo GetImplementationFile(Options options)
        {
            var implementationFileName = $"{GetSolutionName(options)}.cs";
            var implementationFilePath = Path.GetFullPath(Path.Combine(options.Directory, implementationFileName));

            return new FileInfo(implementationFilePath);
        }
    }
}