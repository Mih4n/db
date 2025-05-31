namespace Domain.Lexer;

public class LexerContext
{
    public Token? Keyword { get; set; }

    public string Text { get; }
    public int Position { get; set; }

    public LexerContext(string text) => Text = text;

    public bool IsAtEnd => Position >= Text.Length;
    public char Peek() => Text[Position];
    public void Advance() => Position++;
    public string ReadWhile(Func<char, bool> predicate)
    {
        var start = Position;
        while (!IsAtEnd && predicate(Peek()))
            Advance();
        return Text[start..Position];
    }
}

