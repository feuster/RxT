RxT - Regex Tester
==================

Compact, portable Windows tool for testing regular expressions (.NET flavor)
against a given text.


Features
--------
- Live evaluation while typing, with highlighted matches in the input text
- Output of all matches with index and length, optionally including capture groups
- Regex options as checkboxes: IgnoreCase, Multiline, Singleline,
  IgnoreWhitespace, ExplicitCapture
- Line ending handling: original text is preserved on paste; optional conversion
  to CRLF, LF or CR; optional display of line endings
- Quick Reference of regex syntax elements; double-click inserts the pattern
  into the regex field
- Copy button for the current regex
- Themes: System (default), Light, Dark
- Languages: English (default), German, French, Spanish, Italian
- Settings (theme, language) are stored in RxT.settings.json next to RxT.exe


Requirements
------------
- Windows, 64-bit
- .NET 10 Desktop Runtime (x64): https://dotnet.microsoft.com/download/dotnet/10.0


Usage
-----
1. Start RxT.exe.
2. Enter or paste the text to test into the upper field.
3. Enter the regular expression into the middle field and select the options.
4. Matches are highlighted in the upper field and listed in the lower field.


Built with
----------
- WPF on .NET 10
- WPF-UI    https://github.com/lepoco/wpfui
- AvalonEdit https://github.com/icsharpcode/AvalonEdit


Note on the use of AI (voluntary)
---------------------------------
Parts of this project (source code, tests and documentation) were created with
the assistance of the AI language model Claude by Anthropic (Claude Sonnet 5.5;
only this one model version was used).


License
-------
MIT, Copyright (c) 2026 Alexander Feuster
Homepage: https://github.com/feuster/RxT
