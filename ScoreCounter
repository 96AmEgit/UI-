using System.Collections;
using UnityEngine;
using TMPro; // TextMeshProを使用

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _duration = 1.0f; // 何秒かけてカウントアップするか

    private Coroutine _counterCoroutine;

    // 外部（ゲーム管理スクリプトなど）からこれを呼び出す
    public void SetScoreAnimated(int startValue, int endValue)
    {
        if (_counterCoroutine != null) StopCoroutine(_counterCoroutine);
        _counterCoroutine = StartCoroutine(CountUp(startValue, endValue));
    }

    private IEnumerator CountUp(int start, int end)
    {
        float elapsedTime = 0f;
        Vector3 originalScale = _scoreText.transform.localScale;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _duration;
            
            // カウントの進捗をイージング（最初は速く、後半はゆっくり）
            float tOut = 1f - Mathf.Pow(1f - t, 3f); 
            
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(start, end, tOut));
            _scoreText.text = currentValue.ToString("N0"); // カンマ区切り

            // カウントアップ中に文字を少しだけ小刻みに震わせる（バイブス演出）
            _scoreText.transform.localScale = originalScale * Random.Range(1.0f, 1.05f);

            yield return null;
        }

        // 最終値の確定と、フィニッシュの「ドン！」という強調
        _scoreText.text = end.ToString("N0");
        
        // 最後にちょっとだけ大きくなって戻る演出
        float punchTime = 0f;
        while (punchTime < 0.15f)
        {
            punchTime += Time.deltaTime;
            float t = punchTime / 0.15f;
            _scoreText.transform.localScale = Vector3.Lerp(originalScale * 1.2f, originalScale, t);
            yield return null;
        }
        
        _scoreText.transform.localScale = originalScale;
    }
}
