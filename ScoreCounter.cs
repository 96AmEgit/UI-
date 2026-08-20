using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private float _duration = 1.0f; // シャッフルが続く長さ（秒）

    private Coroutine _counterCoroutine;
    private Vector3 _baseScale;

    private void Awake()
    {
        if (_scoreText != null)
        {
            _baseScale = _scoreText.transform.localScale;
        }
    }

    public void SetScoreAnimated(float startValue, float endValue)
    {
        if (_scoreText == null) return;

        if (_counterCoroutine != null) StopCoroutine(_counterCoroutine);
        _counterCoroutine = StartCoroutine(CountUp(startValue, endValue));
    }

    private IEnumerator CountUp(float start, float end)
    {
        float elapsedTime = 0f;

        // 【1. とぅるるるるる（数字の高速ランダムシャッフル）】
        // 位置や角度は一切動かさず、テキストの内容だけを文字通り「とぅるるる」と切り替えます
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;

            // 完全にランダムな「分」と「秒（ミリ秒含む）」を毎フレーム生成して表示
            int randomMinutes = Random.Range(0, 10); // 0〜9分でランダム
            float randomSeconds = Random.Range(0f, 59.99f); // 0〜59.99秒でランダム

            _scoreText.text = string.Format("{0:00}:{1:00.00}", randomMinutes, randomSeconds);

            yield return null;
        }

        // 【2. たん！（正しいクリアタイムの確定 ＋ 微小なブレ感）】
        // 最後に本来のクリアタイム（end）を表示します
        _scoreText.text = FormatToTime(end);

        // 世界観を崩さない極小の「たん！」演出
        // 派手に拡大させず、ほんの一瞬だけパッと文字が引き締まる（1.1倍から1.0倍へ戻る）上品な揺らぎです
        float punchDuration = 0.12f;
        float punchTime = 0f;

        while (punchTime < punchDuration)
        {
            punchTime += Time.deltaTime;
            float t = punchTime / punchDuration;

            // 直線的に元のサイズ（1.0）へピタッと戻す
            _scoreText.transform.localScale = Vector3.Lerp(_baseScale * 1.1f, _baseScale, t);

            yield return null;
        }

        // 完全に元のサイズに固定
        _scoreText.transform.localScale = _baseScale;
    }

    // 正しい秒数を「00:00.00」に変換する関数
    //クリアタイム用
    private string FormatToTime(float totalSeconds)
    {
        int minutes = (int)totalSeconds / 60;
        float seconds = totalSeconds % 60f;
        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
}
