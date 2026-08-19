using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems; // マウス検知に必須

// マウスの進入（PointerEnter）と退出（PointerExit）を監視するインターフェースを実装
public class HoverZoomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _animationDuration = 0.15f; // 大きくなるまでの時間
    [SerializeField] private Vector3 _hoverScale = new Vector3(1.1f, 1.1f, 1.1f); // ホバー時のサイズ（1.1倍）

    private Vector3 _originalScale;
    private Coroutine _zoomCoroutine;

    private void Awake()
    {
        _originalScale = transform.localScale;
    }

    // マウスがUIの上に乗った瞬間にUnityが自動で呼び出す
    public void OnPointerEnter(PointerEventData eventData)
    {
        StartZoom(_hoverScale);
    }

    // マウスがUIの上から離れた瞬間にUnityが自動で呼び出す
    public void OnPointerExit(PointerEventData eventData)
    {
        StartZoom(_originalScale);
    }

    private void StartZoom(Vector3 targetScale)
    {
        // 既に動いているズームアニメーションがあれば安全に止める
        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        
        // 現在のサイズからターゲットのサイズへ向けてアニメーションを開始
        _zoomCoroutine = StartCoroutine(AnimateScale(transform.localScale, targetScale));
    }

    private IEnumerator AnimateScale(Vector3 startScale, Vector3 endScale)
    {
        float elapsedTime = 0f;

        while (elapsedTime < _animationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _animationDuration;

            // イージング（CubicOut：最初は速く、後半は滑らかに減速してピタッと止まる高級感）
            float tCubicOut = 1f - Mathf.Pow(1f - t, 3f);

            // startScale（現在のサイズ）を基準にするため、アニメーション途中でマウスを出し入れしてもカクつかない
            transform.localScale = Vector3.Lerp(startScale, endScale, tCubicOut);
            yield return null;
        }

        transform.localScale = endScale;
    }

    // エラー防止：UIが非表示になったらスケールを強制リセット
    private void OnDisable()
    {
        if (_zoomCoroutine != null) StopCoroutine(_zoomCoroutine);
        transform.localScale = _originalScale;
    }
}
