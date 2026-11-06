using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace RxT;

/// <summary>Zeichnet die Regex-Treffer als farbige Hintergründe in einen AvalonEdit-TextView.</summary>
internal sealed class MatchHighlighter : IBackgroundRenderer
{
    private static readonly Brush[] Brushes =
    [
        CreateBrush(0, 150, 255),
        CreateBrush(255, 170, 0),
    ];

    private readonly TextView _textView;
    private IReadOnlyList<MatchInfo> _matches = [];

    public MatchHighlighter(TextView textView)
    {
        _textView = textView;
        _textView.BackgroundRenderers.Add(this);
    }

    public KnownLayer Layer => KnownLayer.Selection;

    public void Update(IReadOnlyList<MatchInfo> matches)
    {
        _matches = matches;
        _textView.InvalidateLayer(Layer);
    }

    public void Draw(TextView textView, DrawingContext drawingContext)
    {
        if (_matches.Count == 0 || !textView.VisualLinesValid || textView.VisualLines.Count == 0)
        {
            return;
        }

        var visibleStart = textView.VisualLines[0].FirstDocumentLine.Offset;
        var visibleEnd = textView.VisualLines[^1].LastDocumentLine.EndOffset;
        var documentLength = textView.Document.TextLength;

        for (var i = 0; i < _matches.Count; i++)
        {
            var match = _matches[i];
            var matchEnd = match.Index + match.Length;

            if (match.Length == 0 || matchEnd > documentLength || matchEnd < visibleStart || match.Index > visibleEnd)
            {
                continue;
            }

            DrawMatch(textView, drawingContext, match, Brushes[i % Brushes.Length]);
        }
    }

    private static void DrawMatch(TextView textView, DrawingContext drawingContext, MatchInfo match, Brush brush)
    {
        var builder = new BackgroundGeometryBuilder { AlignToWholePixels = true, CornerRadius = 2 };
        builder.AddSegment(textView, new TextSegment { StartOffset = match.Index, Length = match.Length });

        var geometry = builder.CreateGeometry();
        if (geometry is not null)
        {
            drawingContext.DrawGeometry(brush, null, geometry);
        }
    }

    private static Brush CreateBrush(byte red, byte green, byte blue)
    {
        var brush = new SolidColorBrush(Color.FromArgb(95, red, green, blue));
        brush.Freeze();
        return brush;
    }
}
