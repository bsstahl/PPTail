using PPTail.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PPTail.Content.Presentations.Extensions;

internal static class SourceFileExtensions
{
    internal static string ToMarkdown(this IEnumerable<SourceFile> presentations)
    {
        var presentationTypes = Enum.GetNames(typeof(Enumerations.PresentationFormat)).ToList();

        var contentBuilder = new StringBuilder();
        foreach (var presentationType in presentationTypes)
        {
            var typePath = $"\\{presentationType}\\";
            var typedPresentations = presentations
                .Where(p => p.RelativePath.Contains(typePath));

            if (typedPresentations.Any())
            {
                contentBuilder.AppendLine($"### {presentationType}\r\n");
                foreach (var presentation in typedPresentations)
                {
                    var path = Path.Combine(presentation.RelativePath, presentation.FileName);
                    var startOfName = presentation.RelativePath.LastIndexOf(presentationType) + presentationType.Length + 1;
                    var name = presentation.RelativePath.Substring(startOfName);
                    contentBuilder.AppendLine($"* [{name}]({path})");
                }
            }
        }
        var content = contentBuilder.ToString();
        return content;
    }

}
