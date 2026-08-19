using System.Collections;
using UnityEngine;
using TMPro;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField] private float _timePerChar = 0.05f; // 文字の表示速度

    private Coroutine _typewriterCoroutine;
    private bool _isComplete = true;
    private string _fullText;

    public bool IsComplete => _isComplete;

    public void PlayText(string text)
    {
        _fullText = text;
        if (_typewriterCoroutine != null) StopCoroutine(_typewriterCoroutine);
        _typewriterCoroutine = StartCoroutine(TypeText());
    }

    // プレイヤーがタップした時に呼ぶスキップ処理（UXに必須）
    public void SkipText()
    {
        if (_isComplete) return;
        
        if (_typewriterCoroutine != null) StopCoroutine(_typewriterCoroutine);
        _textComponent.text = _fullText;
        _isComplete = true;
    }

    private IEnumerator TypeText()
    {
        _isComplete = false;
        _textComponent.text = "";
        
        // 1文字ずつ追加していく
        foreach (char c in _fullText)
        {
            _textComponent.text += c;
            
            // ここに「文字が鳴るSE」の再生処理を入れると手触りが神になる
            // AudioManager.Instance.PlaySE("TextSound");

            yield return new WaitForSeconds(_timePerChar);
        }

        _isComplete = true;
    }
}
