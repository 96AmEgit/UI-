using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHpBar : MonoBehaviour
{
    [SerializeField] private Slider _mainSlider;       // 前面の緑バー（すぐ減る）
    [SerializeField] private Slider _delayedSlider;    // 背面の赤バー（遅れて減る）
    [SerializeField] private float _smoothSpeed = 2f;  // 追従する速度

    private Coroutine _updateCoroutine;

    // HPが変動したときに外部からこれを1回だけ呼ぶ（Updateで監視しない＝高評価）
    public void UpdateHp(float currentHp, float maxHp)
    {
        float targetValue = currentHp / maxHp;
        _mainSlider.value = targetValue; // 前面は即座に変更

        if (_updateCoroutine != null) StopCoroutine(_updateCoroutine);
        _updateCoroutine = StartCoroutine(AnimateDelayedBar(targetValue));
    }

    private IEnumerator AnimateDelayedBar(float targetValue)
    {
        // 少しだけ待ってから背面のバーを減らし始める（手触りのテクニック）
        yield return new WaitForSeconds(0.2f);

        // 背面のバーがターゲット値に追いつくまで滑らかに補間（Mathf.Lerp）
        while (Mathf.Abs(_delayedSlider.value - targetValue) > 0.001f)
        {
            _delayedSlider.value = Mathf.Lerp(_delayedSlider.value, targetValue, Time.deltaTime * _smoothSpeed);
            yield return null;
        }

        _delayedSlider.value = targetValue;
    }
}
