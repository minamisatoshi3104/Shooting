using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// テキストを一文字ずつ表示
/// </summary>
public class TextTypewriter : MonoBehaviour
{
    public bool IsBusy { get; private set; }

    protected TextMeshProUGUI _tmpText;
    private Coroutine _playingCoroutine;

    private string _text = "";
    private float _delayDuration = 0.05f;   // 次の文字を表示するまでの時間

    protected virtual void Awake()
    {
        _tmpText = GetComponent<TextMeshProUGUI>();
        _text = _tmpText.text;
        // 文字を非表示にしておく
        DisplayTextInit();
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
    /// 次の文字を表示するまでの時間設定
    /// </summary>
    /// <param name="duration"></param>
    public void SetDelayDuration(float duration)
    {
        _delayDuration = duration;
    }

    /// <summary>
    /// 表示テキスト初期化
    /// </summary>
    public void DisplayTextInit()
    {
        SetMaxVisibleCharactersRate(0f);
    }

    /// <summary>
    /// 強制終了
    /// </summary>
    public void ForceEndProc()
    {
        if (_playingCoroutine == null) return;   // 再生中のコルーチンがない
        StopCoroutine(_playingCoroutine);
        EndProc();
    }

    /// <summary>
    /// 表示するテキストを設定
    /// </summary>
    /// <param name="text"></param>
    protected virtual void SetDisplayText(string text)
    {
        _tmpText.text = text;
    }

    /// <summary>
    /// テキストの長さを取得
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    protected virtual int GetTextLength(string text)
    {
        return text.Length;
    }

    /// <summary>
    /// 表示するテキストの長さを取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    protected virtual int GetVisibleCharactersIndex(int index)
    {
        return index;
    }

    /// <summary>
    /// テキスト更新
    /// </summary>
    private void RefreshText()
    {
        // テキスト変更
        SetDisplayText(_text);
        // 再生中は強制終了
        ForceEndProc();
        // 文字を非表示にする
        DisplayTextInit();
        // 一文字ずつ表示
        IsBusy = true;
        _playingCoroutine = StartCoroutine(TypewriterProc());
    }

    /// <summary>
    /// 一文字ずつ表示する
    /// </summary>
    /// <returns></returns>
    private IEnumerator TypewriterProc()
    {
        yield return null;  // テキスト更新待ち
        int count = GetTextLength(_tmpText.GetParsedText());
        float currentSec = 0f;
        float duration = _delayDuration * count;
        if (duration != 0f)
        {
            while (currentSec < duration)
            {
                yield return null;
                currentSec += Time.deltaTime;
                float rate = currentSec / duration;
                SetMaxVisibleCharactersRate(rate);
            }
        }
        EndProc();
    }

    /// <summary>
    /// 表示する文字数の割合を反映
    /// </summary>
    /// <param name="rate"></param>
    private void SetMaxVisibleCharactersRate(float rate)
    {
        int length = GetTextLength(_tmpText.GetParsedText()); 
        int index = (int)Mathf.Lerp(0, length, rate);
        _tmpText.maxVisibleCharacters = GetVisibleCharactersIndex(index);
    }

    /// <summary>
    /// 処理終了
    /// </summary>
    private void EndProc()
    {
        SetMaxVisibleCharactersRate(1f);
        _playingCoroutine = null;
        IsBusy = false;
    }
}