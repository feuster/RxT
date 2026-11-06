namespace RxT;

/// <summary>Unterstützte UI-Sprachen. Die Reihenfolge bestimmt den Index in den Textarrays.</summary>
internal enum Language
{
    English,
    German,
    French,
    Spanish,
    Italian,
}

/// <summary>Lokalisierte UI-Texte (Reihenfolge je Eintrag: en, de, fr, es, it).</summary>
internal static class Loc
{
    public static readonly string[] NativeNames = ["English", "Deutsch", "Français", "Español", "Italiano"];

    private static readonly Dictionary<string, string[]> Table = new()
    {
        ["menu.settings"] = T("Settings", "Einstellungen", "Paramètres", "Configuración", "Impostazioni"),
        ["menu.theme"] = T("Theme", "Theme", "Thème", "Tema", "Tema"),
        ["menu.language"] = T("Language", "Sprache", "Langue", "Idioma", "Lingua"),
        ["menu.help"] = T("Help", "Hilfe", "Aide", "Ayuda", "Guida"),
        ["menu.about"] = T("About", "Info", "À propos", "Acerca de", "Informazioni"),
        ["menu.quickhelp"] = T("Quick help", "Kurzhilfe", "Aide rapide", "Ayuda rápida", "Guida rapida"),
        ["help.text"] = T(
            "Paste or type the input text at the top, then enter the regular expression below it. Matches are highlighted in the input text and listed in the results at the bottom.\n\n• Options: .NET RegexOptions; “Groups” also lists the capture groups.\n• Line ending: converts the input text; “Show EOL” makes line endings visible.\n• Quick reference (right): double-click inserts the pattern into the expression.\n• Copy button: copies the expression to the clipboard.\n• Settings: theme and language are saved.",
            "Oben den Eingabetext einfügen oder eingeben, darunter den regulären Ausdruck. Treffer werden im Eingabetext markiert und unten in den Ergebnissen aufgelistet.\n\n• Optionen: .NET-RegexOptions; „Gruppen“ listet zusätzlich die Capture-Gruppen auf.\n• Zeilenende: konvertiert den Eingabetext; „EOL anzeigen“ macht Zeilenenden sichtbar.\n• Quick Reference (rechts): Doppelklick fügt das Muster in den Ausdruck ein.\n• Kopieren-Button: kopiert den Ausdruck in die Zwischenablage.\n• Einstellungen: Theme und Sprache werden gespeichert.",
            "Saisissez ou collez le texte d'entrée en haut, puis l'expression régulière en dessous. Les correspondances sont surlignées dans le texte d'entrée et listées dans les résultats en bas.\n\n• Options : RegexOptions de .NET ; « Groupes » liste aussi les groupes de capture.\n• Fin de ligne : convertit le texte d'entrée ; « Afficher EOL » rend les fins de ligne visibles.\n• Référence rapide (à droite) : un double-clic insère le motif dans l'expression.\n• Bouton Copier : copie l'expression dans le presse-papiers.\n• Paramètres : le thème et la langue sont enregistrés.",
            "Escriba o pegue el texto de entrada arriba y, debajo, la expresión regular. Las coincidencias se resaltan en el texto de entrada y se listan en los resultados de abajo.\n\n• Opciones: RegexOptions de .NET; «Grupos» también lista los grupos de captura.\n• Fin de línea: convierte el texto de entrada; «Mostrar EOL» hace visibles los fines de línea.\n• Referencia rápida (derecha): el doble clic inserta el patrón en la expresión.\n• Botón copiar: copia la expresión al portapapeles.\n• Configuración: el tema y el idioma se guardan.",
            "Scrivi o incolla il testo di input in alto e, sotto, l'espressione regolare. Le corrispondenze sono evidenziate nel testo di input ed elencate nei risultati in basso.\n\n• Opzioni: RegexOptions di .NET; «Gruppi» elenca anche i gruppi di cattura.\n• Fine riga: converte il testo di input; «Mostra EOL» rende visibili i fine riga.\n• Riferimento rapido (a destra): il doppio clic inserisce il modello nell'espressione.\n• Pulsante copia: copia l'espressione negli appunti.\n• Impostazioni: tema e lingua vengono salvati."),
        ["theme.system"] = T("System", "System", "Système", "Sistema", "Sistema"),
        ["theme.light"] = T("Light", "Hell", "Clair", "Claro", "Chiaro"),
        ["theme.dark"] = T("Dark", "Dunkel", "Sombre", "Oscuro", "Scuro"),
        ["about.version"] = T("Version", "Version", "Version", "Versión", "Versione"),
        ["pattern.placeholder"] = T("Regular expression", "Regex-Ausdruck", "Expression régulière", "Expresión regular", "Espressione regolare"),
        ["pattern.copy"] = T("Copy expression", "Ausdruck kopieren", "Copier l'expression", "Copiar expresión", "Copia espressione"),
        ["input.placeholder"] = T("Input text", "Eingabetext", "Texte d'entrée", "Texto de entrada", "Testo di input"),
        ["output.placeholder"] = T("Results", "Ergebnisse", "Résultats", "Resultados", "Risultati"),
        ["opt.multiline"] = T("^ and $ match per line", "^ und $ gelten je Zeile", "^ et $ s'appliquent à chaque ligne", "^ y $ se aplican a cada línea", "^ e $ valgono per ogni riga"),
        ["opt.singleline"] = T(". also matches \\n", ". trifft auch \\n", ". correspond aussi à \\n", ". también coincide con \\n", ". corrisponde anche a \\n"),
        ["opt.explicit"] = T("only named groups capture", "nur benannte Gruppen", "seuls les groupes nommés capturent", "solo capturan los grupos con nombre", "catturano solo i gruppi con nome"),
        ["opt.groups"] = T("Groups", "Gruppen", "Groupes", "Grupos", "Gruppi"),
        ["opt.showEol"] = T("Show EOL", "EOL anzeigen", "Afficher EOL", "Mostrar EOL", "Mostra EOL"),
        ["eol.label"] = T("Line ending:", "Zeilenende:", "Fin de ligne :", "Fin de línea:", "Fine riga:"),
        ["eol.original"] = T("Original", "Original", "Original", "Original", "Originale"),
        ["eol.tip"] = T("Converts the input text (Original = unchanged)", "Konvertiert den Eingabetext (Original = unverändert)", "Convertit le texte saisi (Original = inchangé)", "Convierte el texto de entrada (Original = sin cambios)", "Converte il testo di input (Originale = invariato)"),
        ["status.error"] = T("Invalid expression", "Fehler im Ausdruck", "Expression invalide", "Expresión no válida", "Espressione non valida"),
        ["status.ready"] = T("Ready", "Bereit", "Prêt", "Listo", "Pronto"),
        ["status.none"] = T("No match", "Kein Match", "Aucune correspondance", "Sin coincidencias", "Nessuna corrispondenza"),
        ["status.count"] = T("{0} match(es)", "{0} Match(es)", "{0} correspondance(s)", "{0} coincidencia(s)", "{0} corrispondenza/e"),
        ["status.truncated"] = T("{0}+ matches (limited to {1})", "{0}+ Matches (auf {1} begrenzt)", "{0}+ correspondances (limité à {1})", "{0}+ coincidencias (limitado a {1})", "{0}+ corrispondenze (limitato a {1})"),
        ["error.timeout"] = T("Timeout: evaluation aborted after {0} s.", "Timeout: Auswertung nach {0} s abgebrochen.", "Délai dépassé : évaluation interrompue après {0} s.", "Tiempo agotado: evaluación cancelada tras {0} s.", "Timeout: valutazione interrotta dopo {0} s."),
    };

    public static Language Current { get; set; } = Language.English;

    public static string[] T(string en, string de, string fr, string es, string it) => [en, de, fr, es, it];

    public static string Pick(string[] texts) => texts[(int)Current];

    public static string Get(string key) => Pick(Table[key]);

    public static string Format(string key, params object[] args) => string.Format(Get(key), args);
}
