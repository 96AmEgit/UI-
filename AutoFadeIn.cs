using System.Collections;
using UnityEngine;

// CanvasGroupコンポーネントが必須になるように強制（バグ防止）
[RequireComponent(typeof(CanvasGroup))]
public class AutoFadeIn : MonoBehaviour
{
    [SerializeField] private float _fadeDuration = 0.4f; // フェードにかける時間（秒）
    
    private CanvasGroup _canvasGroup;
    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    // SetActive(true) が呼ばれた瞬間にUnityが自動で実行するイベント関数
    private void OnEnable()
    {
        // 初期状態は完全に透明にする
        if (_canvasGroup == null) _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;

        // 既に動いているフェードがあれば安全に止める
        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        
        // フェードイン開始
        _fadeCoroutine = StartCoroutine(FadeInRoutine());
    }

    private IEnumerator FadeInRoutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _fadeDuration;

            // イージング（セカンドオーダー：最初は速く、後半はじわ〜っと馴染む）
            float tQuadOut = t * (2f - t);

            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, tQuadOut);
            yield return null;
        }

        // 最後に確実に不透明度を1にする
        _canvasGroup.alpha = 1f;
    }
}
