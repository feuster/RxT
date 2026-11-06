using System.Text;

namespace RxT;

internal static class ResultFormatter
{
    public static string FormatMatches(EvaluationResult result, bool includeGroups)
    {
        if (result.Error is not null)
        {
            return result.Error;
        }

        var builder = new StringBuilder();

        for (var i = 0; i < result.Matches.Count; i++)
        {
            var match = result.Matches[i];
            builder.Append('#').Append(i + 1).Append(' ');
            AppendEntry(builder, match.Index, match.Length, match.Value);

            if (!includeGroups)
            {
                continue;
            }

            foreach (var group in match.Groups)
            {
                builder.Append("    ").Append(group.Name).Append(' ');
                AppendEntry(builder, group.Index, group.Length, group.Value);
            }
        }

        return builder.ToString();
    }

    public static string FormatStatus(EvaluationResult result, bool hasPattern)
    {
        if (result.Error is not null)
        {
            return Loc.Get("status.error");
        }

        if (!hasPattern)
        {
            return Loc.Get("status.ready");
        }

        if (result.Truncated)
        {
            return Loc.Format("status.truncated", result.Matches.Count, RegexEvaluator.MaxMatches);
        }

        return result.Matches.Count == 0 ? Loc.Get("status.none") : Loc.Format("status.count", result.Matches.Count);
    }

    private static void AppendEntry(StringBuilder builder, int index, int length, string value)
    {
        builder.Append('[').Append(index).Append('+').Append(length).Append("] \"")
               .Append(MakeVisible(value)).AppendLine("\"");
    }

    private static string MakeVisible(string value) =>
        value.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t");
}
