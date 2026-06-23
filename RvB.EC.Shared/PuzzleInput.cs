using RvB.Linq;
using System.Collections;
using SplitEnumerator = RvB.Linq.Iterable<RvB.Linq.Split, RvB.Linq.StringRange>;

namespace RvB.EC.Shared;

public class PuzzleInput : IEnumerable<char> {
    private static readonly string[] s_newlineSeparators = ["\r\n", "\n"];
    private static readonly string[] s_doubleNewlineSeparators = ["\r\n\r\n", "\n\n"];

    public static string[] LineSeparators { get; } = s_newlineSeparators;

    public static string[] SectionSeparators { get; } = s_doubleNewlineSeparators;

    private readonly StringRange _text;

    public PuzzleInput(string text) {
        _text = new(text);
    }

    public PuzzleSection AsSection(SpanSplitOptions splitOptions = SpanSplitOptions.None)
        => new(_text, splitOptions);

    public PuzzleSections AsSections(SpanSplitOptions splitOptions = SpanSplitOptions.None) {
        return new(_text, s_doubleNewlineSeparators, splitOptions);
    }

    public SplitEnumerator GetLines(SpanSplitOptions splitOptions = SpanSplitOptions.None)
        => _text.SplitAny(s_newlineSeparators, splitOptions);

    public IEnumerator<char> GetEnumerator() => ((IEnumerable<char>)_text).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator StringRange(PuzzleInput input) => input._text;

    public StringRange AsStringRange() => _text;
}

public ref struct PuzzleSection {
    private readonly StringRange _text;
    private readonly SpanSplitOptions _splitOptions;
    private int? _length;

    public PuzzleSection(StringRange text, SpanSplitOptions splitOptions = SpanSplitOptions.None) {
        _text = text;
        _splitOptions = splitOptions;
    }

    public int Length {
        get {
            _length ??= GetEnumerator().Count();
            return _length.Value;
        }
    }

    public readonly SplitEnumerator GetLines()
        => _text.SplitAny(PuzzleInput.LineSeparators, _splitOptions);

    public readonly SplitEnumerator GetEnumerator()
        => _text.SplitAny(PuzzleInput.LineSeparators, _splitOptions).GetEnumerator();

    public override readonly string ToString() => _text.ToString();

    public StringRange AsStringRange() => _text;

    public static implicit operator StringRange(PuzzleSection section) => section._text;
}

public ref struct PuzzleSections {
    private int? _length;
    private readonly StringRange _text;
    private readonly string[] _separators;
    private readonly SpanSplitOptions _splitOptions;
    private SectionEnumerator _enumerator;

    internal PuzzleSections(StringRange text, string[] separators, SpanSplitOptions splitOptions = SpanSplitOptions.None) {
        _text = text;
        _separators = separators;
        _splitOptions = splitOptions;
        _enumerator = new(_text, _separators, _splitOptions);
    }

    public int Length {
        get {
            if (_length == null) {
                var length = 0;
                var enumerator = GetEnumerator();
                while (enumerator.MoveNext()) {
                    length += 1;
                }
                _length = length;
            }
            return _length.Value;
        }
    }

    public bool TryTake(out PuzzleSection section1) {
        if (_text.Length == 0) {
            goto failed;
        }
        var enumerator = GetEnumerator();
        if (!enumerator.MoveNext())
            goto failed;
        section1 = enumerator.Current;
        _enumerator = enumerator;
        return true;
    failed:
        section1 = default;
        return false;
    }

    public bool TryTake(out PuzzleSection section1, out PuzzleSection section2) {
        var enumerator = GetEnumerator();
        if (!enumerator.MoveNext())
            goto failed;
        section1 = enumerator.Current;
        if (!enumerator.MoveNext())
            goto failed;
        section2 = enumerator.Current;
        _enumerator = enumerator;
        return true;
    failed:
        section1 = section2 = default;
        return false;
    }

    public bool TryTake(out PuzzleSection section1, out PuzzleSection section2, out PuzzleSection section3) {
        var enumerator = GetEnumerator();
        if (!enumerator.MoveNext())
            goto failed;
        section1 = enumerator.Current;
        if (!enumerator.MoveNext())
            goto failed;
        section2 = enumerator.Current;
        if (!enumerator.MoveNext())
            goto failed;
        section3 = enumerator.Current;
        _enumerator = enumerator;
        return true;
    failed:
        section1 = section2 = section3 = default;
        return false;
    }

    public bool TryTake(out PuzzleSection section1, out PuzzleSection section2, out PuzzleSection section3, out PuzzleSection section4) {
        var enumerator = GetEnumerator();
        if (!enumerator.MoveNext())
            goto failed;
        section1 = enumerator.Current;
        if (!enumerator.MoveNext())
            goto failed;
        section2 = enumerator.Current;
        if (!enumerator.MoveNext())
            goto failed;
        section3 = enumerator.Current;
        if (!enumerator.MoveNext())
            goto failed;
        section4 = enumerator.Current;
        _enumerator = enumerator;
        return true;
    failed:
        section1 = section2 = section3 = section4 = default;
        return false;
    }

    public void Deconstruct(out PuzzleSection section1, out PuzzleSection section2) {
        _ = TryTake(out section1, out section2);
    }

    public void Deconstruct(out PuzzleSection section1, out PuzzleSection section2, out PuzzleSection section3) {
        _ = TryTake(out section1, out section2, out section3);
    }

    public void Deconstruct(out PuzzleSection section1, out PuzzleSection section2, out PuzzleSection section3, out PuzzleSection section4) {
        _ = TryTake(out section1, out section2, out section3, out section4);
    }

    public readonly SectionEnumerator GetEnumerator() => _enumerator;

    public ref struct SectionEnumerator {
        private SplitEnumerator _enumerator;

        internal SectionEnumerator(StringRange text, string[] separators, SpanSplitOptions splitOptions) : this() {
            _enumerator = text.SplitAny(separators, splitOptions).GetEnumerator();
        }

        public readonly PuzzleSection Current
            => new(_enumerator.Current);

        public bool MoveNext()
            => _enumerator.MoveNext();
    }
}
