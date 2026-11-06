namespace RxT;

internal sealed record QuickReferenceEntry(string Pattern, string[] Description);

internal sealed record QuickReferenceGroup(string[] Title, IReadOnlyList<QuickReferenceEntry> Entries);

internal sealed record QuickReferenceItem(string Category, string Pattern, string Description);

internal static class QuickReference
{
    // Textreihenfolge je Eintrag: en, de, fr, es, it (siehe Loc.T).
    public static IReadOnlyList<QuickReferenceGroup> Groups { get; } =
    [
        new(Loc.T("Character classes", "Zeichenklassen", "Classes de caractères", "Clases de caracteres", "Classi di caratteri"),
        [
            new(@".", Loc.T("any character except \\n", "beliebiges Zeichen außer \\n", "n'importe quel caractère sauf \\n", "cualquier carácter excepto \\n", "qualsiasi carattere tranne \\n")),
            new(@"\d", Loc.T("digit", "Ziffer", "chiffre", "dígito", "cifra")),
            new(@"\D", Loc.T("non-digit", "keine Ziffer", "non-chiffre", "no es un dígito", "non cifra")),
            new(@"\w", Loc.T("word character [a-zA-Z0-9_] and Unicode letters", "Wortzeichen [a-zA-Z0-9_] und Unicode-Buchstaben", "caractère de mot [a-zA-Z0-9_] et lettres Unicode", "carácter de palabra [a-zA-Z0-9_] y letras Unicode", "carattere di parola [a-zA-Z0-9_] e lettere Unicode")),
            new(@"\W", Loc.T("non-word character", "kein Wortzeichen", "caractère hors mot", "carácter que no es de palabra", "carattere non di parola")),
            new(@"\s", Loc.T("whitespace", "Whitespace", "espace blanc", "espacio en blanco", "spazio bianco")),
            new(@"\S", Loc.T("non-whitespace", "kein Whitespace", "non-espace blanc", "no es espacio en blanco", "non spazio bianco")),
            new(@"[abc]", Loc.T("one of the characters a, b, c", "eines der Zeichen a, b, c", "l'un des caractères a, b, c", "uno de los caracteres a, b, c", "uno dei caratteri a, b, c")),
            new(@"[^abc]", Loc.T("none of the characters a, b, c", "keines der Zeichen a, b, c", "aucun des caractères a, b, c", "ninguno de los caracteres a, b, c", "nessuno dei caratteri a, b, c")),
            new(@"[a-z]", Loc.T("character range", "Zeichenbereich", "plage de caractères", "rango de caracteres", "intervallo di caratteri")),
            new(@"[a-z-[aeiou]]", Loc.T("character class subtraction", "Zeichenklassen-Subtraktion", "soustraction de classe de caractères", "sustracción de clase de caracteres", "sottrazione di classe di caratteri")),
            new(@"\p{L}", Loc.T("Unicode category letter", "Unicode-Kategorie Buchstabe", "catégorie Unicode lettre", "categoría Unicode letra", "categoria Unicode lettera")),
            new(@"\p{Lu}", Loc.T("Unicode category uppercase letter", "Unicode-Kategorie Großbuchstabe", "catégorie Unicode lettre majuscule", "categoría Unicode letra mayúscula", "categoria Unicode lettera maiuscola")),
            new(@"\p{N}", Loc.T("Unicode category number", "Unicode-Kategorie Zahl", "catégorie Unicode nombre", "categoría Unicode número", "categoria Unicode numero")),
            new(@"\P{L}", Loc.T("negation of the Unicode category", "Negation der Unicode-Kategorie", "négation de la catégorie Unicode", "negación de la categoría Unicode", "negazione della categoria Unicode")),
        ]),
        new(Loc.T("Anchors", "Anker", "Ancres", "Anclas", "Ancore"),
        [
            new(@"^", Loc.T("start of line/text (see Multiline)", "Zeilen-/Textanfang (Multiline beachten)", "début de ligne/texte (voir Multiline)", "inicio de línea/texto (ver Multiline)", "inizio di riga/testo (vedi Multiline)")),
            new(@"$", Loc.T("end of line/text (see Multiline)", "Zeilen-/Textende (Multiline beachten)", "fin de ligne/texte (voir Multiline)", "fin de línea/texto (ver Multiline)", "fine di riga/testo (vedi Multiline)")),
            new(@"\A", Loc.T("start of text", "Textanfang", "début du texte", "inicio del texto", "inizio del testo")),
            new(@"\z", Loc.T("end of text", "Textende", "fin du texte", "fin del texto", "fine del testo")),
            new(@"\Z", Loc.T("end of text or before final \\n", "Textende oder vor letztem \\n", "fin du texte ou avant le dernier \\n", "fin del texto o antes del último \\n", "fine del testo o prima dell'ultimo \\n")),
            new(@"\G", Loc.T("end of previous match", "Ende des vorherigen Matches", "fin de la correspondance précédente", "fin de la coincidencia anterior", "fine della corrispondenza precedente")),
            new(@"\b", Loc.T("word boundary", "Wortgrenze", "limite de mot", "límite de palabra", "confine di parola")),
            new(@"\B", Loc.T("non-word boundary", "keine Wortgrenze", "pas une limite de mot", "no es límite de palabra", "non confine di parola")),
        ]),
        new(Loc.T("Quantifiers", "Quantifizierer", "Quantificateurs", "Cuantificadores", "Quantificatori"),
        [
            new(@"*", Loc.T("0 or more (greedy)", "0 oder mehr (greedy)", "0 ou plus (gourmand)", "0 o más (codicioso)", "0 o più (greedy)")),
            new(@"+", Loc.T("1 or more (greedy)", "1 oder mehr (greedy)", "1 ou plus (gourmand)", "1 o más (codicioso)", "1 o più (greedy)")),
            new(@"?", Loc.T("0 or 1 (greedy)", "0 oder 1 (greedy)", "0 ou 1 (gourmand)", "0 o 1 (codicioso)", "0 o 1 (greedy)")),
            new(@"{n}", Loc.T("exactly n repetitions", "genau n Wiederholungen", "exactement n répétitions", "exactamente n repeticiones", "esattamente n ripetizioni")),
            new(@"{n,}", Loc.T("at least n repetitions", "mindestens n Wiederholungen", "au moins n répétitions", "al menos n repeticiones", "almeno n ripetizioni")),
            new(@"{n,m}", Loc.T("n to m repetitions", "n bis m Wiederholungen", "de n à m répétitions", "de n a m repeticiones", "da n a m ripetizioni")),
            new(@"*?", Loc.T("0 or more (lazy)", "0 oder mehr (lazy)", "0 ou plus (paresseux)", "0 o más (perezoso)", "0 o più (lazy)")),
            new(@"+?", Loc.T("1 or more (lazy)", "1 oder mehr (lazy)", "1 ou plus (paresseux)", "1 o más (perezoso)", "1 o più (lazy)")),
            new(@"??", Loc.T("0 or 1 (lazy)", "0 oder 1 (lazy)", "0 ou 1 (paresseux)", "0 o 1 (perezoso)", "0 o 1 (lazy)")),
        ]),
        new(Loc.T("Groups and references", "Gruppen und Verweise", "Groupes et références", "Grupos y referencias", "Gruppi e riferimenti"),
        [
            new(@"(abc)", Loc.T("capture group", "Capture-Gruppe", "groupe capturant", "grupo de captura", "gruppo di cattura")),
            new(@"(?:abc)", Loc.T("non-capturing group", "Non-Capture-Gruppe", "groupe non capturant", "grupo sin captura", "gruppo non catturante")),
            new(@"(?<name>abc)", Loc.T("named group", "benannte Gruppe", "groupe nommé", "grupo con nombre", "gruppo con nome")),
            new(@"\1", Loc.T("backreference to group 1", "Rückverweis auf Gruppe 1", "référence arrière au groupe 1", "referencia inversa al grupo 1", "riferimento al gruppo 1")),
            new(@"\k<name>", Loc.T("backreference to named group", "Rückverweis auf benannte Gruppe", "référence arrière au groupe nommé", "referencia inversa al grupo con nombre", "riferimento al gruppo con nome")),
            new(@"a|b", Loc.T("alternation", "Alternative", "alternative", "alternancia", "alternativa")),
            new(@"(?>abc)", Loc.T("atomic group (no backtracking)", "atomare Gruppe (kein Backtracking)", "groupe atomique (sans backtracking)", "grupo atómico (sin backtracking)", "gruppo atomico (senza backtracking)")),
            new(@"(?(1)yes|no)", Loc.T("condition: group 1 exists", "Bedingung: Gruppe 1 vorhanden", "condition : le groupe 1 existe", "condición: el grupo 1 existe", "condizione: il gruppo 1 esiste")),
            new(@"(?<open>\()(?<-open>\))", Loc.T("balancing group", "Balancing Group", "groupe d'équilibrage", "grupo de equilibrio", "gruppo di bilanciamento")),
        ]),
        new(Loc.T("Lookaround", "Lookaround", "Lookaround", "Lookaround", "Lookaround"),
        [
            new(@"(?=abc)", Loc.T("positive lookahead", "Positive Lookahead", "lookahead positif", "lookahead positivo", "lookahead positivo")),
            new(@"(?!abc)", Loc.T("negative lookahead", "Negative Lookahead", "lookahead négatif", "lookahead negativo", "lookahead negativo")),
            new(@"(?<=abc)", Loc.T("positive lookbehind", "Positive Lookbehind", "lookbehind positif", "lookbehind positivo", "lookbehind positivo")),
            new(@"(?<!abc)", Loc.T("negative lookbehind", "Negative Lookbehind", "lookbehind négatif", "lookbehind negativo", "lookbehind negativo")),
        ]),
        new(Loc.T("Miscellaneous", "Sonstiges", "Divers", "Varios", "Varie"),
        [
            new(@"(?i)", Loc.T("inline option (i, m, s, x, n)", "Inline-Option (i, m, s, x, n)", "option en ligne (i, m, s, x, n)", "opción en línea (i, m, s, x, n)", "opzione inline (i, m, s, x, n)")),
            new(@"(?#text)", Loc.T("comment", "Kommentar", "commentaire", "comentario", "commento")),
            new(@"\t", Loc.T("tab", "Tabulator", "tabulation", "tabulador", "tabulazione")),
            new(@"\n", Loc.T("line feed", "Zeilenvorschub", "saut de ligne", "salto de línea", "avanzamento riga")),
            new(@"\r", Loc.T("carriage return", "Wagenrücklauf", "retour chariot", "retorno de carro", "ritorno a capo")),
            new(@"\xhh", Loc.T("character by hex code (2 digits)", "Zeichen per Hex-Code (2 Stellen)", "caractère par code hexadécimal (2 chiffres)", "carácter por código hexadecimal (2 dígitos)", "carattere tramite codice esadecimale (2 cifre)")),
            new(@"\uhhhh", Loc.T("character by Unicode code (4 digits)", "Zeichen per Unicode-Code (4 Stellen)", "caractère par code Unicode (4 chiffres)", "carácter por código Unicode (4 dígitos)", "carattere tramite codice Unicode (4 cifre)")),
        ]),
    ];

    /// <summary>Liefert die Einträge in der aktuellen Sprache (<see cref="Loc.Current"/>).</summary>
    public static IReadOnlyList<QuickReferenceItem> GetItems() => Groups
        .SelectMany(group => group.Entries.Select(entry =>
            new QuickReferenceItem(Loc.Pick(group.Title), entry.Pattern, Loc.Pick(entry.Description))))
        .ToList();
}
