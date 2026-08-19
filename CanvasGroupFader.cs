using System.Collections;
using UnityEngine;

public class CanvasGroupFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup; // フェード用
    [SerializeField] private RectTransform _panelTransform; // 移動用
    [SerializeField] private float _duration = 0.3f;
    [SerializeField] private Vector2 _startOffset = new Vector2(0f, -100f); // どこから滑り込ませるか

    private Vector2 _targetPosition;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _targetPosition = _panelTransform.anchoredPosition;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(Fade(0f, 1f, _panelTransform.anchoredPosition + _startOffset, _targetPosition));
    }

    public void Hide()
    {
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(Fade(1f, 0f, _panelTransform.anchoredPosition, _targetPosition + _startOffset, () => {
            gameObject.SetActive(false);
        }));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, Vector2 startPos, Vector2 endPos, System.Action onComplete = null)
    {
        float elapsedTime = 0f;
        _canvasGroup.alpha = startAlpha;
        _panelTransform.anchoredPosition = startPos;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _duration;

            // イージング（ExpoOut：シュッと動いてピタッと止まる高級感ある動き）
            float tExpo = (t == 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t);

            _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, tExpo);
            _panelTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, tExpo);

            yield return null;
        }

        _canvasGroup.alpha = endAlpha;
        _panelTransform.anchoredPosition = endPos;
        onComplete?.Invoke();
    }
}
