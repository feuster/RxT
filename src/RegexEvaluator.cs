using System.Text.RegularExpressions;

namespace RxT;

internal sealed record GroupInfo(string Name, int Index, int Length, string Value);

internal sealed record MatchInfo(int Index, int Length, string Value, IReadOnlyList<GroupInfo> Groups);

internal sealed record EvaluationResult(IReadOnlyList<MatchInfo> Matches, string? Error, bool Truncated)
{
    public static EvaluationResult Empty { get; } = new([], null, false);

    public static EvaluationResult Failure(string error) => new([], error, false);
}

internal static class RegexEvaluator
{
    public const int MaxMatches = 5000;

    private static readonly TimeSpan MatchTimeout = TimeSpan.FromSeconds(2);

    public static EvaluationResult Evaluate(string pattern, string input, RegexOptions options)
    {
        if (pattern.Length == 0)
        {
            return EvaluationResult.Empty;
        }

        try
        {
            return Collect(new Regex(pattern, options, MatchTimeout), input);
        }
        catch (RegexMatchTimeoutException)
        {
            return EvaluationResult.Failure(Loc.Format("error.timeout", MatchTimeout.TotalSeconds.ToString("0")));
        }
        catch (ArgumentException ex)
        {
            return EvaluationResult.Failure(ex.Message);
        }
    }

    private static EvaluationResult Collect(Regex regex, string input)
    {
        var matches = new List<MatchInfo>();

        foreach (Match match in regex.Matches(input))
        {
            if (matches.Count == MaxMatches)
            {
                return new EvaluationResult(matches, null, true);
            }

            matches.Add(ToMatchInfo(match));
        }

        return new EvaluationResult(matches, null, false);
    }

    private static MatchInfo ToMatchInfo(Match match)
    {
        var groups = match.Groups
            .Cast<Group>()
            .Skip(1)
            .Where(group => group.Success)
            .Select(group => new GroupInfo(group.Name, group.Index, group.Length, group.Value))
            .ToList();

        return new MatchInfo(match.Index, match.Length, match.Value, groups);
    }
}
