using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

// ホバー（Enter/Exit）と クリック（Down/Up）の4つの状態を完璧に管理
public class PerfectClickButton : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, 
    IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float _animationDuration = 0.1f; // アニメーションの速度（キレを出すために短め）
    [SerializeField] private Vector3 _hoverScale = new Vector3(1.08f, 1.08f, 1.08f); // ホバー時（少し大きく）
    [SerializeField] private Vector3 _clickScale = new Vector3(0.92f, 0.92f, 0.92f); // クリック時（キュッと沈む）

    private Vector3 _baseScale;
    private Coroutine _scaleCoroutine;
    private bool _isHovering = false;

    private void Awake()
    {
        _baseScale = transform.localScale;
    }

    // 1. マウスが乗ったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovering = true;
        TriggerScale(_hoverScale);
    }

    // 2. マウスが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovering = false;
        TriggerScale(_baseScale);
    }

    // 3. ボタンが押された瞬間（沈み込む）
    public void OnPointerDown(PointerEventData eventData)
    {
        TriggerScale(_clickScale);
    }

    // 4. ボタンから指・マウスが離れた瞬間（状態に応じて戻す）
    public void OnPointerUp(PointerEventData eventData)
    {
        // 指を離したときに、まだボタンの上にマウスがいるならホバーサイズへ、外に出ているなら通常サイズへ戻す
        Vector3 target = _isHovering ? _hoverScale : _baseScale;
        TriggerScale(target);
    }

    private void TriggerScale(Vector3 targetScale)
    {
        if (_scaleCoroutine != null) StopCoroutine(_scaleCoroutine);
        _scaleCoroutine = StartCoroutine(AnimateScale(transform.localScale, targetScale));
    }

    private IEnumerator AnimateScale(Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _animationDuration;

            // イージング（CubicOut：押し込みと戻りにキレを出す）
            float tCubicOut = 1f - Mathf.Pow(1f - t, 3f);

            transform.localScale = Vector3.Lerp(startScale, endScale, tCubicOut);
            yield return null;
        }

        transform.localScale = endScale;
    }

    private void OnDisable()
    {
        if (_scaleCoroutine != null) StopCoroutine(_scaleCoroutine);
        transform.localScale = _baseScale;
        _isHovering = false;
    }
}
