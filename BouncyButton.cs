using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BouncyButton : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private float _animationDuration = 0.2f;
    [SerializeField] private Vector3 _targetScale = new Vector3(0.9f, 0.9f, 1f); // 押し込んだ時のサイズ
    
    private Vector3 _originalScale;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    // クリック（タップ）された瞬間に実行
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(AnimateScale(_targetScale, _originalScale));
    }

    private IEnumerator AnimateScale(Vector3 target, Vector3 scaleBack)
    {
        float elapsedTime = 0f;

        // 1. 押し込むアニメーション（リニア）
        while (elapsedTime < _animationDuration * 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (_animationDuration * 0.5f);
            transform.localScale = Vector3.Lerp(_originalScale, target, t);
            yield return null;
        }

        elapsedTime = 0f;

        // 2. 指を離した風に戻る（イージング：少し行き過ぎて戻る弾力性）
        while (elapsedTime < _animationDuration * 0.5f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (_animationDuration * 0.5f);
            
            // イージング数式（BackOut効果：ぷるんと戻る）
            float c1 = 1.70158f;
            float c3 = c1 + 1f;
            float tBack = 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);

            transform.localScale = Vector3.LerpUnclamped(target, scaleBack, tBack);
            yield return null;
        }

        transform.localScale = scaleBack;
    }
}
