using UnityEngine;
using System.Collections;

public class CoinView : MonoBehaviour
{
    private Vector3 _start;
    private Vector3 _end;
    private float _duration;
    private System.Func<float, float> _easing;
    [SerializeField] private GameObject _coinShadow;
    public void StartMove(Vector3 start, Vector3 end, float duration, System.Func<float, float> easing = null)
    {
        _start = start;
        _end = end;
        _duration = duration;
        _easing = easing ?? EaseInOutQuad;
        transform.position = _start;
        if (gameObject.activeSelf) {
            StartCoroutine(MoveCoroutine());
        }
    }

    private IEnumerator MoveCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < _duration)
        {
            float t = elapsed / _duration;
            float easedT = _easing(t);
            transform.position = Vector3.Lerp(_start, _end, easedT);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = _end;
        Destroy(gameObject);
    }

    void Update() {
        Instantiate(_coinShadow, transform.position, Quaternion.identity, transform);
    }

    // Default quadratic ease-in-out
    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }
}
