/// <summary>
/// テキストを一文字ずつ表示（ルビ対応用）
/// </summary>
public class TextTypewriterRuby : TextTypewriter
{
    TextRuby _textRuby;

    protected override void Awake()
    {
        base.Awake();
        _textRuby = _tmpText.GetComponentInChildren<TextRuby>();
    }

    /// <summary>
    /// 表示するテキストを設定
    /// </summary>
    /// <param name="text"></param>
    protected override void SetDisplayText(string text)
    {
        _textRuby.SetText(text);
    }

    /// <summary>
    /// テキストの長さを取得
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected override int GetTextLength(string text)
    {
        int ruby_length = _textRuby != null ? _textRuby.GetRubyLength(text.Length - 1) : 0;
        return text.Length - ruby_length;
    }

    /// <summary>
    /// 表示するテキストの長さを取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected override int GetVisibleCharactersIndex(int index)
    {
        int ruby_length = _textRuby != null ? _textRuby.GetRubyLength(index) : 0;
        return index + ruby_length;
    }

}
