using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ICSharpCode.AvalonEdit.Document;

namespace RxT;

public partial class MainWindow : Wpf.Ui.Controls.FluentWindow
{
    private static readonly TimeSpan DebounceInterval = TimeSpan.FromMilliseconds(120);
    private static readonly Brush ErrorBrush = CreateErrorBrush();

    private readonly DispatcherTimer _debounce = new() { Interval = DebounceInterval };
    private readonly MatchHighlighter _highlighter;
    private readonly AppSettings _settings = AppSettings.Load();

    public MainWindow()
    {
        InitializeComponent();
        Loc.Current = _settings.Language;
        BuildLanguageMenu();
        ApplyTheme(_settings.Theme);
        ApplyLanguage();

        _highlighter = new MatchHighlighter(InputEditor.TextArea.TextView);
        _debounce.Tick += (_, _) => UpdateResults();
        InputEditor.TextChanged += (_, _) =>
        {
            UpdatePlaceholders();
            ScheduleUpdate();
        };
        CommandManager.AddPreviewExecutedHandler(InputEditor, OnInputEditorCommand);

        UpdateResults();
    }

    private static ICollectionView CreateReferenceView()
    {
        var view = new ListCollectionView(QuickReference.GetItems().ToList());
        view.GroupDescriptions.Add(new PropertyGroupDescription(nameof(QuickReferenceItem.Category)));
        return view;
    }

    private static Brush CreateErrorBrush()
    {
        var brush = new SolidColorBrush(Color.FromRgb(196, 43, 28));
        brush.Freeze();
        return brush;
    }

    private void ApplyTheme(AppTheme theme)
    {
        switch (theme)
        {
            case AppTheme.System:
                Wpf.Ui.Appearance.ApplicationThemeManager.ApplySystemTheme();
                break;
            default:
                Wpf.Ui.Appearance.ApplicationThemeManager.Apply(
                    theme == AppTheme.Dark ? Wpf.Ui.Appearance.ApplicationTheme.Dark : Wpf.Ui.Appearance.ApplicationTheme.Light,
                    Wpf.Ui.Controls.WindowBackdropType.Mica);
                break;
        }

        SystemThemeItem.IsChecked = theme == AppTheme.System;
        LightThemeItem.IsChecked = theme == AppTheme.Light;
        DarkThemeItem.IsChecked = theme == AppTheme.Dark;
    }

    private void OnThemeClick(object sender, RoutedEventArgs e)
    {
        _settings.Theme = ReferenceEquals(sender, DarkThemeItem) ? AppTheme.Dark
            : ReferenceEquals(sender, LightThemeItem) ? AppTheme.Light
            : AppTheme.System;
        ApplyTheme(_settings.Theme);
        _settings.Save();
    }

    private async void OnAboutClick(object sender, RoutedEventArgs e)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var version = assembly.GetName().Version!.ToString(3);
        var copyright = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()!.Copyright;
        await new Wpf.Ui.Controls.MessageBox
        {
            Title = Loc.Get("menu.about"),
            Content = $"RxT - Regex Tester\n{Loc.Get("about.version")} {version}\n\n{copyright}",
            CloseButtonText = "OK",
            Owner = this,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        }.ShowDialogAsync();
    }

    private async void OnQuickHelpClick(object sender, RoutedEventArgs e)
    {
        await new Wpf.Ui.Controls.MessageBox
        {
            Title = Loc.Get("menu.quickhelp"),
            Content = new TextBlock { Text = Loc.Get("help.text"), TextWrapping = TextWrapping.Wrap, MaxWidth = 520 },
            CloseButtonText = "OK",
            Owner = this,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
        }.ShowDialogAsync();
    }

    private void BuildLanguageMenu()
    {
        foreach (var language in Enum.GetValues<Language>())
        {
            var item = new MenuItem { Header = Loc.NativeNames[(int)language], Tag = language, IsCheckable = true };
            item.Click += OnLanguageClick;
            LanguageMenu.Items.Add(item);
        }
    }

    private void ApplyLanguage()
    {
        SettingsMenu.Header = Loc.Get("menu.settings");
        ThemeMenu.Header = Loc.Get("menu.theme");
        SystemThemeItem.Header = Loc.Get("theme.system");
        LightThemeItem.Header = Loc.Get("theme.light");
        DarkThemeItem.Header = Loc.Get("theme.dark");
        LanguageMenu.Header = Loc.Get("menu.language");
        HelpMenu.Header = Loc.Get("menu.help");
        QuickHelpItem.Header = Loc.Get("menu.quickhelp");
        AboutItem.Header = Loc.Get("menu.about");

        PatternBox.PlaceholderText = Loc.Get("pattern.placeholder");
        CopyButton.ToolTip = Loc.Get("pattern.copy");
        InputPlaceholder.Text = Loc.Get("input.placeholder");
        OutputPlaceholder.Text = Loc.Get("output.placeholder");
        MultilineBox.ToolTip = "RegexOptions.Multiline: " + Loc.Get("opt.multiline");
        SinglelineBox.ToolTip = "RegexOptions.Singleline: " + Loc.Get("opt.singleline");
        ExplicitCaptureBox.ToolTip = "RegexOptions.ExplicitCapture: " + Loc.Get("opt.explicit");
        GroupsBox.Content = Loc.Get("opt.groups");
        ShowEolBox.Content = Loc.Get("opt.showEol");
        EolLabel.Text = Loc.Get("eol.label");
        EolOriginalItem.Content = Loc.Get("eol.original");
        EolBox.ToolTip = Loc.Get("eol.tip");

        foreach (MenuItem item in LanguageMenu.Items)
        {
            item.IsChecked = (Language)item.Tag == Loc.Current;
        }

        ReferenceList.ItemsSource = CreateReferenceView();
    }

    private void OnLanguageClick(object sender, RoutedEventArgs e)
    {
        _settings.Language = (Language)((MenuItem)sender).Tag;
        Loc.Current = _settings.Language;
        ApplyLanguage();
        UpdateResults();
        _settings.Save();
    }

    private void OnCopyClick(object sender, RoutedEventArgs e)
    {
        if (PatternBox.Text.Length == 0)
        {
            return;
        }

        // Die Zwischenablage kann kurzzeitig von anderen Prozessen gesperrt sein (CLIPBRD_E_CANT_OPEN).
        for (var attempt = 0; attempt < 10; attempt++)
        {
            try
            {
                Clipboard.SetText(PatternBox.Text);
                return;
            }
            catch (COMException)
            {
                Thread.Sleep(50);
            }
        }
    }

    private void OnOptionChanged(object sender, RoutedEventArgs e) => ScheduleUpdate();

    private void ScheduleUpdate()
    {
        if (!IsInitialized)
        {
            return;
        }

        _debounce.Stop();
        _debounce.Start();
    }

    private void UpdateResults()
    {
        _debounce.Stop();

        var result = RegexEvaluator.Evaluate(PatternBox.Text, InputEditor.Text, ReadOptions());

        if (result.Error is null)
        {
            PatternBox.ClearValue(Control.ForegroundProperty);
        }
        else
        {
            PatternBox.Foreground = ErrorBrush;
        }

        InputEditor.Options.ShowEndOfLine = ShowEolBox.IsChecked == true;
        _highlighter.Update(result.Matches);
        OutputEditor.Text = ResultFormatter.FormatMatches(result, GroupsBox.IsChecked == true);
        UpdatePlaceholders();
        StatusText.Text = ResultFormatter.FormatStatus(result, PatternBox.Text.Length > 0);
    }

    private void UpdatePlaceholders()
    {
        InputPlaceholder.Visibility = InputEditor.Document.TextLength == 0 ? Visibility.Visible : Visibility.Collapsed;
        OutputPlaceholder.Visibility = OutputEditor.Document.TextLength == 0 ? Visibility.Visible : Visibility.Collapsed;
    }

    private RegexOptions ReadOptions() =>
        OptionsPanel.Children
            .OfType<CheckBox>()
            .Where(box => box.IsChecked == true && box.Tag is RegexOptions)
            .Aggregate(RegexOptions.None, (options, box) => options | (RegexOptions)box.Tag);

    /// <summary>Fügt Clipboard-Text unverändert ein, damit Zeilenenden (LF/CRLF/CR) erhalten bleiben.</summary>
    private void OnInputEditorCommand(object sender, ExecutedRoutedEventArgs e)
    {
        if (e.Command != ApplicationCommands.Paste || !Clipboard.ContainsText())
        {
            return;
        }

        e.Handled = true;
        var text = Clipboard.GetText(TextDataFormat.UnicodeText);
        var start = InputEditor.SelectionStart;
        InputEditor.Document.Replace(start, InputEditor.SelectionLength, text);
        InputEditor.Select(start + text.Length, 0);
    }

    private void OnEolSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (!IsInitialized || EolBox.SelectedItem is not ComboBoxItem { Tag: string name })
        {
            return;
        }

        var newLine = name switch
        {
            "CRLF" => "\r\n",
            "LF" => "\n",
            _ => "\r",
        };

        InputEditor.Text = TextUtilities.NormalizeNewLines(InputEditor.Text, newLine);
    }

    private void OnReferenceDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var source = e.OriginalSource as DependencyObject;
        if (source is null
            || ItemsControl.ContainerFromElement(ReferenceList, source) is null
            || ReferenceList.SelectedItem is not QuickReferenceItem item)
        {
            return;
        }

        PatternBox.SelectedText = item.Pattern;
        PatternBox.Focus();
    }
}
