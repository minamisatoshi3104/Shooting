using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

/// <summary>
/// ルビ表示
/// <r=ルビ>漢字</r> で表示します
/// </summary>
public class TextRuby : MonoBehaviour
{
    static readonly Regex RubyRegex = new Regex("<r=\"?(?<ruby>.*?)\"?>(?<kanji>.*?)</r>", RegexOptions.IgnoreCase);
    static readonly string[] NewLineTexts = new string[] { "\n", "<br>" };

    private string _text;

    public class RubyInfo
    {
        public int StartIndex;
        public int Length;
    }
    List<RubyInfo> _rubyInfoList = new List<RubyInfo>();

    TextMeshProUGUI _tmpText;

    private void Awake()
    {
        _tmpText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SetText(_tmpText.text);
    }

    /// <summary>
    /// テキスト設定
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text)
    {
        _text = text;
        RefreshText();
    }

    /// <summary>
    /// 指定の文字までのルビの長さ
    /// </summary>
    /// <param name="index">指定の文字数</param>
    /// <returns>ルビの長さ</returns>
    public int GetRubyLength(int index)
    {
        int result_length = 0;
        for (int i = 0; i < _rubyInfoList.Count; i++)
        {
            if (index + result_length < _rubyInfoList[i].StartIndex) continue;
            result_length += _rubyInfoList[i].Length;
        }
        return result_length;
    }

    /// <summary>
    /// テキスト更新
    /// </summary>
    private void RefreshText()
    {
        string result_text = _text;
        _rubyInfoList.Clear();
        int ignore_text_length = 0;

        // ルビを置換する
        Match match = RubyRegex.Match(_text);
        while (match.Success)
        {
            string kanji = match.Groups["kanji"].Value;
            string ruby = match.Groups["ruby"].Value;

            float kanji_length = kanji.Length;
            float ruby_length = ruby.Length / 2f;    // ルビは半分の大きさ
            float ruby_offset = (kanji_length - ruby_length) / 2f; // 差分の片方分
            bool is_ruby_longer = kanji_length < ruby_length;

            // 改行の文字数取得
            string prev_text = _text.Substring(0, match.Index);
            int new_line_text_length = 0;
            for (int i = 0; i < NewLineTexts.Length; i++)
            {
                string target_text = NewLineTexts[i];
                int index = prev_text.IndexOf(target_text);
                while (index != -1)
                {
                    new_line_text_length += target_text.Length - 1;
                    index = prev_text.IndexOf(target_text, index + target_text.Length);
                };
            }

            // ルビのデータ保持
            RubyInfo ruby_info = new RubyInfo();
            ruby_info.StartIndex = match.Index + kanji.Length - ignore_text_length - new_line_text_length;
            ruby_info.Length = ruby.Length;
            _rubyInfoList.Add(ruby_info);
            ignore_text_length += match.Length - ruby.Length - kanji.Length;

            // タグを変換
            string replaceText = ruby;                                      // ルビを表示
            replaceText = AddSizeTag(replaceText, 50f);                     // 半分の大きさにする
            replaceText = AddSpaceFrontTag(replaceText, -kanji_length);      // 漢字の始まりに重なるように左に寄せる
            replaceText = AddSpaceFrontTag(replaceText, ruby_offset);        // ルビの始まりの位置を合わせる
            replaceText = AddSpaceBackTag(replaceText, ruby_offset);         // ルビの終わりの位置を合わせる
            replaceText = AddOneLineVOffsetTag(replaceText);                // 漢字の上に表示されるよう上げる
            replaceText = kanji + replaceText;                              // 漢字を表示
            // ルビの方が長い
            if (is_ruby_longer)
            {
                replaceText = AddSpaceFrontTag(replaceText, -ruby_offset);   // 文字の手前に隙間を空ける
                replaceText = AddSpaceBackTag(replaceText, -ruby_offset);    // 文字の後ろに隙間を空ける
            }
            result_text = result_text.Replace(match.Groups[0].Value, replaceText);
            match = match.NextMatch();
        }

        // 行間を固定
        var lineHeight = _tmpText.font.faceInfo.lineHeight / _tmpText.font.faceInfo.pointSize;
        result_text = $"<line-height={lineHeight:F3}em>{result_text}";

        // 一行目にルビがあるか
        int firstLineIndex = -1;
        for (int i = 0; i < NewLineTexts.Length; i++)
        {
            int index = _text.IndexOf(NewLineTexts[i]);
            if (firstLineIndex == -1 || firstLineIndex > index)
            {
                firstLineIndex = index;
            }
        }
        string firstLine = firstLineIndex != -1 ? _text.Substring(0, firstLineIndex + 1) : _text;

        // 一行目の高さを調整
        Vector4 margin = _tmpText.margin;
        margin.y = RubyRegex.IsMatch(firstLine) ? -_tmpText.fontSize * 0.5f : 0f;
        _tmpText.margin = margin;

        // 反映
        _tmpText.text = result_text;
    }

    /// <summary>
    /// 大きさを変更するタグ追加
    /// </summary>
    /// <param name="text">元のテキスト</param>
    /// <param name="percent">サイズをパーセント表示</param>
    /// <returns>結果のテキスト</returns>
    private string AddSizeTag(string text, float percent)
    {
        return $"<size={percent}%>{text}</size>";
    }

    /// <summary>
    /// 1文字分上に上げるオフセットタグ追加
    /// </summary>
    /// <param name="text">元のテキスト</param>
    /// <returns>結果のテキスト</returns>
    private string AddOneLineVOffsetTag(string text)
    {
        return $"<voffset=1em>{text}</voffset>";
    }

    /// <summary>
    /// 手前にスペースを追加するタグ追加
    /// </summary>
    /// <param name="text">元のテキスト</param>
    /// <param name="space">追加したいスペース（文字単位）</param>
    /// <returns>結果のテキスト</returns>
    private string AddSpaceFrontTag(string text, float space)
    {
        return $"<space={space}em>{text}";
    }

    /// <summary>
    /// 後ろにスペースを追加するタグ追加
    /// </summary>
    /// <param name="text">元のテキスト</param>
    /// <param name="space">追加したいスペース（文字単位）</param>
    /// <returns>結果のテキスト</returns>
    private string AddSpaceBackTag(string text, float space)
    {
        return $"{text}<space={space}em>";
    }
}